using System.Data.Entity;
using System.Diagnostics;
using CollectingProductionDataSystem.Constants;
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
using CollectingProductionDataSystem.Application.TransactionsDailyDataServices;

namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    public class TransactionsDailyController : AreaBaseController
    {
        private readonly ITransactionsDailyDataService transactionsDailyData;

        public TransactionsDailyController(IProductionData dataParam, ITransactionsDailyDataService transactionDailyDataParam) 
            : base(dataParam)
        {
            this.transactionsDailyData = transactionDailyDataParam;
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
            var kendoResult = new DataSourceResult();
            try
            {
                HashSet<MeasuringPointsConfigsReportData> dbResult = this.transactionsDailyData.ReadTransactionsDailyData(date.Value, flowDirection.Value);
                kendoResult = dbResult.ToDataSourceResult(request, ModelState);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
            }

            return Json(kendoResult);
        }
 
        //private HashSet<MeasuringPointsDataViewModel> PopulateAllMeaurementPointDatas(SortedDictionary<int, MeasuringPointsDataViewModel> dict, 
        //    Dictionary<int, decimal> actDict, Dictionary<int, decimal> totallyQuantityByProducts)
        //{
        //    var hs = new HashSet<MeasuringPointsDataViewModel>();
        //    foreach (var item in dict)
        //    {
        //        var p = SetMeasuringPointsDataViewModel(item, actDict, totallyQuantityByProducts);
        //        if (p.TotalQuantity > 0)
        //        {
        //            hs.Add(p);   
        //        }
        //    }

        //    if (totallyQuantityByProducts != null && totallyQuantityByProducts.Count > 0)
        //    {
        //        foreach (var item in totallyQuantityByProducts)
        //        {
        //            var m = new MeasuringPointsDataViewModel();
        //            m.ProductId = item.Key;
        //            m.ProductName = this.data.Products.All().Where(x => x.Code == item.Key).FirstOrDefault().Name;
        //            m.TotalMonthQuantity = item.Value / 1000;
        //            if (actDict != null && actDict.ContainsKey(item.Key))
        //            {
        //                m.ActiveQuantity = actDict[item.Key] / 1000;
        //                actDict.Remove(item.Key);
        //            }
        //            hs.Add(m);
        //        }
        //    }

        //    if (actDict != null && actDict.Count > 0)
        //    {
        //        foreach (var item in actDict)
        //        {
        //            var m = new MeasuringPointsDataViewModel();
        //            m.ProductId = item.Key;
        //            m.ProductName = this.data.Products.All().Where(x => x.Code == item.Key).FirstOrDefault().Name;
        //            m.ActiveQuantity = item.Value / 1000;
        //            hs.Add(m);
        //        }
        //    }

        //    return hs;
        //}
 
        //private IQueryable<MeasuringPointsConfigsData> GetMeasurementPointsDataByDateAndDirection(DateTime? date, int? flowDirection, bool checkMonthData = false)
        //{
        //    var transactions = this.data.MeasuringPointsConfigsDatas
        //                           .All()
        //                           .Include(x => x.MeasuringPointConfig)
        //                           .Where(x => x.MeasuringPointConfig.IsInternalPoint == false)
        //                           .Where(x => x.RowId == -1);

        //    if (date.HasValue)
        //    {
        //        var beginTimestamp = new DateTime();
        //        var endTimestamp = new DateTime();
        //        if (!checkMonthData)
        //        {
        //            beginTimestamp = date.Value.AddMinutes(300);
        //            endTimestamp = beginTimestamp.AddMinutes(1440);  
        //        }
        //        else
        //        {
        //            beginTimestamp = new DateTime(date.Value.Year, date.Value.Month, 1, 5, 0, 0);
        //            endTimestamp = date.Value.AddMinutes(1740);
        //        }

        //        transactions = transactions.Where(x => x.TransactionEndTime >= beginTimestamp & x.TransactionEndTime < endTimestamp); 
        //    }

        //    if (flowDirection.HasValue)
        //    {
        //        transactions = transactions.Where(x => x.MeasuringPointConfig.FlowDirection.Value == flowDirection || x.MeasuringPointConfig.FlowDirection.Value == 3);
        //    }
        //    return transactions;
        //}
 
        //private Dictionary<int, decimal> GetActiveTransactionsData(DateTime? date, int? flowDirection)
        //{
        //    var actDict = new Dictionary<int, decimal>();
        //    var activeTransactionsDatas = this.data.ActiveTransactionsDatas
        //                                      .All()
        //                                      .Include(x => x.Product)
        //                                      .Include(x => x.MeasuringPointConfig)
        //                                      .Where(x => x.MeasuringPointConfig.IsInternalPoint == false)
        //                                      .Where(x => !string.IsNullOrEmpty(x.MeasuringPointConfig.ActiveTransactionStatusTag))
        //                                      .Where(x => x.RecordTimestamp == date.Value)
        //                                      .Where(x => x.MeasuringPointConfig.FlowDirection.Value == flowDirection || x.MeasuringPointConfig.FlowDirection.Value == 3)
        //                                      .ToList();

        //    foreach (var activeTransaction in activeTransactionsDatas)
        //    {
        //        var direction = activeTransaction.MeasuringPointConfig.FlowDirection.Value;

        //        if (direction == 3)
        //        {
        //            // data from Pt Rosenec
        //            if (flowDirection == 2 && activeTransaction.MassReverse == 0)
        //            {
        //                continue;
        //            }
        //            if (flowDirection == 1 && activeTransaction.Mass == 0)
        //            {
        //                continue;
        //            }
        //        }

        //        var prodId = data.Products.All().Where(p => p.Id == activeTransaction.ProductId).FirstOrDefault().Code;
        //        if (actDict.ContainsKey(prodId))
        //        {
        //            if (direction == 1)
        //            {
        //                actDict[prodId] = actDict[prodId] + activeTransaction.Mass; 
        //            }
        //            else if (direction == 2)
        //            {
        //                actDict[prodId] = actDict[prodId] + activeTransaction.MassReverse;  
        //            }
        //            else 
        //            {
        //                if (flowDirection == 1)
        //                {
        //                    actDict[prodId] = actDict[prodId] + activeTransaction.Mass;    
        //                }
        //                else
        //                {
        //                    actDict[prodId] = actDict[prodId] + activeTransaction.MassReverse;  
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (direction == 1)
        //            {
        //                actDict[prodId] = activeTransaction.Mass; 
        //            }
        //            else if (direction == 2)
        //            {
        //                actDict[prodId] = activeTransaction.MassReverse;  
        //            }
        //            else 
        //            {
        //                if (flowDirection == 1)
        //                {
        //                    actDict[prodId] = activeTransaction.Mass;    
        //                }
        //                else
        //                {
        //                    actDict[prodId] = activeTransaction.MassReverse;  
        //                }
        //            }
        //        }
        //    }

        //    return actDict;
        //}
 
        //private MeasuringPointsDataViewModel SetMeasuringPointsDataViewModel(KeyValuePair<int, MeasuringPointsDataViewModel> item, 
        //    Dictionary<int, decimal> actDict, Dictionary<int, decimal> totallyQuantityByProducts)
        //{
        //    var p = item.Value;
        //    p.ProductName = this.data.Products.All().Where(x => x.Code == p.ProductId).FirstOrDefault().Name;
        //    if (actDict != null && actDict.ContainsKey(p.ProductId))
        //    {
        //        p.ActiveQuantity = actDict[p.ProductId] / 1000;
        //        actDict.Remove(p.ProductId);
        //    }

        //    if (totallyQuantityByProducts != null && totallyQuantityByProducts.ContainsKey(p.ProductId))
        //    {
        //        p.TotalMonthQuantity = totallyQuantityByProducts[p.ProductId] / 1000;
        //        totallyQuantityByProducts.Remove(p.ProductId);
        //    }

        //    if (p.AvtoQuantity > 0)
        //    {
        //        p.AvtoQuantity = p.AvtoQuantity / 1000;
        //    }
        //    if (p.JpQuantity > 0)
        //    {
        //        p.JpQuantity = p.JpQuantity / 1000;
        //    }
        //    if (p.SeaQuantity > 0)
        //    {
        //        p.SeaQuantity = p.SeaQuantity / 1000;
        //    }
        //    if (p.PipeQuantity > 0)
        //    {
        //        p.PipeQuantity = p.PipeQuantity / 1000;
        //    }
        //    if (p.TotalQuantity > 0)
        //    {
        //        p.TotalQuantity = p.TotalQuantity / 1000;
        //    }
        //    return p;
        //}
 
        //private MeasuringPointsDataViewModel FillModelObject(TransactionDataModel transactionData, 
        //    SortedDictionary<int, MeasuringPointsDataViewModel> dict, 
        //    int? flowDirection)
        //{
        //    var measuringPointData = new MeasuringPointsDataViewModel();
        //    if (transactionData.DirectionId == 3)
        //    {
        //        // data from Pt Rosenec
        //        if (flowDirection == 2 && (transactionData.MassReverse.HasValue == false || transactionData.MassReverse.Value == 0))
        //        {
        //            return null;
        //        }
        //        if (flowDirection == 1 && (transactionData.Mass.HasValue == false || transactionData.Mass.Value == 0))
        //        {
        //            return null;
        //        }
        //    }

        //    if (dict.ContainsKey(transactionData.ProductId))
        //    {
        //        measuringPointData = dict[transactionData.ProductId];
        //    }
                
        //    measuringPointData.ProductId = transactionData.ProductId;

        //    if (transactionData.TransportId == 1)
        //    {
        //        measuringPointData.AvtoQuantity += transactionData.RealMass;
        //    }
        //    else if (transactionData.TransportId == 2)
        //    {
        //        measuringPointData.JpQuantity += transactionData.RealMass;
        //    }
        //    else if (transactionData.TransportId == 3)
        //    {
        //        measuringPointData.SeaQuantity += transactionData.RealMass;
        //    }
        //    else if (transactionData.TransportId == 4)
        //    {
        //        measuringPointData.PipeQuantity += transactionData.RealMass;
        //    }
        //    measuringPointData.TotalQuantity += transactionData.RealMass;

        //    return measuringPointData;
        //}
 
        private void ValidateModelState(DateTime? date, int? directionId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
            if (directionId == null || directionId.Value == CommonConstants.InputOutputDirection)
            {
                this.ModelState.AddModelError("direction", string.Format(Resources.ErrorMessages.Required, Resources.Layout.Direction));
            }
        }
    }
}