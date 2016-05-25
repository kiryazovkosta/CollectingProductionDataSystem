using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;

namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Controllers
{
    public class PlanNormController : AreaBaseController
    {
        public PlanNormController(IProductionData dataParam)
            :base(dataParam)
        {
               
        }

        // GET: PlanConfiguration/PlanNorm
        public ActionResult Index()
        {
            return View();
        }
    }
}