using System.Data.Entity;
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
                .Include(x => x.MeasuringPointConfig)
                .Where(x => x.MeasuringPointConfig.IsInternalPoint == false)
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
                MeasuringPointId = t.MeasuringPointConfigId,
                TransportId = t.MeasuringPointConfig.TransportTypeId,
                ProductId = t.ProductNumber.Value,
                Mass = t.Mass,
                MassReverse = t.MassReverse
            });

            var dict = new SortedDictionary<int, MeasuringPointsDataViewModel>();

            foreach (var transactionData in transactionsData)
            {
                var mpd = new MeasuringPointsDataViewModel();

                if (dict.ContainsKey(transactionData.ProductId))
                {
                    mpd = dict[transactionData.ProductId];
                }
                
                mpd.ProductId = transactionData.ProductId;

                if (transactionData.TransportId == 1)
                {
                    mpd.AvtoQuantity += transactionData.RealMass;
                }
                else if (transactionData.TransportId == 2)
                {
                    mpd.JpQuantity += transactionData.RealMass;
                }
                else if (transactionData.TransportId == 3)
                {
                    mpd.SeaQuantity += transactionData.RealMass;
                }
                else if (transactionData.TransportId == 4)
                {
                    mpd.PipeQuantity += transactionData.RealMass;
                }
                mpd.TotalQuantity += transactionData.RealMass;
                dict[transactionData.ProductId] = mpd;
            }

            var kendoResult = new DataSourceResult();
            try
            {
                var hs = new HashSet<MeasuringPointsDataViewModel>();
                foreach (var item in dict)
                {
                    var p = item.Value;
                    p.ProductName = this.data.Products.All().Where(x => x.Code == p.ProductId).FirstOrDefault().Name;
                    if (p.AvtoQuantity > 0)
                    {
                        p.AvtoQuantity = p.AvtoQuantity / 1000;
                    }
                    if (p.JpQuantity > 0)
                    {
                        p.JpQuantity = p.JpQuantity / 1000;
                    }
                    if (p.SeaQuantity > 0)
                    {
                        p.SeaQuantity = p.SeaQuantity / 1000;
                    }
                    if (p.PipeQuantity > 0)
                    {
                        p.PipeQuantity = p.PipeQuantity / 1000;
                    }
                    if (p.TotalQuantity > 0)
                    {
                        p.TotalQuantity = p.TotalQuantity / 1000;
                    }
                    hs.Add(p);
                }

                kendoResult = hs.ToDataSourceResult(request, ModelState);
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
        public int TransportId { get; set; }
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