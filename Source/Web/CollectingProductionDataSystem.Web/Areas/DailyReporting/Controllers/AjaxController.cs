using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Application.UnitsDataServices;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.ViewModels.Units;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    public class AjaxController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;

        public AjaxController(IProductionData dataParam, IUnitsDataService unitsDataParam)
            : base(dataParam)
        {
            this.unitsData = unitsDataParam;
        }

        public ActionResult ReadDetails([DataSourceRequest]
                                        DataSourceRequest request, int recordId)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<UnitsData> dbResult = this.unitsData.GetUnitsDataForDailyRecord(recordId);
                var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsData>, IEnumerable<UnitDataViewModel>>(dbResult);
                var kendoResult = new DataSourceResult();
                kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
            else
            {
                return Json(new[] { recordId }.ToDataSourceResult(request, ModelState));
            }
        }
    }
}