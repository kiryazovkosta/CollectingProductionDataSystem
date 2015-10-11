using System.Diagnostics;
using CollectingProductionDataSystem.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Web.ViewModels.Transactions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using AutoMapper;
using CollectingProductionDataSystem.Models.Transactions;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    public class TransactionsDailyController : AreaBaseController
    {
        public TransactionsDailyController(IProductionData dataParam) : base(dataParam)
        {

        }

        [HttpGet]
        public ActionResult TransactionsDailyData()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ReadTransactionsDailyData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? flowDirection)
        {
            ValidateModelState(date, flowDirection);
            var transactions = this.data.MeasuringPointsConfigsDatas
                .All()
                .Where(x => x.RowId == -1);

            if (date.HasValue)
            {
                var beginTimestamp = date.Value.AddMinutes(300);
                var endTimestamp = beginTimestamp.AddMinutes(1440);

                transactions = transactions.Where(x => x.TransactionEndTime >= beginTimestamp & x.TransactionEndTime < endTimestamp); 
            }

            if (flowDirection.HasValue)
            {
                transactions = transactions.Where(x => x.FlowDirection == flowDirection);
            }

            var transactionsData = transactions.Select(t => new TransactionDataModel
            {
                MeasuringPointId = t.MeasuringPointId,
                ProductId = t.ProductNumber.Value,
                Mass = t.Mass,
                MassReverse = t.MassReverse
            });

            foreach (var transactionData in transactionsData)
            {
                
            }


            var kendoPreparedResult = Mapper.Map<IEnumerable<MeasuringPointsConfigsData>, IEnumerable<MeasuringPointsDataViewModel>>(transactions);
            var kendoResult = new DataSourceResult();
            try
            {
                kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
            }

            return Json(kendoResult);
        }
 
        private void ValidateModelState(DateTime? date, int? directionId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
            if (directionId == null)
            {
                this.ModelState.AddModelError("direction", string.Format(Resources.ErrorMessages.Required, Resources.Layout.Direction));
            }
        }
    }

    public class TransactionDataModel 
    { 
        public int MeasuringPointId { get; set; }
        public int ProductId { get; set; }
        public decimal? Mass { get; set; }
        public decimal? MassReverse { get; set; }
        public decimal RealMass
        {
            get 
            {
                if (this.Mass.HasValue)
                {
                    return this.Mass.Value;
                }
                else if (this.MassReverse.HasValue)
                {
                    return this.MassReverse.Value;   
                }
                else
                {
                    return 0.0m;
                }
            }
        }
    }
}