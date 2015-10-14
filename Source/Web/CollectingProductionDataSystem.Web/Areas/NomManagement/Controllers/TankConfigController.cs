using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class TankConfigController : GenericNomController<TankConfig, TankConfigViewModel>
    {
        public TankConfigController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public override ActionResult Index()
        {
            ViewData["parks"] = data.Parks.All().ToList();
            return base.Index();
        }
    }
}