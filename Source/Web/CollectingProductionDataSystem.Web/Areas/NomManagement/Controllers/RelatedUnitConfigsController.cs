using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class RelatedUnitConfigsController : GenericNomController<UnitConfig, RelatedUnitConfigsViewModel>
    {
        public RelatedUnitConfigsController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        [ValidateAntiForgeryToken]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int id)
        {
            var result = this.data.UnitConfigs.All().Where(x => x.Id != id);
            return Json(result.ToDataSourceResult(request, ModelState, Mapper.Map<RelatedUnitConfigsViewModel>));
        }
    }
}