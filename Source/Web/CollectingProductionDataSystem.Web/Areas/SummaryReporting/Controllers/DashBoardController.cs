using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;

namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers
{
    public class DashBoardController : AreaBaseController
    {
        public DashBoardController(IProductionData dataParam)
        :base(dataParam)
        {
            
        }
        // GET: SummaryReporting/DashBoard
        public ActionResult Index()
        {
            return View();
        }
    }
}