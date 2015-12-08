namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data.Entity;
    using System.Web.UI;
    using CollectingProductionDataSystem.Data.Contracts;
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

    public class SummaryReportsController : AreaBaseController
    {
        private const int HalfAnHour = 60 * 30;
        private readonly IUnitsDataService unitsData;
        private readonly IUnitDailyDataService dailyService;

        public SummaryReportsController(IProductionData dataParam, IUnitsDataService unitsDataParam, IUnitDailyDataService dailyServiceParam)
            : base(dataParam)
        {
            this.unitsData = unitsDataParam;
            this.dailyService = dailyServiceParam;
        }

        [HttpGet]
        public ActionResult TanksData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public ActionResult ReadTanksData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, int? shiftId, int? areaId)
        {
            ValidateTanksInputModel(date, parkId, shiftId);

            if (this.ModelState.IsValid)
            {
                date = date.Value.AddTicks(data.Shifts.GetById(shiftId).EndTicks);

                var dbResult = this.data.TanksData.All()
                    .Include(t => t.TankConfig)
                    .Include(t => t.TankConfig.Park)
                    .Where(t => t.RecordTimestamp == date
                        && t.TankConfig.Park.AreaId == (areaId ?? t.TankConfig.Park.AreaId)
                        && t.ParkId == (parkId ?? t.ParkId));


                var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
                kendoResult.Data = Mapper.Map<IEnumerable<TankData>, IEnumerable<TankDataViewModel>>((IEnumerable<TankData>)kendoResult.Data);
                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<TankDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        private void ValidateTanksInputModel(DateTime? date, int? parkId, int? shiftId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksDateSelector));
            }

            if (shiftId == null)
            {
                this.ModelState.AddModelError("shifts", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksShiftMinutesOffsetSelector));
            }
        }

        [HttpGet]
        public ActionResult UnitsReportsData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public JsonResult ReadUnitsReportsData([DataSourceRequest]
                                        DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            ValidateUnitsReportModelState(date);
            if (ModelState.IsValid)
            {
                var dbResult = this.unitsData.GetUnitsDataForDateTime(date, processUnitId, null)
                    .Where(x => x.UnitConfig.IsMemberOfShiftsReport
                    && x.UnitConfig.ProcessUnit.FactoryId == (factoryId ?? x.UnitConfig.ProcessUnit.FactoryId))
                    .OrderBy(x => x.UnitConfig.Code)
                    .ToList();

                var result = dbResult.Select(x => new MultiShift
                {
                    TimeStamp = x.RecordTimestamp,
                    Factory = x.UnitConfig.ProcessUnit.Factory.ShortName,
                    ProcessUnit = x.UnitConfig.ProcessUnit.ShortName,
                    Code = x.UnitConfig.Code,
                    Position = x.UnitConfig.Position,
                    MeasureUnit = x.UnitConfig.MeasureUnit.Code,
                    UnitConfigId = x.UnitConfigId,
                    UnitName = x.UnitConfig.Name,
                    Shift1 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == ShiftType.First).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                    Shift2 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == ShiftType.Second).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                    Shift3 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == ShiftType.Third).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                }).Distinct(new MultiShiftComparer()).ToList();

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

                return Json(kendoResult);
            }
            else
            {
                return Json(new[] { new object() }.ToDataSourceResult(request, ModelState));
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeFactory]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public JsonResult ReadDailyUnitsData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? factoryId)
        {
            ValidateDailyModelState(date);
            if (ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (ModelState.IsValid)
                {
                    var dbResult = unitsData.GetUnitsDailyDataForDateTime(date, processUnitId);
                    dbResult.Where(x => x.UnitsDailyConfig.ProcessUnit.FactoryId == (factoryId ?? x.UnitsDailyConfig.ProcessUnit.FactoryId));
                    var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsDailyData>, IEnumerable<UnitDailyDataViewModel>>(dbResult);
                    kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);

                }

                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
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

            var SelectedFactories = data.ProcessUnits.All().Include(x => x.Factory)
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


            for (int i = 0; i < SelectedFactories.Count; i++)
            {
                var confirmationFirstShift = ConfirmedRecords.FirstOrDefault(x => x.ProcessUnitId == SelectedFactories[i].ProcessUnitId && x.ShiftId == (int)ShiftType.First);
                var confirmationSecondShift = ConfirmedRecords.FirstOrDefault(x => x.ProcessUnitId == SelectedFactories[i].ProcessUnitId && x.ShiftId == (int)ShiftType.Second);
                var confirmationThirdShift = ConfirmedRecords.FirstOrDefault(x => x.ProcessUnitId == SelectedFactories[i].ProcessUnitId && x.ShiftId == (int)ShiftType.Third);
                var confirmationOfDay = ConfirmedDailyRecord.FirstOrDefault(x => x.RecordDate == targetDate && x.ProcessUnitId == SelectedFactories[i].ProcessUnitId);
                var confirmationUntilTheDay = ConfirmedDailyRecord.Where(x => x.ProcessUnitId == SelectedFactories[i].ProcessUnitId).Select(x => new DailyConfirmationViewModel()
                {
                    Day = x.RecordDate,
                    IsConfirmed = x.Approved,
                }).ToList();

                SelectedFactories[i].Shift1Confirmed = (confirmationFirstShift == null) ? false : confirmationFirstShift.Approved;
                SelectedFactories[i].Shift2Confirmed = (confirmationSecondShift == null) ? false : confirmationSecondShift.Approved;
                SelectedFactories[i].Shift3Confirmed = (confirmationThirdShift == null) ? false : confirmationThirdShift.Approved;
                SelectedFactories[i].DayConfirmed = (confirmationOfDay == null) ? false : confirmationOfDay.Approved;
                SelectedFactories[i].ConfirmedDaysUntilTheDay = JsonConvert.SerializeObject(confirmationUntilTheDay ?? new List<DailyConfirmationViewModel>());
            }

            return Json(SelectedFactories.ToDataSourceResult(request, ModelState));

        }

        private void ValidateDailyModelState(DateTime? date)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
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