using System;
using System.Linq;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;

namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    public class UserStatisticsController : AreaBaseController
    {
        public UserStatisticsController(IProductionData dataParam) 
            :base(dataParam)
        { 
        }

        // GET: Administration/UserStatistics
        public ActionResult Index()
        {
            return View();
        }
    }
}