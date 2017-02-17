namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data.Entity;
    using System.Web.UI;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.MonthlyReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
    using CollectingProductionDataSystem.Web.Areas.SummaryReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;
    using AutoMapper;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using Newtonsoft.Json;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels;
    using System.Diagnostics;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Application.MonthlyServices;
    using System.Text;

    public class SummaryReportsController : AreaBaseController
    {
        private const int HalfAnHour = 60 * 30;
        private readonly IUnitsDataService unitsDataService;
        private readonly IUnitDailyDataService dailyService;
        private readonly IInventoryTanksService tanksService;
        private readonly IPipelineServices pipes;

        public SummaryReportsController(IProductionData dataParam, IUnitsDataService unitsDataServiceParam, IUnitDailyDataService dailyServiceParam,
            IInventoryTanksService tanksParam, IPipelineServices pipesParam)
            : base(dataParam)
        {
            this.unitsDataService = unitsDataServiceParam;
            this.dailyService = dailyServiceParam;
            this.tanksService = tanksParam;
            this.pipes = pipesParam;
        }

        [HttpGet]
        public ActionResult TanksData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public ActionResult ReadTanksData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, int? areaId)
        {
            ValidateInputModel(date, parkId);

            if (this.ModelState.IsValid)
            {
                var statuses = this.tanksService.ReadDataForDay(date.Value, areaId, parkId).ToList();

                var dbResult = this.data.TanksData.All()
                    .Include(t => t.TankConfig)
                    .Include(t => t.TankConfig.Park)
                    .Where(t => t.RecordTimestamp == date
                        && (areaId == null || t.TankConfig.Park.AreaId == areaId)
                        && (parkId == null || t.ParkId == parkId))
                    .ToList();

                var vmResult = Mapper.Map<IEnumerable<TankDataViewModel>>(dbResult);
                foreach (var tank in vmResult)
                {
                    var status = statuses.Where(x => x.Tank.Id == tank.TankId).FirstOrDefault();
                    if (status != null && status.Quantity.TankStatus != null)
                    {
                        tank.StatusOfTank = status.Quantity.TankStatus.Id.ToString();
                    }
                }

                return Json(vmResult.OrderByDescending(x => x.TankName[0]).ThenBy(x => x.TankNumber).ToDataSourceResult(request, this.ModelState));
            }
            else
            {
                var kendoResult = new List<TankDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        [HttpGet]
        public ActionResult TanksWeightInVacuumData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public ActionResult ReadTanksWeightInVacuumData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, int? areaId)
        {
            ValidateUnitsReportModelState(date);

            if (this.ModelState.IsValid)
            {
                var products = this.data.Products.All().ToList();
                var dbResult = this.data.TanksData.All()
                    .Include(t => t.TankConfig)
                    .Include(t => t.TankConfig.Park)
                    .Include(t => t.Product)
                    .Where(t => t.RecordTimestamp == date
                        && (areaId == null || t.TankConfig.Park.AreaId == areaId)
                        && t.Product.Code > 0)
                    .GroupBy(x => x.Product.Code)
                    .ToDictionary(g => g.Key, g => g.Sum(v => v.WeightInVacuum));

                var weightInVacuumList = new List<WeightInVacuumDto>();
                foreach (var weight in dbResult)
                {
                    weightInVacuumList.Add(new WeightInVacuumDto
                    {
                        Id = weight.Key,
                        RecordTimestamp = date.Value,
                        Product = products.Where(p => p.Code == weight.Key).FirstOrDefault(),
                        WeightInVaccum = weight.Value.Value
                    });
                }

                var vmResult = Mapper.Map<IEnumerable<TankWeighInVacuumViewModel>>(weightInVacuumList);
                return Json(vmResult.OrderBy(x => x.ProductId).ToDataSourceResult(request, this.ModelState));
            }
            else
            {
                var kendoResult = new List<TankDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        private void ValidateTanksInputModel(DateTime? date, int? parkId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksDateSelector));
            }
        }

        [HttpGet]
        public ActionResult UnitsReportsData()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public JsonResult ReadUnitsReportsData([DataSourceRequest]
                                        DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            ValidateUnitsReportModelState(date);
            if (ModelState.IsValid)
            {
                var result = this.unitsDataService.GetConsolidatedShiftData(date.Value, processUnitId, factoryId);

                var kendoPreparedResult = Mapper.Map<IEnumerable<MultiShift>, IEnumerable<UnitsReportsDataViewModel>>(result);
                var kendoResult = new DataSourceResult();
                try
                {
                    kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                }
                catch (Exception ex1)
                {
                    Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
                }

                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new[] { new object() }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult UnitsDailyReportsData()
        {
            return View();
        }

        /// <summary>
        /// Validates the state of the model.
        /// </summary>
        /// <param name="date">The date.</param>
        private void ValidateUnitsReportModelState(DateTime? date)
        {

            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthorizeFactory]
        //[OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public JsonResult ReadDailyUnitsData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            ValidateDailyModelState(date);
            if (ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (ModelState.IsValid)
                {
                    var dbResult = this.unitsDataService.GetUnitsDailyApprovedDataForDateTime(date, processUnitId, factoryId);

                    var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsDailyData>, IEnumerable<UnitDailyDataViewModel>>(dbResult);
                    kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                }

                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DataConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadConfirmationData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            ValidateDailyModelState(date);
            if (!ModelState.IsValid)
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }

            //TODO: Need to refactory to also and by aprocess unit active period
            var SelectedFactories = data.ProcessUnits.All().Include(x => x.Factory)
                .Where(x => x.HasApprovedStatistics == true)
                .Where(x => x.Id == (processUnitId ?? x.Id)
                && x.Factory.Id == (factoryId ?? x.Factory.Id)).Select(x => new DataConfirmationViewModel()
                {
                    FactoryId = x.FactoryId,
                    FactoryName = x.Factory.ShortName,
                    ProcessUnitId = x.Id,
                    ProcessUnitName = x.ShortName
                }).OrderBy(x => x.FactoryId).ToList();
            var targetProcessUnitIds = SelectedFactories.Select(x => x.ProcessUnitId);
            var beginOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);
            var targetDate = date.Value.Date;
            var ConfirmedRecords = data.UnitsApprovedDatas.All().Where(x => x.RecordDate == targetDate && targetProcessUnitIds.Any(y => x.ProcessUnitId == y)).ToList();
            var ConfirmedDailyRecord = data.UnitsApprovedDailyDatas.All()
                .Where(x => beginOfMonth <= x.RecordDate
                    && x.RecordDate <= targetDate
                    && targetProcessUnitIds.Any(y => x.ProcessUnitId == y)).ToList();

            var isProcessUnitEnergyPreApproved = GetIsProcessUnitEnergyPreApproved();

            var IsEnergyCheckOff = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ComplitionEnergyCheckDeactivared"]);

            for (int i = 0; i < SelectedFactories.Count; i++)
            {
                var confirmationFirstShift = ConfirmedRecords.FirstOrDefault(x => x.ProcessUnitId == SelectedFactories[i].ProcessUnitId && x.ShiftId == (int)ShiftType.First);
                var confirmationSecondShift = ConfirmedRecords.FirstOrDefault(x => x.ProcessUnitId == SelectedFactories[i].ProcessUnitId && x.ShiftId == (int)ShiftType.Second);
                var confirmationThirdShift = ConfirmedRecords.FirstOrDefault(x => x.ProcessUnitId == SelectedFactories[i].ProcessUnitId && x.ShiftId == (int)ShiftType.Third);
                var confirmationOfDay = ConfirmedDailyRecord.FirstOrDefault(x => x.RecordDate == targetDate && x.ProcessUnitId == SelectedFactories[i].ProcessUnitId);
                var confirmationUntilTheDay = ConfirmedDailyRecord.Where(x => x.ProcessUnitId == SelectedFactories[i].ProcessUnitId
                                                                          && (x.EnergyApproved || IsEnergyCheckOff || isProcessUnitEnergyPreApproved[SelectedFactories[i].ProcessUnitId]))
                                                                          .Select(x => new DailyConfirmationViewModel()
                                                                            {
                                                                                Day = x.RecordDate,
                                                                                IsConfirmed = x.Approved,
                                                                            }).ToList();

                SelectedFactories[i].Shift1Confirmed = (confirmationFirstShift == null) ? false : confirmationFirstShift.Approved;
                SelectedFactories[i].Shift2Confirmed = (confirmationSecondShift == null) ? false : confirmationSecondShift.Approved;
                SelectedFactories[i].Shift3Confirmed = (confirmationThirdShift == null) ? false : confirmationThirdShift.Approved;
                SelectedFactories[i].DayMaterialConfirmed = (confirmationOfDay == null) ? false : confirmationOfDay.Approved;
                SelectedFactories[i].DayEnergyConfirmed = ((confirmationOfDay == null) ? false : confirmationOfDay.EnergyApproved)
                    || (isProcessUnitEnergyPreApproved.ContainsKey(SelectedFactories[i].ProcessUnitId) ? isProcessUnitEnergyPreApproved[SelectedFactories[i].ProcessUnitId] : false);
                SelectedFactories[i].DayConfirmed = (confirmationOfDay == null) ? false : confirmationOfDay.Approved && (confirmationOfDay.EnergyApproved || isProcessUnitEnergyPreApproved[SelectedFactories[i].ProcessUnitId]);
                SelectedFactories[i].ConfirmedDaysUntilTheDay = JsonConvert.SerializeObject(confirmationUntilTheDay ?? new List<DailyConfirmationViewModel>());
            }

            return Json(SelectedFactories.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// Gets the is process unit energy approvable.
        /// </summary>
        /// <param name="processUnitId">The process unit id.</param>
        /// <returns></returns>
        private Dictionary<int, bool> GetIsProcessUnitEnergyPreApproved()
        {
            var processUnits = this.data.UnitsDailyConfigs.All()
                .GroupBy(x => x.ProcessUnitId)
                .Select(x => new
                {
                    Id = x.Key,
                    NoEnergyPosition = !x.Any(y => y.MaterialType.Id == CommonConstants.EnergyType)
                }).ToDictionary(x => x.Id, x => x.NoEnergyPosition);

            return processUnits;
        }

        [HttpGet]
        public ActionResult ProductionPlanReport()
        {
            return View();
        }

        [AuthorizeFactory]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public JsonResult ReadProductionPlanData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            ValidateDailyModelState(date);
            if (ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (ModelState.IsValid)
                {
                    var dbResult = data.ProductionPlanDatas.All()
                        .Include(x => x.ProductionPlanConfig)
                        .Include(x => x.ProductionPlanConfig.ProcessUnit)
                        .Include(x => x.ProductionPlanConfig.ProcessUnit.Factory)
                        .Where(x =>
                            x.RecordTimestamp == date
                            && x.FactoryId == (factoryId ?? x.FactoryId)
                            && x.ProcessUnitId == (processUnitId ?? x.ProcessUnitId)
                            && x.ProductionPlanConfig.MaterialTypeId == CommonConstants.MaterialType
                            && x.ProductionPlanConfig.IsPropductionPlan == true
                        )
                        .OrderBy(x => x.ProductionPlanConfig.Code);

                    kendoResult = dbResult.ToDataSourceResult(request, ModelState, Mapper.Map<SummaryProductionPlanViewModel>);
                }

                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ProductionPlanEnergyReport()
        {
            return View();
        }

        [AuthorizeFactory]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public JsonResult ReadProductionPlanEnergyData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            ValidateDailyModelState(date);
            if (ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (ModelState.IsValid)
                {
                    var dbResult = data.ProductionPlanDatas.All()
                        .Include(x => x.ProductionPlanConfig)
                        .Include(x => x.ProductionPlanConfig.ProcessUnit)
                        .Include(x => x.ProductionPlanConfig.MeasureUnit)
                        .Include(x => x.ProductionPlanConfig.ProcessUnit.Factory)
                        .Where(x =>
                            x.RecordTimestamp == date
                            && x.FactoryId == (factoryId ?? x.FactoryId)
                            && x.ProcessUnitId == (processUnitId ?? x.ProcessUnitId)
                            && x.ProductionPlanConfig.MaterialTypeId == CommonConstants.EnergyType
                            && x.ProductionPlanConfig.IsPropductionPlan == true
                        )
                        .OrderBy(x => x.ProductionPlanConfig.Code);

                    kendoResult = dbResult.ToDataSourceResult(request, ModelState, Mapper.Map<EnergyProductionPlanDataViewModel>);
                }

                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DailyInfoHydrocarbonsData()
        {
            return View();
        }

        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public JsonResult ReadDailyInfoHydrocarbonsData([DataSourceRequest]
                                        DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            ValidateUnitsReportModelState(date);
            if (ModelState.IsValid)
            {
                IEnumerable<UnitsData> dbResult = this.unitsDataService.GetUnitsDataForDateTime(date, processUnitId, null)
                    .Where(x =>
                        x.UnitConfig.ShiftProductTypeId == CommonConstants.DailyInfoDailyInfoHydrocarbonsShiftTypeId
                        && (factoryId == null || x.UnitConfig.ProcessUnit.FactoryId == factoryId))
                    .ToList();
                // ToDo: On shifts changed to 2 must repair this code
                var result = dbResult.Select(x => new MultiShift
                {
                    TimeStamp = x.RecordTimestamp,
                    Factory = string.Format("{0:d2} {1}", x.UnitConfig.ProcessUnit.Factory.Id, x.UnitConfig.ProcessUnit.Factory.ShortName),
                    ProcessUnit = string.Format("{0:d2} {1}", x.UnitConfig.ProcessUnit.Id, x.UnitConfig.ProcessUnit.ShortName),
                    Code = x.UnitConfig.Code,
                    Position = x.UnitConfig.Position,
                    MeasureUnit = x.UnitConfig.MeasureUnit.Code,
                    ShiftProductType = string.Format("{0:d2} {1}", x.UnitConfig.ShiftProductType.Id, x.UnitConfig.ShiftProductType.Name),
                    UnitConfigId = x.UnitConfigId,
                    UnitName = x.UnitConfig.Name,
                    Shift1 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == (int)ShiftType.First).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                    Shift2 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == (int)ShiftType.Second).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                    Shift3 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == (int)ShiftType.Third).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                    NotATotalizedPosition = x.UnitConfig.NotATotalizedPosition,
                }).Distinct(new MultiShiftComparer()).ToList();

                var kendoPreparedResult = Mapper.Map<IEnumerable<MultiShift>, IEnumerable<UnitsReportsDataViewModel>>(result);

                var totalMonthQuantities = this.unitsDataService.GetTotalMonthQuantityToDayFromShiftData(date.Value, processUnitId ?? 0);

                foreach (var position in kendoPreparedResult)
                {
                    if (totalMonthQuantities.ContainsKey(position.Code))
                    {
                        if (position.NotATotalizedPosition)
                        {
                            var val = (double) position.Shift3QuantityValue;
                            if (double.IsInfinity(val) || double.IsNaN(val))
                            {
                                val = 0;
                            }
                            position.TotalMonthQuantity = val;
                        }
                        else
                        {
                            position.TotalMonthQuantity = totalMonthQuantities[position.Code];
                        }
                    }
                }

                var kendoResult = new DataSourceResult();
                try
                {
                    kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                }
                catch (Exception ex1)
                {
                    Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
                }

                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new[] { new object() }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult InnerPipelinesData()
        {
            this.ViewData["products"] = Mapper.Map<IEnumerable<ProductViewModel>>(this.data.Products.All()).ToList();
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public ActionResult ReadInnerPipelinesData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            ValidateDailyModelState(date);

            if (this.ModelState.IsValid)
            {
                List<InnerPipelineDto> dbResult = new List<InnerPipelineDto>();
                if (date.Value <= DateTime.Now)
                {
                    dbResult = this.pipes.ReadDataForMonth(date.Value).ToList();
                }

                var vmResult = Mapper.Map<IEnumerable<InnerPipelinesDataViewModel>>(dbResult).Where(x => x.Volume != 0 || x.Mass != 0).OrderBy(x => x.Product.Code);
                    var kendoResult = vmResult.ToDataSourceResult(request, ModelState);
                    return Json(kendoResult, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var kendoResult = new List<InnerPipelinesDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult InProcessUnitsData()
        {
            this.ViewData["products"] = Mapper.Map<IEnumerable<ProductViewModel>>(this.data.Products.All().ToList());
            this.ViewData["processunits"] = Mapper.Map<IEnumerable<CollectingProductionDataSystem.Web.ViewModels.Units.ProcessUnitViewModel>>(this.data.ProcessUnits.All().ToList());
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public ActionResult ReadInProcessUnitsData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            ValidateDailyModelState(date);

            if (this.ModelState.IsValid)
            {
                var dbResult = this.data.InProcessUnitDatas.All().Where(x => x.RecordTimestamp == date);
                var kendoResult = dbResult.ToDataSourceResult(request, this.ModelState, Mapper.Map<InProcessUnitsViewModel>);
                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kendoResult = new List<InProcessUnitsViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
        }

        private void ValidateDailyModelState(DateTime? date)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
        }

        private void ValidateInputModel(DateTime? date, int? parkId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksDateSelector));
            }
            if (!User.IsInRole("Administrator") && !User.IsInRole("AllParksReporter"))
            {
                if (parkId == null)
                {
                    this.ModelState.AddModelError("parks", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksParkSelector));
                }
            }
        }

        private List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }
    }
}