﻿namespace CollectingProductionDataSystem.Application.ProductionPlanDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Data.Contracts;

    public class ProductionPlanDataService : IProductionPlanDataService
    {
        private readonly IProductionData data;
        private readonly ICalculatorService calculator;
        private readonly IUnitsDataService unitData;

        public ProductionPlanDataService(ICalculatorService calculatorParam, IProductionData dataParam, IUnitsDataService unitData)
        {
            this.data = dataParam;
            this.calculator = calculatorParam;
            this.unitData = unitData;
        }

        public IEnumerable<ProductionPlanData> ReadProductionPlanData(DateTime? date, int? processUnitId, int? materialTypeId)
        {
            var result = new HashSet<ProductionPlanData>();

            var dailyData = unitData.GetUnitsDailyDataForDateTime(date, processUnitId, materialTypeId).ToList();
            if (dailyData.Count == 0)
            {
                return result;
            }

            var dbResult = this.data.ProductionPlanConfigs.All();
            if (processUnitId != null)
            {
                dbResult = dbResult.Where(x => x.ProcessUnitId == processUnitId);
            }
            if (materialTypeId != null)
            {
                dbResult = dbResult.Where(x => x.MaterialTypeId == materialTypeId);
            }

            var productionPlans = dbResult
                .OrderBy(x => x.ProcessUnitId)
                .ThenBy(x=>x.Position)
                .ToList();

            var currentMonthQuantity = AppendTotalMonthQuantityToDailyProductionPlanRecords(processUnitId.Value, date.Value);
            var totallyQuantity = GetSumOfTottallyProcessing(processUnitId.Value, date.Value);
            var totallyQuantityAs = 0m;

            foreach (ProductionPlanConfig productionPlan in productionPlans)
            {
                var planValue = CalculatePlanValue(productionPlan, dailyData, this.calculator);
                var factValue = CalculateFactValue(productionPlan, dailyData, this.calculator);
                var realValue = dailyData.Where(x => x.UnitsDailyConfig.Code == productionPlan.QuantityPlanMembers).FirstOrDefault().RealValue;
                var factPercents = (factValue * 100.00) / realValue;

                var productionPlanData = new ProductionPlanData
                {
                    ProductionPlanConfigId = productionPlan.Id,
                    RecordTimestamp = date.Value,
                    PercentagesPlan = productionPlan.Percentages,
                    QuanityPlan = (decimal)planValue,
                    PercentagesFact = double.IsNaN(factPercents) ? 0.0m : (decimal)factPercents,
                    QuantityFact = double.IsNaN(factValue) ? 0.0m : (decimal)factValue,
                    Name = productionPlan.Name,
                    FactoryId = productionPlan.ProcessUnit.FactoryId,
                    ProcessUnitId = processUnitId.Value,
                    QuanityFactCurrentMonth = currentMonthQuantity.ContainsKey(productionPlan.Name) == true ?  currentMonthQuantity[productionPlan.Name] : 0.00m,
                };

                if (productionPlan.Name == totallyQuantity.First().Key)
	            {
                    if (totallyQuantity[productionPlan.Name] == 0)
                    {
                        totallyQuantityAs = productionPlanData.QuantityFact;   
                    }
                    else 
                    {
                        if (totallyQuantity[productionPlan.Name] == productionPlanData.QuanityFactCurrentMonth + productionPlanData.QuantityFact)
                        {
                            totallyQuantityAs = totallyQuantity[productionPlan.Name];   
                        }
                        else
                        {
                            totallyQuantityAs = productionPlanData.QuanityFactCurrentMonth + productionPlanData.QuantityFact;
                        }
                    }
	            }

                result.Add(productionPlanData);
            }

            foreach (var item in result)
            {
                var percs = (((double)item.QuanityFactCurrentMonth + (double)item.QuantityFact) * 100.00) / (double)totallyQuantityAs;
                item.PercentagesFactCurrentMonth = double.IsNaN(percs) || double.IsInfinity(percs) ? 0.0m : (decimal)percs;
            }

            return result;
        }

        private double CalculateFactValue(ProductionPlanConfig productionPlan, List<UnitsDailyData> dailyData, ICalculatorService calculator)
        {
            var splitter = new char[] { '@' };

            var factTokens = productionPlan.QuantityFactMembers.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            var factInputParamsValues = new List<double>();
            foreach (var token in factTokens)
            {
                foreach (var item in dailyData)
                {
                    if (item.UnitsDailyConfig.Code == token)
                    {
                        factInputParamsValues.Add((double)item.RealValue);
                        break;
                    }
                }
            }

            var factInputParams = new Dictionary<string, double>();
            for (int i = 0; i < factInputParamsValues.Count(); i++)
            {
                factInputParams.Add(string.Format("p{0}", i), factInputParamsValues[i]);  
            }
                
            var factValue = calculator.Calculate(productionPlan.QuantityFactFormula, "p", factInputParams.Count, factInputParams);
            return factValue;
        }
 
        private double CalculatePlanValue(ProductionPlanConfig productionPlan, List<UnitsDailyData> dailyData, ICalculatorService calculator)
        {
            var splitter = new char[] { '@' };

            var planTokens = productionPlan.QuantityPlanMembers.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            var planInputParamsValues = new List<double>();
            foreach (var token in planTokens)
            {
                foreach (var item in dailyData)
                {
                    if (item.UnitsDailyConfig.Code == token)
                    {
                        planInputParamsValues.Add((double)item.RealValue);
                    }
                }
            }

            var planInputParams = new Dictionary<string, double>();
            for (int i = 0; i < planInputParamsValues.Count(); i++)
            {
                planInputParams.Add(string.Format("p{0}", i), planInputParamsValues[i]);  
            }

            var planValue = calculator.Calculate(productionPlan.QuantityPlanFormula, "p", planInputParams.Count, planInputParams);
            return planValue;
        }

        private Dictionary<string, decimal>  AppendTotalMonthQuantityToDailyProductionPlanRecords(int processUnitId, DateTime targetDay)
        {
            var beginningOfMonth = new DateTime(targetDay.Year, targetDay.Month, 1);
            var totalMonthProductionPlanQuantities = data.ProductionPlanDatas.All().Include(x => x.ProductionPlanConfig)
                .Where(x => x.ProductionPlanConfig.ProcessUnitId == processUnitId &&
                        beginningOfMonth <= x.RecordTimestamp &&
                        x.RecordTimestamp < targetDay).GroupBy(x => x.ProductionPlanConfig.Name).ToList()
                        .Select(group => new { Name = group.Key, Value = group.Sum(x => x.QuantityFact) }).ToDictionary(x => x.Name, x => x.Value);

            return totalMonthProductionPlanQuantities;
        }

        private Dictionary<string, decimal> GetSumOfTottallyProcessing(int processUnitId, DateTime targetDay)
        {
            var beginningOfMonth = new DateTime(targetDay.Year, targetDay.Month, 1);
            var totalsRecords = data.ProductionPlanDatas.All()
                .Include(x => x.ProductionPlanConfig)
                .Where(x => x.ProductionPlanConfig.ProcessUnitId == processUnitId &&
                    x.ProductionPlanConfig.IsSummaryOfProcessing == true &&
                    (beginningOfMonth <= x.RecordTimestamp && x.RecordTimestamp <= targetDay))
                .GroupBy(x => x.ProductionPlanConfig.Name)
                .ToList()
                .Select(group => new 
                { 
                    Name = group.Key, 
                    Value = group.Sum(x => x.QuantityFact) 
                })
                .ToDictionary(x => x.Name, x => x.Value);

            if (totalsRecords.Count == 0)
            {
                var summaryProductionPlanConfig = this.data.ProductionPlanConfigs
                    .All()
                    .Where(x => x.ProcessUnitId == processUnitId && x.IsSummaryOfProcessing == true)
                    .FirstOrDefault();
                if (summaryProductionPlanConfig == null)
                {
                    totalsRecords.Add("Сума преработка", 0m);
                }
                else
                {
                    totalsRecords.Add(summaryProductionPlanConfig.Name, 0m);
                }
            }

            return totalsRecords;
        }
    }
}
