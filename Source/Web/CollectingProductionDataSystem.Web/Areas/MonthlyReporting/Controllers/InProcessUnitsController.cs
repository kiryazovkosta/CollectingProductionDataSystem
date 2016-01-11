namespace CollectingProductionDataSystem.Web.Areas.MonthlyReporting.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Contracts;
    using Kendo.Mvc.UI;

    public class InProcessUnitsController : AreaBaseController
    {
        public InProcessUnitsController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        [HttpGet]
        public ActionResult InProcessUnitsData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadInProcessUnitsData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            return Json(date);
        }
    }
}