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
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;
    using AutoMapper;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels;
    using System.Diagnostics;
    public class SummaryReportsController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;
        private const int HalfAnHour = 60 * 30;

        public SummaryReportsController(IProductionData dataParam, IUnitsDataService unitsDataParam)
            :base(dataParam)
        {
            this.unitsData = unitsDataParam;
        }

        [HttpGet]
        public ActionResult TanksData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "date;parkId;shiftId;areaId")]
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
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "date;processUnitId;factoryId")]
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


    }
}