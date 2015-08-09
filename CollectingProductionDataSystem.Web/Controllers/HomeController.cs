namespace CollectingProductionDataSystem.Web.Controllers
{
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using CollectingProductionDataSystem.Web.ViewModels.Home;
    using CollectingProductionDataSystem.Common;
    using CollectingProductionDataSystem.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}