namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data.Entity;
    using System.Web.UI;
    using Constants;
    using Data.Contracts;
    using MonthlyReporting.ViewModels;
    using NomManagement.Models.ViewModels;
    using ViewModels;
    using Infrastructure.Extentions;
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;
    using AutoMapper;
    using Models.Inventories;
    using Web.ViewModels.Tank;
    using Newtonsoft.Json;
    using Resources = App_GlobalResources.Resources;
    using Application.Contracts;
    using Models.Productions;
    using DailyReporting.ViewModels;
    using System.Diagnostics;
    using Infrastructure.Filters;
    using Web.ViewModels.Units;
    using Application.MonthlyServices;
    using System.Text;

    public class SummaryReportsController : AreaBaseController
    {
        private const int HalfAnHour = 60 * 30;
        private readonly IUnitsDataService unitsDataService;
        private readonly IUnitDailyDataService dailyService;
        private readonly IInventoryTanksService tanksService;
        private readonly IPipelineServices pipes;
        private readonly IHistoricalService historicalService;

        public SummaryReportsController(IProductionData dataParam, IUnitsDataService unitsDataServiceParam, IUnitDailyDataService dailyServiceParam,
            IInventoryTanksService tanksParam, IPipelineServices pipesParam, IHistoricalService historicalServiceParam)
            : base(dataParam)
        {
            this.unitsDataService = unitsDataServiceParam;
            this.dailyService = dailyServiceParam;
            this.tanksService = tanksParam;
            this.pipes = pipesParam;
            this.historicalService = historicalServiceParam;
        }

        [HttpGet]
        public ActionResult TanksData()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public ActionResult ReadTanksData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, int? areaId)
        {
            this.ValidateInputModel(date, parkId);

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

                var vmResult = Mapper.Map<IEnumerable<TankDataViewModel>>(dbResult).ToList();
                foreach (var tank in vmResult)
                {
                    var status = statuses.FirstOrDefault(x => x.Tank.Id == tank.TankId);
                    if (status?.Quantity.TankStatus != null)
                    {
                        tank.StatusOfTank = status.Quantity.TankStatus.Id.ToString();
                    }
                }

                return this.Json(vmResult.OrderByDescending(x => x.TankName[0]).ThenBy(x => x.TankNumber).ToDataSourceResult(request, this.ModelState));
            }
            else
            {
                var kendoResult = new List<TankDataViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult);
            }
        }

        [HttpGet]
        public ActionResult TanksWeightInVacuumData()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public ActionResult ReadTanksWeightInVacuumData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, int? areaId)
        {
            this.ValidateUnitsReportModelState(date);

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
                        Product = products.FirstOrDefault(p => p.Code == weight.Key),
                        WeightInVaccum = weight.Value.Value
                    });
                }

                var vmResult = Mapper.Map<IEnumerable<TankWeighInVacuumViewModel>>(weightInVacuumList);
                return this.Json(vmResult.OrderBy(x => x.ProductId).ToDataSourceResult(request, this.ModelState));
            }
            else
            {
                var kendoResult = new List<TankDataViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult);
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
            return this.View();
        }

        [HttpGet]
        public ActionResult UnitsDailyReportsData()
        {
            return this.View();
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
            this.ValidateDailyModelState(date);
            if (this.ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (this.ModelState.IsValid)
                {
                    var dbResult = this.unitsDataService.GetUnitsDailyApprovedDataForDateTime(date, processUnitId, factoryId);

                    var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsDailyData>, IEnumerable<UnitDailyDataViewModel>>(dbResult);
                    kendoResult = kendoPreparedResult.ToDataSourceResult(request, this.ModelState);
                }

                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DataConfirmation()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadConfirmationData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            this.ValidateDailyModelState(date);
            if (!this.ModelState.IsValid)
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult);
            }

            //TODO: Need to refactory to also and by aprocess unit active period
            var selectedFactories = this.data.ProcessUnits.All().Include(x => x.Factory)
                .Where(x => x.HasApprovedStatistics == true)
                .Where(x => x.Id == (processUnitId ?? x.Id)
                && x.Factory.Id == (factoryId ?? x.Factory.Id)).Select(x => new DataConfirmationViewModel()
                {
                    FactoryId = x.FactoryId,
                    FactoryName = x.Factory.ShortName,
                    ProcessUnitId = x.Id,
                    ProcessUnitName = x.ShortName
                }).OrderBy(x => x.FactoryId).ToList();
            var targetProcessUnitIds = selectedFactories.Select(x => x.ProcessUnitId);
            var beginOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);
            var targetDate = date.Value.Date;
            var ConfirmedRecords = this.data.UnitsApprovedDatas.All().Where(x => x.RecordDate == targetDate && targetProcessUnitIds.Any(y => x.ProcessUnitId == y)).ToList();
            var ConfirmedDailyRecord = this.data.UnitsApprovedDailyDatas.All()
                .Where(x => beginOfMonth <= x.RecordDate
                    && x.RecordDate <= targetDate
                    && targetProcessUnitIds.Any(y => x.ProcessUnitId == y)).ToList();

            var isProcessUnitEnergyPreApproved = this.GetIsProcessUnitEnergyPreApproved();

            var isEnergyCheckOff = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ComplitionEnergyCheckDeactivared"]);

            for (int i = 0; i < selectedFactories.Count; i++)
            {
                var confirmationFirstShift = ConfirmedRecords.FirstOrDefault(x => x.ProcessUnitId == selectedFactories[i].ProcessUnitId && x.ShiftId == (int)ShiftType.First);
                var confirmationSecondShift = ConfirmedRecords.FirstOrDefault(x => x.ProcessUnitId == selectedFactories[i].ProcessUnitId && x.ShiftId == (int)ShiftType.Second);
                var confirmationThirdShift = ConfirmedRecords.FirstOrDefault(x => x.ProcessUnitId == selectedFactories[i].ProcessUnitId && x.ShiftId == (int)ShiftType.Third);
                var confirmationOfDay = ConfirmedDailyRecord.FirstOrDefault(x => x.RecordDate == targetDate && x.ProcessUnitId == selectedFactories[i].ProcessUnitId);
                var confirmationUntilTheDay = ConfirmedDailyRecord.Where(x => x.ProcessUnitId == selectedFactories[i].ProcessUnitId
                                                                          && (x.EnergyApproved || isEnergyCheckOff || isProcessUnitEnergyPreApproved[selectedFactories[i].ProcessUnitId]))
                                                                          .Select(x => new DailyConfirmationViewModel()
                                                                            {
                                                                                Day = x.RecordDate,
                                                                                IsConfirmed = x.Approved,
                                                                            }).ToList();

                selectedFactories[i].Shift1Confirmed = confirmationFirstShift?.Approved ?? false;
                selectedFactories[i].Shift2Confirmed = confirmationSecondShift?.Approved ?? false;
                selectedFactories[i].Shift3Confirmed = confirmationThirdShift?.Approved ?? false;
                selectedFactories[i].DayMaterialConfirmed = confirmationOfDay?.Approved ?? false;
                selectedFactories[i].DayEnergyConfirmed = (confirmationOfDay?.EnergyApproved ?? false)
                    || (isProcessUnitEnergyPreApproved.ContainsKey(selectedFactories[i].ProcessUnitId) && isProcessUnitEnergyPreApproved[selectedFactories[i].ProcessUnitId]);
                selectedFactories[i].DayConfirmed = (confirmationOfDay != null) && (confirmationOfDay.Approved && (confirmationOfDay.EnergyApproved || isProcessUnitEnergyPreApproved[selectedFactories[i].ProcessUnitId]));
                selectedFactories[i].ConfirmedDaysUntilTheDay = JsonConvert.SerializeObject(confirmationUntilTheDay ?? new List<DailyConfirmationViewModel>());
            }

            return this.Json(selectedFactories.ToDataSourceResult(request, this.ModelState));
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
            return this.View();
        }

        [AuthorizeFactory]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public JsonResult ReadProductionPlanData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            this.ValidateDailyModelState(date);
            if (this.ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (this.ModelState.IsValid)
                {
                    var dbResult = this.data.ProductionPlanDatas.All()
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

                    kendoResult = dbResult.ToDataSourceResult(request, this.ModelState, Mapper.Map<SummaryProductionPlanViewModel>);
                }

                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ProductionPlanEnergyReport()
        {
            return this.View();
        }

        [AuthorizeFactory]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public JsonResult ReadProductionPlanEnergyData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            this.ValidateDailyModelState(date);
            if (this.ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (this.ModelState.IsValid)
                {
                    var dbResult = this.data.ProductionPlanDatas.All()
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

                    kendoResult = dbResult.ToDataSourceResult(request, this.ModelState, Mapper.Map<EnergyProductionPlanDataViewModel>);
                }

                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DailyInfoHydrocarbonsData()
        {
            return this.View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public JsonResult ReadUnitsReportsData([DataSourceRequest]
                                        DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            this.ValidateUnitsReportModelState(date);
            if (this.ModelState.IsValid)
            {
                var result = this.unitsDataService.GetConsolidatedShiftData(date.Value, processUnitId, factoryId);

                var kendoPreparedResult = Mapper.Map<IEnumerable<MultiShift>, IEnumerable<UnitsReportsDataViewModel>>(result);
                var kendoResult = new DataSourceResult();
                try
                {
                    kendoResult = kendoPreparedResult.ToDataSourceResult(request, this.ModelState);
                }
                catch (Exception ex1)
                {
                    Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
                }

                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(new[] { new object() }.ToDataSourceResult(request, this.ModelState), JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public JsonResult ReadDailyInfoHydrocarbonsData([DataSourceRequest]
                                        DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            this.ValidateUnitsReportModelState(date);
            if (this.ModelState.IsValid)
            {
                var result = this.unitsDataService.GetConsolidatedShiftData(date.Value, processUnitId,
                    factoryId, CommonConstants.DailyInfoDailyInfoHydrocarbonsShiftTypeId, false);

                var kendoPreparedResult = Mapper.Map<IEnumerable<MultiShift>, IEnumerable<UnitsReportsDataViewModel>>(result).ToList();

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
                    kendoResult = kendoPreparedResult.ToDataSourceResult(request, this.ModelState);
                }
                catch (Exception ex1)
                {
                    Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
                }

                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }

            return this.Json(new[] { new object() }.ToDataSourceResult(request, this.ModelState), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InnerPipelinesData()
        {
            this.ViewData["products"] = Mapper.Map<IEnumerable<ProductViewModel>>(this.data.Products.All()).ToList();
            return this.View();
        }

        [HttpGet]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public ActionResult ReadInnerPipelinesData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            this.ValidateDailyModelState(date);

            if (this.ModelState.IsValid)
            {
                List<InnerPipelineDto> dbResult = new List<InnerPipelineDto>();
                if (date.Value <= DateTime.Now)
                {
                    dbResult = this.pipes.ReadDataForMonth(date.Value).ToList();
                }

                var vmResult = Mapper.Map<IEnumerable<InnerPipelinesDataViewModel>>(dbResult).Where(x => x.Volume != 0 || x.Mass != 0).OrderBy(x => x.Product.Code);
                    var kendoResult = vmResult.ToDataSourceResult(request, this.ModelState);
                    return this.Json(kendoResult, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var kendoResult = new List<InnerPipelinesDataViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult InProcessUnitsData()
        {
            this.ViewData["products"] = Mapper.Map<IEnumerable<ProductViewModel>>(this.data.Products.All().ToList());
            this.ViewData["processunits"] = Mapper.Map<IEnumerable<Web.ViewModels.Units.ProcessUnitViewModel>>(this.data.ProcessUnits.All().ToList());
            return this.View();
        }

        [HttpGet]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public ActionResult ReadInProcessUnitsData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            this.ValidateDailyModelState(date);

            if (this.ModelState.IsValid)
            {
                var dbResult = this.data.InProcessUnitDatas.All().Where(x => x.RecordTimestamp == date);
                var kendoResult = dbResult.ToDataSourceResult(request, this.ModelState, Mapper.Map<InProcessUnitsViewModel>);
                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kendoResult = new List<InProcessUnitsViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult, JsonRequestBehavior.AllowGet);
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
            if (!this.User.IsInRole("Administrator") && !this.User.IsInRole("AllParksReporter"))
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