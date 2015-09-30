using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Infrastructure.Filters;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using CollectingProductionDataSystem.Web.ViewModels.Transactions;

namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    [Authorize]
    public class MeasuringPointsController : AreaBaseController
    {
        public MeasuringPointsController(IProductionData dataParam) : base(dataParam)
        {
        }

        // GET: DailyReporting/MeasuringPoints
        public ActionResult MeasuringPointsData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeFactory]
        public JsonResult ReadMeasuringPointsData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? direction)
        {
            var transactions = new HashSet<MeasuringPointsDataViewModel>();
            transactions.Add(new MeasuringPointsDataViewModel
            {

            });

            return Json(new[] { transactions }.ToDataSourceResult(request, ModelState));
        }
    }
}