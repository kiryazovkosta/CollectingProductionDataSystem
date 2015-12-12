using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers
{
    public class DashBoardController : Controller
    {
        // GET: SummaryReporting/DashBoard
        public ActionResult Index()
        {
            return View();
        }
    }
}