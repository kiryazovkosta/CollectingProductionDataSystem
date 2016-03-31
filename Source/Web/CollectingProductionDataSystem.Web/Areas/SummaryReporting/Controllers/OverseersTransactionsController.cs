namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Web.Areas.OverseersReporting.Controllers;
    using CollectingProductionDataSystem.Web.Controllers;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using CollectingProductionDataSystem.Models.Transactions;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Web.ViewModels.Transactions;
    using System.Diagnostics;

    [Authorize(Roles = "Administrator, OverseerReporter")]
    public class OverseersTransactionsController : BaseController
    {
        public OverseersTransactionsController(IProductionData productionDataParam) : base(productionDataParam)
        {
        }

        [HttpGet]
        public ActionResult OverseersTransactionsData()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ReadOverseersTransactionsData([DataSourceRequest]
                                                        DataSourceRequest request, DateTime? date, int? flowDirection)
        {
            ValidateModelState(date, flowDirection);
            var kendoResult = new DataSourceResult();
            try
            {
                var transactionsData = GetMeasurementPointsDataByDateAndDirection(date, flowDirection)
                                                    .Select(t => new TransactionDataModel
                                                            {
                                                                MeasuringPointId = t.MeasuringPointId,
                                                                DirectionId = t.MeasuringPointConfig.FlowDirection.Value,
                                                                TransportId = t.MeasuringPointConfig.TransportTypeId,
                                                                ProductId = t.ProductNumber.Value,
                                                                Mass = t.Mass,
                                                                MassReverse = t.MassReverse,
                                                                FlowDirection = t.MeasuringPointConfig.FlowDirection.Value,
                                                            });
    
                var dict = new SortedDictionary<int, MeasuringPointsDataViewModel>();
                
                foreach (var transactionData in transactionsData)
                {
                    var measuringPointData = FillModelObject(transactionData, dict, flowDirection);
                    if (measuringPointData != null)
                    {
                        dict[transactionData.ProductId] = measuringPointData;   
                    }
                }
                
                var beginTimestamp = date.Value.AddMinutes(270);
                var endTimestamp = beginTimestamp.AddMinutes(1440);
                var totalizers = this.data.MeasuringPointProductsConfigs.All().Where(x => x.IsUsedInProductionReport == true).ToList();
                var totalizersData = this.data.MeasurementPointsProductsDatas.All()
                                         .Where(x => x.RecordTimestamp > beginTimestamp &&
                                                     x.RecordTimestamp < endTimestamp &&
                                                     x.DirectionId == flowDirection.Value)
                                         .ToList();

                foreach (var item in totalizersData)
                {
                    var measuringPoint = totalizers.Select(x => x.MeasuringPointConfigId == item.MeasuringPointConfigId && x.ProductId == item.ProductId).FirstOrDefault();
                    if (measuringPoint != null)
                    {
                        var measuringPointData = new MeasuringPointsDataViewModel();
                        var code = this.data.Products.All().Where(p => p.Id == item.ProductId).FirstOrDefault().Code;

                        if (dict.ContainsKey(code))
                        {
                            measuringPointData = dict[code];
                        }

                        measuringPointData.ProductId = code;
                        var transportId = item.MeasuringPointConfig.TransportTypeId;
                        if (transportId == 1)
                        {
                            measuringPointData.AvtoQuantity += item.Value.Value;
                        }
                        else if (transportId == 2)
                        {
                            measuringPointData.JpQuantity += item.Value.Value;
                        }
                        else if (transportId == 3)
                        {
                            measuringPointData.SeaQuantity += item.Value.Value;
                        }
                        else if (transportId == 4)
                        {
                            measuringPointData.PipeQuantity += item.Value.Value;
                        }
                        measuringPointData.TotalQuantity += item.Value.Value;
                        dict[code] = measuringPointData;
                    }
                }

                var hs = PopulateAllMeaurementPointDatas(dict);
                kendoResult = hs.ToDataSourceResult(request, ModelState);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
            }

            return Json(kendoResult);
        }

        private HashSet<MeasuringPointsDataViewModel> PopulateAllMeaurementPointDatas(SortedDictionary<int, MeasuringPointsDataViewModel> dict)
        {
            var hs = new HashSet<MeasuringPointsDataViewModel>();
            foreach (var item in dict)
            {
                var p = SetMeasuringPointsDataViewModel(item);
                if (p.TotalQuantity > 0)
                {
                    hs.Add(p);   
                }
            }

            return hs;
        }
 
        private IQueryable<MeasuringPointsConfigsData> GetMeasurementPointsDataByDateAndDirection(DateTime? date, int? flowDirection, bool checkMonthData = false)
        {
            var transactions = this.data.MeasuringPointsConfigsDatas
                                   .All()
                                   .Include(x => x.MeasuringPointConfig)
                                   .Where(x => x.MeasuringPointConfig.IsUsedInProductionReport == true)
                                   .Where(x => x.MeasuringPointConfig.IsInternalPoint == false)
                                   .Where(x => x.RowId == -1);

            if (date.HasValue)
            {
                var beginTimestamp = new DateTime();
                var endTimestamp = new DateTime();
                if (!checkMonthData)
                {
                    beginTimestamp = date.Value.AddMinutes(270);
                    endTimestamp = beginTimestamp.AddMinutes(1440);  
                }
                else
                {
                    beginTimestamp = new DateTime(date.Value.Year, date.Value.Month, 1, 4, 30, 0);
                    endTimestamp = date.Value.AddMinutes(1740);
                }

                transactions = transactions.Where(x => x.TransactionEndTime >= beginTimestamp & x.TransactionEndTime < endTimestamp); 
            }

            if (flowDirection.HasValue)
            {
                transactions = transactions.Where(x => x.MeasuringPointConfig.FlowDirection.Value == flowDirection || x.MeasuringPointConfig.FlowDirection.Value == 3);
            }
            return transactions;
        }
 
        private MeasuringPointsDataViewModel SetMeasuringPointsDataViewModel(KeyValuePair<int, MeasuringPointsDataViewModel> item)
        {
            var p = item.Value;
            var product = this.data.Products.All().Where(x => x.Code == p.ProductId).FirstOrDefault();
            if (product != null)
            {
                p.ProductName = product.Name;    
            }
           
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
            return p;
        }
 
        private MeasuringPointsDataViewModel FillModelObject(TransactionDataModel transactionData,
            SortedDictionary<int, MeasuringPointsDataViewModel> dict,
            int? flowDirection)
        {
            var measuringPointData = new MeasuringPointsDataViewModel();
            if (transactionData.DirectionId == 3)
            {
                // data from Pt Rosenec
                if (flowDirection == 2 && (transactionData.MassReverse.HasValue == false || transactionData.MassReverse.Value == 0))
                {
                    return null;
                }
                if (flowDirection == 1 && (transactionData.Mass.HasValue == false || transactionData.Mass.Value == 0))
                {
                    return null;
                }
            }

            if (dict.ContainsKey(transactionData.ProductId))
            {
                measuringPointData = dict[transactionData.ProductId];
            }
                
            measuringPointData.ProductId = transactionData.ProductId;

            if (transactionData.TransportId == 1)
            {
                measuringPointData.AvtoQuantity += transactionData.RealMass;
            }
            else if (transactionData.TransportId == 2)
            {
                measuringPointData.JpQuantity += transactionData.RealMass;
            }
            else if (transactionData.TransportId == 3)
            {
                measuringPointData.SeaQuantity += transactionData.RealMass;
            }
            else if (transactionData.TransportId == 4)
            {
                measuringPointData.PipeQuantity += transactionData.RealMass;
            }
            measuringPointData.TotalQuantity += transactionData.RealMass;

            return measuringPointData;
        }
 
        private void ValidateModelState(DateTime? date, int? directionId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
            if (directionId == null || directionId.Value == 3)
            {
                this.ModelState.AddModelError("direction", string.Format(Resources.ErrorMessages.Required, Resources.Layout.Direction));
            }
        }
    }
}