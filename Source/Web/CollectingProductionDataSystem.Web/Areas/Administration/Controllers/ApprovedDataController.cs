namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    using CollectingProductionDataSystem.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class ApprovedDataController : AreaBaseController
    {
        public ApprovedDataController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        // GET: Administration/ApprovedData
        public ActionResult Index()
        {
            return View();
        }
    }
}