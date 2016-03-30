using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Controllers;

namespace CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Controllers
{
    public class DashboardController : AreaBaseController
    {
        public DashboardController(IProductionData dataParam)
            :base(dataParam)
        {
        }

        // GET: ManagemetDashBoard/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}