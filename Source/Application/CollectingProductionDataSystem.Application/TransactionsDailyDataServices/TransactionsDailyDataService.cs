
namespace CollectingProductionDataSystem.Application.TransactionsDailyDataServices
{
    using System.Data.Entity;
    using System.Diagnostics;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Transactions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Constants;

    public class TransactionsDailyDataService : ITransactionsDailyDataService
    {
        private readonly IProductionData data;

        public TransactionsDailyDataService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public HashSet<MeasuringPointsConfigsReportData> ReadTransactionsDailyData(DateTime date, int flowDirection)
        {
            var transactionsData = GetMeasurementPointsDataByDateAndDirection(date, flowDirection)
                .Select(t => new TransactionDataDto
            {
                MeasuringPointId = t.MeasuringPointId,
                DirectionId = t.MeasuringPointConfig.FlowDirection.Value,
                TransportId = t.MeasuringPointConfig.TransportTypeId,
                ProductId = t.ProductNumber.Value,
                Mass = t.Mass,
                MassReverse = t.MassReverse,
                FlowDirection = t.MeasuringPointConfig.FlowDirection.Value,
            });

            var dict = new SortedDictionary<int, MeasuringPointsConfigsReportData>();
                
            foreach (var transactionData in transactionsData)
            {
                var measuringPointData = FillModelObject(transactionData, dict, flowDirection);
                if (measuringPointData != null)
                {
                    dict[transactionData.ProductId] = measuringPointData;   
                }
            }

            var actDict = GetActiveTransactionsData(date, flowDirection);

            var monthTransactions = GetMeasurementPointsDataByDateAndDirection(date, flowDirection, true)
                .Select(t => new TransactionDataDto
            {
                MeasuringPointId = t.MeasuringPointId,
                DirectionId = t.MeasuringPointConfig.FlowDirection.Value,
                TransportId = t.MeasuringPointConfig.TransportTypeId,
                ProductId = t.ProductNumber.Value,
                Mass = t.Mass,
                MassReverse = t.MassReverse,
                FlowDirection = t.MeasuringPointConfig.FlowDirection.Value,
            });

            var totallyQuantityByProducts = new Dictionary<int, decimal>();
            foreach (var item in monthTransactions)
            {
                if (item.DirectionId == CommonConstants.InputOutputDirection)
                {
                    if (flowDirection == CommonConstants.InputDirection 
                        && (item.MassReverse.HasValue == false || item.MassReverse.Value == 0))
                    {
                        continue;
                    }
                    if (flowDirection == CommonConstants.OutputDirection 
                        && (item.Mass.HasValue == false || item.Mass.Value == 0))
                    {
                        continue;
                    }
                }

                if (totallyQuantityByProducts.ContainsKey(item.ProductId))
                {
                    totallyQuantityByProducts[item.ProductId] = totallyQuantityByProducts[item.ProductId] + item.RealMass;
                }
                else
                {
                    totallyQuantityByProducts[item.ProductId] = item.RealMass;
                }
            }

            var hs = PopulateAllMeaurementPointDatas(dict, actDict, totallyQuantityByProducts);
            return hs;
        }

