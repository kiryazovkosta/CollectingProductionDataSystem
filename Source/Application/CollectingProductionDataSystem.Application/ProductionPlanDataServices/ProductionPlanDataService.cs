namespace CollectingProductionDataSystem.Application.ProductionPlanDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Constants;

    public class ProductionPlanDataService : IProductionPlanDataService
    {
        private readonly IProductionData data;
        private readonly ICalculatorService calculator;
        private readonly IUnitsDataService unitData;
        private readonly IUnitDailyDataService dailyData;

        public ProductionPlanDataService(ICalculatorService calculatorParam, IProductionData dataParam, IUnitsDataService unitData, IUnitDailyDataService dailyDataParam)
        {
            this.data = dataParam;
            this.calculator = calculatorParam;
            this.unitData = unitData;
            this.dailyData = dailyDataParam;
        }

        public IEnumerable<ProductionPlanData> ReadProductionPlanData(DateTime? date, int? processUnitId, int? materialTypeId)
        {
            var energyId = CommonConstants.EnergyType;
            var result = new HashSet<ProductionPlanData>();
            var dailyData = new List<UnitsDailyData>();

            var status = this.dailyData.CheckIfPreviousDaysAreReady(processUnitId.Value, date.Value, materialTypeId.Value);
            if (!status.IsValid)
            {
                return result;
            }

            if (materialTypeId.HasValue &&
                materialTypeId.Value == energyId &&
                !this.dailyData.CheckIfDayIsApproved(date.Value, processUnitId.Value))
            {
                return result;
            }

            if (materialTypeId.HasValue && materialTypeId.Value == energyId)
            {
                dailyData = unitData.GetUnitsDailyDataForDateTime(date, null, null).ToList();
            }
            else
            {
                dailyData = unitData.GetUnitsDailyDataForDateTime(date, processUnitId, null).ToList();
            }

            if (dailyData.Count == 0)
            {
                return result;
            }

            var existingProductionPlanData = this.data.ProductionPlanDatas.All()
                .Include(p => p.ProductionPlanConfig)
                .Include(p => p.ProductionPlanConfig.MaterialType)
                .Where(p => p.RecordTimestamp == date &&
                        p.ProcessUnitId == processUnitId &&
                        p.ProductionPlanConfig.MaterialTypeId == materialTypeId &&
                        p.ProductionPlanConfig.IsPropductionPlan == true)
                .ToList();
            if (existingProductionPlanData.Count > 0)
	        {
                return existingProductionPlanData;
	        }

            var dbResult = this.data.ProductionPlanConfigs.All()
                .Include(x => x.PlanNorms)
                .Include(x => x.ProcessUnit)
                .Include(x => x.ProcessUnit.PlanValues);
                //.Where(x => x.IsPropductionPlan == true);
            if (processUnitId != null)
            {
                dbResult = dbResult.Where(x => x.ProcessUnitId == processUnitId);
            }
            if (materialTypeId != null)
            {
                if (materialTypeId.Value == CommonConstants.MaterialType)
                {
                    dbResult = dbResult.Where(x => x.MaterialTypeId == materialTypeId);
                }
                else
                {
                    dbResult = dbResult.Where(x => x.MaterialTypeId >= materialTypeId);
                }

            }

            var productionPlans = dbResult
                .OrderBy(x => x.ProcessUnitId)
                .ThenBy(x=>x.Position)
                .ToList();

            var currentMonthQuantity = AppendTotalMonthQuantityToDailyProductionPlanRecords(processUnitId.Value, date.Value);
            var totallyQuantity = GetSumOfTottallyProcessing(processUnitId.Value, date.Value);
            var totallyQuantityAs = 0m;

            var month = new DateTime(date.Value.Year, date.Value.Month, 1, 0, 0, 0);

            foreach (ProductionPlanConfig productionPlan in productionPlans)
            {
                var planValue = CalculatePlanValue(productionPlan, dailyData, this.calculator, month);
                var factValue = CalculateFactValue(productionPlan, dailyData, this.calculator);
                if (factValue < 0 && materialTypeId >= CommonConstants.EnergyType)
                {
                    factValue = 0;
                }
                var factPercents = CalculateUsageRateValue(productionPlan, dailyData, this.calculator);
                if (factPercents < 0 && materialTypeId >= CommonConstants.EnergyType)
                {
                    factPercents = 0;
                }

                var percentagesPlan = productionPlan.PlanNorms.Where(x => x.Month == month).First().Value;

                var productionPlanData = new ProductionPlanData
                {
                    ProductionPlanConfigId = productionPlan.Id,
                    ProductionPlanConfig = productionPlan,
                    RecordTimestamp = date.Value,
                    PercentagesPlan = percentagesPlan,
                    QuanityPlan = (decimal)planValue,
                    PercentagesFact = GetValidValueOrZero(factPercents),
                    QuantityFact = GetValidValueOrZero(factValue),
                    Name = productionPlan.Name,
                    FactoryId = productionPlan.ProcessUnit.FactoryId,
                    ProcessUnitId = processUnitId.Value,
                    QuanityFactCurrentMonth = currentMonthQuantity.ContainsKey(productionPlan.Name) == true ?  currentMonthQuantity[productionPlan.Name] : 0.00m,
                };

                if (materialTypeId >= CommonConstants.EnergyType)
                {
                    var percs = CalculateUsageRateToTheDayValue(productionPlan, dailyData, this.calculator, productionPlanData);
                    productionPlanData.PercentagesFactCurrentMonth = GetValidValueOrZero(percs);
                }

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

            if (materialTypeId >= CommonConstants.EnergyType && totallyQuantity.Count == 1)
            {
                totallyQuantityAs = totallyQuantity.First().Value;
            }

            foreach (var item in result)
            {
                if (materialTypeId == CommonConstants.MaterialType)
                {
                    var percs = (((double)item.QuanityFactCurrentMonth + (double)item.QuantityFact) * 100.00) / (double)totallyQuantityAs;
                    item.PercentagesFactCurrentMonth = GetValidValueOrZero(percs);
                }
            }

            return result;
        }

        private double CalculateUsageRateValue(ProductionPlanConfig productionPlan, List<UnitsDailyData> dailyData, ICalculatorService calculatorService)
        {
            var splitter = new char[] { '@' };

            var planTokens = productionPlan.UsageRateMembers.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            var planInputParamsValues = new List<double>();
            foreach (var token in planTokens)
            {
                foreach (var item in dailyData)
                {
                    if (item.UnitsDailyConfig.Code == token)
                    {
                        planInputParamsValues.Add((double)item.RealValue);
                        break;
                    }
                }
            }

            var planInputParams = new Dictionary<string, double>();
            for (int i = 0; i < planInputParamsValues.Count(); i++)
            {
                planInputParams.Add(string.Format("p{0}", i), planInputParamsValues[i]);
            }

            var planValue = calculator.Calculate(productionPlan.UsageRateFormula, "p", planInputParams.Count, planInputParams, productionPlan.Code);
            if (double.IsNaN(planValue) || double.IsInfinity(planValue))
            {
                planValue = 0.0;
            }
            return planValue;
        }

        private double CalculateUsageRateToTheDayValue(ProductionPlanConfig productionPlan, List<UnitsDailyData> dailyData, ICalculatorService calculatorService,
            ProductionPlanData productionPlanData)
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
                        planInputParamsValues.Add((double)item.RealValueTillDay);
                        break;
                    }
                }
            }

            var formula = new StringBuilder("p.pt/(");

            var planInputParams = new Dictionary<string, double>();
            planInputParams.Add("pt", (double)(productionPlanData.QuanityFactCurrentMonth + productionPlanData.QuantityFact));
            for (int i = 0; i < planInputParamsValues.Count(); i++)
            {
                planInputParams.Add(string.Format("p{0}", i), planInputParamsValues[i]);
                formula.Append(string.Format("p.p{0}", i));
                if (i + 1 == planInputParamsValues.Count())
	            {
		            formula.Append(")");
	            }
                else
                {
                    formula.Append("+");
                }
            }

            var planValue = calculator.Calculate(formula.ToString(), "p", planInputParams.Count, planInputParams, productionPlan.Code);
            if (double.IsNaN(planValue) || double.IsInfinity(planValue))
            {
                planValue = 0.0;
            }
            return planValue;
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

            var factValue = calculator.Calculate(productionPlan.QuantityFactFormula, "p", factInputParams.Count, factInputParams, productionPlan.Code);
            return factValue;
        }

        private double CalculatePlanValue(ProductionPlanConfig productionPlan, List<UnitsDailyData> dailyData, ICalculatorService calculator, DateTime month)
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
                        break;
                    }
                }
            }

            var planInputParams = new Dictionary<string, double>();
            for (int i = 0; i < planInputParamsValues.Count(); i++)
            {
                planInputParams.Add(string.Format("p{0}", i), planInputParamsValues[i]);
            }

            if (productionPlan.QuantityPlanFormula.IndexOf("p.pn") != -1)
            {
                var value = (double)productionPlan.PlanNorms.Where(x => x.Month == month).First().Value;
                planInputParams.Add("pn", value);
            }

            if (productionPlan.QuantityPlanFormula.IndexOf("p.pv") != -1)
            {
                var value = (double)productionPlan.ProcessUnit.PlanValues.Where(x => x.Month == month).First().Value;
                planInputParams.Add("pv", value);
            }

            if (productionPlan.QuantityPlanFormula.IndexOf("p.pl") != -1)
            {
                decimal? dbResult = productionPlan.ProcessUnit.PlanValues.Where(x => x.Month == month).First().ValueLiquid;
                if (!dbResult.HasValue)
                {
                    throw new ArgumentNullException("p.pl", $"No ValueLiquid entered in ~/PlanConfiguration/PlanValues for  ProcessUnitId:\n {productionPlan.ProcessUnit.ShortName}\nMonth: {month}");
                }
                var value = (double) dbResult.Value;
                planInputParams.Add("pl", value);
            }

            var planValue = calculator.Calculate(productionPlan.QuantityPlanFormula, "p", planInputParams.Count, planInputParams, productionPlan.Code);
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

        private decimal GetValidValueOrZero(double value)
        {
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                return 0.0m;
            }

            return (decimal)value;
        }
    }
}
