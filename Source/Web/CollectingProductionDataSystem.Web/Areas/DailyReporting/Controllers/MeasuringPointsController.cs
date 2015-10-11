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
        public JsonResult ReadMeasuringPointsData([DataSourceRequest]DataSourceRequest request, DateTime? dateParam, int? directionParam)
        {
            var beginTimestamp = dateParam.Value.AddMinutes(300);
            var endTimestamp = beginTimestamp.AddMinutes(1440);

            var transactionsData = this.data.MeasuringPointsConfigsDatas
                .All()
                .Where(x => x.TransactionEndTime >= beginTimestamp)
                .Where(x => x.TransactionEndTime < endTimestamp)
                .Where(x => x.RowId == -1)
                .Select(x => new
                {
                    Id = x.MeasuringPointId,
                    Direction = x.FlowDirection,
                    ProductId = x.ProductNumber
                });

            var transactions = new HashSet<MeasuringPointsDataViewModel>();
            transactions.Add(new MeasuringPointsDataViewModel
            {

            });

            return Json(new[] { transactions }.ToDataSourceResult(request, ModelState));
        }
    }
}