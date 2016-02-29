namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data.Entity;
    using System.Web.UI;
    using CollectingProductionDataSystem.Constants;
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

    public class DailyInfoHydrocarbonsController : AreaBaseController
    {
        private const int HalfAnHour = 60 * 30;
        private readonly IUnitsDataService unitsDataService;

        public DailyInfoHydrocarbonsController(IProductionData dataParam, IUnitsDataService unitsDataParam)
            : base(dataParam)
        {
            this.unitsDataService = unitsDataParam;
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

                var result = dbResult.Select(x => new MultiShift
                {
                    TimeStamp = x.RecordTimestamp,
                    Factory = string.Format("{0:d2} {1}", x.UnitConfig.ProcessUnit.Factory.Id, x.UnitConfig.ProcessUnit.Factory.ShortName),
                    ProcessUnit = string.Format("{0:d2} {1}", x.UnitConfig.ProcessUnit.Id, x.UnitConfig.ProcessUnit.ShortName),
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

                var output = Json(kendoResult, JsonRequestBehavior.AllowGet);
                output.MaxJsonLength = int.MaxValue;
                return output;
            }
            else
            {
                return Json(new[] { new object() }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
        }

        private void ValidateUnitsReportModelState(DateTime? date)
        {

            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }

        }
    }
}