        private IQueryable<MeasuringPointsConfigsData> GetMeasurementPointsDataByDateAndDirection(DateTime? date, int? flowDirection, bool checkMonthData = false)
        {
            var transactions = this.data.MeasuringPointsConfigsDatas
                                   .All()
                                   .Include(x => x.MeasuringPointConfig)
                                   .Where(x => x.MeasuringPointConfig.IsInternalPoint == false)
                                   .Where(x => x.RowId == -1);

            if (date.HasValue)
            {
                var beginTimestamp = new DateTime();
                var endTimestamp = new DateTime();
                if (!checkMonthData)
                {
                    beginTimestamp = date.Value.AddMinutes(300);
                    endTimestamp = beginTimestamp.AddMinutes(1440);  
                }
                else
                {
                    beginTimestamp = new DateTime(date.Value.Year, date.Value.Month, 1, 5, 0, 0);
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

        private Dictionary<int, decimal> GetActiveTransactionsData(DateTime? date, int? flowDirection)
        {
            var actDict = new Dictionary<int, decimal>();
            var activeTransactionsDatas = this.data.ActiveTransactionsDatas
                                              .All()
                                              .Include(x => x.Product)
                                              .Include(x => x.MeasuringPointConfig)
                                              .Where(x => x.MeasuringPointConfig.IsInternalPoint == false)
                                              .Where(x => !string.IsNullOrEmpty(x.MeasuringPointConfig.ActiveTransactionStatusTag))
                                              .Where(x => x.RecordTimestamp == date.Value)
                                              .Where(x => x.MeasuringPointConfig.FlowDirection.Value == flowDirection || x.MeasuringPointConfig.FlowDirection.Value == 3)
                                              .ToList();

            foreach (var activeTransaction in activeTransactionsDatas)
            {
                var direction = activeTransaction.MeasuringPointConfig.FlowDirection.Value;

                if (direction == 3)
                {
                    // data from Pt Rosenec
                    if (flowDirection == CommonConstants.InputDirection && activeTransaction.MassReverse == 0)
                    {
                        continue;
                    }
                    if (flowDirection == CommonConstants.OutputDirection && activeTransaction.Mass == 0)
                    {
                        continue;
                    }
                }

                var prodId = data.Products.All().Where(p => p.Id == activeTransaction.ProductId).FirstOrDefault().Code;
                if (actDict.ContainsKey(prodId))
                {
                    if (direction == CommonConstants.OutputDirection)
                    {
                        actDict[prodId] = actDict[prodId] + activeTransaction.Mass; 
                    }
                    else if (direction == CommonConstants.InputDirection)
                    {
                        actDict[prodId] = actDict[prodId] + activeTransaction.MassReverse;  
                    }
                    else 
                    {
                        if (flowDirection == CommonConstants.OutputDirection)
                        {
                            actDict[prodId] = actDict[prodId] + activeTransaction.Mass;    
                        }
                        else
                        {
                            actDict[prodId] = actDict[prodId] + activeTransaction.MassReverse;  
                        }
                    }
                }
                else
                {
                    if (direction == CommonConstants.OutputDirection)
                    {
                        actDict[prodId] = activeTransaction.Mass; 
                    }
                    else if (direction == CommonConstants.InputDirection)
                    {
                        actDict[prodId] = activeTransaction.MassReverse;  
                    }
                    else 
                    {
                        if (flowDirection == CommonConstants.OutputDirection)
                        {
                            actDict[prodId] = activeTransaction.Mass;    
                        }
                        else
                        {
                            actDict[prodId] = activeTransaction.MassReverse;  
                        }
                    }
                }
            }

            return actDict;
        }

        private MeasuringPointsConfigsReportData FillModelObject(TransactionDataDto transactionData, SortedDictionary<int, MeasuringPointsConfigsReportData> dict,  int? flowDirection)
        {
            var measuringPointData = new MeasuringPointsConfigsReportData();
            if (transactionData.DirectionId == CommonConstants.InputOutputDirection)
            {
                // data from Pt Rosenec
                if (flowDirection == CommonConstants.InputDirection 
                    && (transactionData.MassReverse.HasValue == false || transactionData.MassReverse.Value == 0))
                {
                    return null;
                }
                if (flowDirection == CommonConstants.OutputDirection 
                    && (transactionData.Mass.HasValue == false || transactionData.Mass.Value == 0))
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

        private HashSet<MeasuringPointsConfigsReportData> PopulateAllMeaurementPointDatas(SortedDictionary<int, MeasuringPointsConfigsReportData> dict, 
            Dictionary<int, decimal> actDict, Dictionary<int, decimal> totallyQuantityByProducts)
        {
            var hs = new HashSet<MeasuringPointsConfigsReportData>();
            foreach (var item in dict)
            {
                var p = SetMeasuringPointsDataViewModel(item, actDict, totallyQuantityByProducts);
                if (p.TotalQuantity > 0)
                {
                    hs.Add(p);   
                }
            }

            if (totallyQuantityByProducts != null && totallyQuantityByProducts.Count > 0)
            {
                foreach (var item in totallyQuantityByProducts)
                {
                    var m = new MeasuringPointsConfigsReportData();
                    m.ProductId = item.Key;
                    m.ProductName = this.data.Products.All().Where(x => x.Code == item.Key).FirstOrDefault().Name;
                    m.TotalMonthQuantity = item.Value / 1000;
                    if (actDict != null && actDict.ContainsKey(item.Key))
                    {
                        m.ActiveQuantity = actDict[item.Key] / 1000;
                        actDict.Remove(item.Key);
                    }
                    hs.Add(m);
                }
            }

            if (actDict != null && actDict.Count > 0)
            {
                foreach (var item in actDict)
                {
                    var m = new MeasuringPointsConfigsReportData();
                    m.ProductId = item.Key;
                    m.ProductName = this.data.Products.All().Where(x => x.Code == item.Key).FirstOrDefault().Name;
                    m.ActiveQuantity = item.Value / 1000;
                    hs.Add(m);
                }
            }

            return hs;
        }

        private MeasuringPointsConfigsReportData SetMeasuringPointsDataViewModel(KeyValuePair<int, MeasuringPointsConfigsReportData> item, 
            Dictionary<int, decimal> actDict, Dictionary<int, decimal> totallyQuantityByProducts)
        {
            var p = item.Value;
            p.ProductName = this.data.Products.All().Where(x => x.Code == p.ProductId).FirstOrDefault().Name;
            if (actDict != null && actDict.ContainsKey(p.ProductId))
            {
                p.ActiveQuantity = actDict[p.ProductId] / 1000;
                actDict.Remove(p.ProductId);
            }

            if (totallyQuantityByProducts != null && totallyQuantityByProducts.ContainsKey(p.ProductId))
            {
                p.TotalMonthQuantity = totallyQuantityByProducts[p.ProductId] / 1000;
                totallyQuantityByProducts.Remove(p.ProductId);
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
    }
}
