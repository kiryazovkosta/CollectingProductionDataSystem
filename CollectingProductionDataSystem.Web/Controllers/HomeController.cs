namespace CollectingProductionDataSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using CollectingProductionDataSystem.Common.Repositories;
    using CollectingProductionDataSystem.Data.Repositories;
    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Data;

    public class HomeController : Controller
    {
        private IRepository<InventoryTank> tanks;

        public HomeController(IRepository<InventoryTank> tanksParam)
        {
            this.tanks = tanksParam;
        }

        public ActionResult Index()
        {
            var tanks = this.tanks.All();
            return View(tanks);
        }
    }
}