namespace CollectingProductionDataSystem.Web.Controllers
{
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using CollectingProductionDataSystem.Web.ViewModels.Home;
    using CollectingProductionDataSystem.Common.Repositories;
    using CollectingProductionDataSystem.Models;

    public class HomeController : Controller
    {
        private IRepository<InventoryTank> tanks;

        public HomeController(IRepository<InventoryTank> tanksParam)
        {
            this.tanks = tanksParam;
        }

        public ActionResult Index()
        {
            var tanks = this.tanks.All().Project().To<IndexInventoryTanksViewModel>();
            return View(tanks);
        }
    }
}