namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Contracts;
    public class NomenclaturesController : AreaBaseController
    {
        public NomenclaturesController(IProductionData dataParam)
            : base(dataParam)
        {
        }
        // GET: NomManagement/Nomenclatures
        public ActionResult Index()
        {
            return View();
        }
    }
}