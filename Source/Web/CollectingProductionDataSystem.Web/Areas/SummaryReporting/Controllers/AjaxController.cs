namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Areas.ShiftReporting.ViewModels;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    public class AjaxController : AreaBaseController
    {
        private const int HalfAnHour = 60 * 30;
        private readonly IUnitsDataService unitsData;

        public AjaxController(IProductionData dataParam, IUnitsDataService unitsDataParam)
            : base(dataParam)
        {
            this.unitsData = unitsDataParam;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public ActionResult ReadDetails([DataSourceRequest]
                                        DataSourceRequest request, int recordId)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<UnitsData> dbResult = this.unitsData.GetUnitsDataForDailyRecord(recordId);
                var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsData>, IEnumerable<UnitDataViewModel>>(dbResult);
                var kendoResult = new DataSourceResult();
                kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                return Json(kendoResult,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new[] { recordId }.ToDataSourceResult(request, ModelState),JsonRequestBehavior.AllowGet);
            }
        }
    }
}