namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels;

    public class TanksStatusesController : AreaBaseController
    {

        public TanksStatusesController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        [HttpGet]
        public ActionResult TanksStatusesData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadTanksStatusesData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? areaId, int? parkId)
        {
                var dbResult = this.data.Tanks.All();
                var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
                kendoResult.Data = Mapper.Map<IEnumerable<TankConfig>, IEnumerable<TanksStatusesViewModel>>((IEnumerable<TankConfig>)kendoResult.Data);
                return Json(kendoResult);
        }

        private void ValidateInputModel(DateTime? date, int? parkId)
        {
            throw new NotImplementedException();
        }
    }
}