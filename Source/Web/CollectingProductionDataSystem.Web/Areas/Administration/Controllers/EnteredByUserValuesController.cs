using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    public class EnteredByUserValuesController : AreaBaseController
    {
        public EnteredByUserValuesController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        // GET: Administration/EnteredByUserValues
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = data.UnitEnteredForCalculationDatas.All().OrderByDescending(x => x.CreatedOn).ToList();
            return Json(result.ToDataSourceResult(request, ModelState, Mapper.Map<EnteredByUserValueViewModel>));
        }
    }
}