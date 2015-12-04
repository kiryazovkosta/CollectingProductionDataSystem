using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Controllers;

namespace CollectingProductionDataSystem.Web.Areas.Documentation.Controllers
{
    public class HelpController : Controller
    {
        // GET: Documentation/Documentation
        public ActionResult Index(string nextAction)
        {
            ViewBag.NextAction = nextAction;
            return View();
        }
        public PartialViewResult ShiftData()
        {
            return PartialView();
        }
        public PartialViewResult ShiftDataMeasuringPosition()
        {
            return PartialView();
        }
        public PartialViewResult ShiftDataTank()
        {
            return PartialView();
        }
        public PartialViewResult DailyData()
        {
            return PartialView();
        }
        public PartialViewResult DailyDataMeasuringPosition()
        {
            return PartialView();
        }
        public PartialViewResult DailyDataExpedition()
        {
            return PartialView();
        }
        public PartialViewResult ShiftDataMeasuringPositionReport()
        {
            return PartialView();
        }
    }
}