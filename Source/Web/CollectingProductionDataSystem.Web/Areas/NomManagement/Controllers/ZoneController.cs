namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

    public class ZoneController : GenericNomController<Zone, ZoneViewModel>
    {
        public ZoneController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public override ActionResult Index()
        {
            ViewData["ikunks"] = data.Ikunks.All().ToList();
            return base.Index();
        }
    }
}