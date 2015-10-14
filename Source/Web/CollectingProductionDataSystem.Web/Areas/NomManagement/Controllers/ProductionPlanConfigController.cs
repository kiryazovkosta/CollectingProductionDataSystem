using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class ProductionPlanConfigController : GenericNomController<ProductionPlanConfig, ProductionPlanConfigViewModel>
    {
        public ProductionPlanConfigController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        public override ActionResult Index()
        {
            ViewData["processUnits"] = data.ProcessUnits.All().ToList();
            return base.Index();
        }
    }
}