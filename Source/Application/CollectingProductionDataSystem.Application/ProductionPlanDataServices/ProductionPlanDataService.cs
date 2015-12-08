namespace CollectingProductionDataSystem.Application.ProductionPlanDataServices
{
    using System;
    using System.Collections.Generic;
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

        public IEnumerable<ProductionPlanData> ReadProductionPlanData(DateTime? date, int? processUnitId)
        {
            var result = new HashSet<ProductionPlanData>();

            if (!date.HasValue || !processUnitId.HasValue)
            {
                return result;
            }

            var dailyData = unitData.GetUnitsDailyDataForDateTime(date, processUnitId).ToList();
            if (dailyData.Count == 0)
            {
                return result;
            }

            var dbResult = this.data.ProductionPlanConfigs.All();
            if (processUnitId != null)
            {
                dbResult = dbResult.Where(x => x.ProcessUnitId == processUnitId.Value);
            }

            var productionPlans = dbResult
                .OrderBy(x => x.ProcessUnitId)
                .ThenBy(x=>x.Position)
                .ToList();
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
                    QuantityFact = (decimal)factValue,
                    Name = productionPlan.Name,
                    FactoryId = productionPlan.ProcessUnit.FactoryId,
                    ProcessUnitId = processUnitId.Value,
                };
                result.Add(productionPlanData);
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
    }
}
