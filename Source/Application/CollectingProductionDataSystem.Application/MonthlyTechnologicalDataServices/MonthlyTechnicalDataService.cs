

namespace CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices
{
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Extentions;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class MonthlyTechnicalDataService : IMonthlyTechnicalDataService
    {
        private readonly IProductionData data;
        private readonly ICalculatorService calculator;

        public MonthlyTechnicalDataService(IProductionData dataParam, ICalculatorService calculatorParam)
        {
            this.data = dataParam;
            this.calculator = calculatorParam;
        }

        public  IEnumerable<MonthlyTechnicalReportDataDto> CalculateMonthlyTechnologicalData(DateTime month)
        {
            var monthDate = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month), 0, 0, 0);
            var firstDayInMonth = new DateTime(month.Year, month.Month, 1, 0, 0, 0);

            var productionPlanData = this.data.ProductionPlanDatas.All()
                .Include(x => x.ProductionPlanConfig)
                .Include(x => x.ProductionPlanConfig.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers)
                .Include(x => x.ProductionPlanConfig.ProductionPlanConfigUnitMonthlyConfigPlanMembers)
                .Include(x => x.ProductionPlanConfig.PlanNorms)
                .Where(x => x.RecordTimestamp == monthDate)
                .ToList();

            var monthlyProductionDataList = data.UnitMonthlyConfigs
                .AllAnual(monthDate.Year)
                .Include(x => x.UnitMonthlyDatas)
                .Include(x => x.ProcessUnit)
                .Include(x => x.ProcessUnit.Factory)
                .Include(x => x.MeasureUnit)
                .Include(x => x.ProductionPlanConfig)
                .Where(x => x.IsAvailableInTechnologicalReport)
                .Where(x => x.ProcessUnitId == 1)
                .OrderBy(x => x.ProcessUnitId)
                .ThenBy(x => x.MonthlyReportTypeId)
                .ToList()
                .SelectMany(y => y.UnitMonthlyDatas.Where(z => z.RecordTimestamp == monthDate))
                .ToList();

            Dictionary<string, MonthlyData> monthlyData = data.UnitMonthlyDatas.All().Where(x => x.RecordTimestamp == monthDate).ToDictionary(x => x.UnitMonthlyConfig.Code, y => new MonthlyData { MonthValue = (decimal)y.RealValue, YearValue = y.YearTotalValue });

            var monthlyProductionData = new List<MonthlyTechnicalReportDataDto>();

            foreach (var item in monthlyProductionDataList)
            {
                var planValue = GetPlanValue(item, productionPlanData, firstDayInMonth);
                var planPercentage = GetPlanPercentage(item, productionPlanData, firstDayInMonth);
                var factPercentage = CalculateFactPercentage(item, productionPlanData, monthlyData);
                var yearPercentage = CalculateYearPercentage(item, productionPlanData);

                var m = new MonthlyTechnicalReportDataDto()
                {
                    Id = item.UnitMonthlyConfig.Id,
                    Code = item.UnitMonthlyConfig.Code,
                    Name = item.UnitMonthlyConfig.Name,
                    Factory = item.UnitMonthlyConfig.ProcessUnit.Factory.FactorySortableName,
                    ProcessUnit = item.UnitMonthlyConfig.ProcessUnit.SortableName,
                    MaterialType = item.UnitMonthlyConfig.MonthlyReportType.Name,
                    MeasurementUnit = item.UnitMonthlyConfig.MeasureUnit.Code,

                    PlanValue = (decimal)planValue,
                    PlanPercentage = (decimal)planPercentage,

                    FactValue = (decimal)item.RealValue,
                    FactPercentage = (decimal)factPercentage,
                    FactValueDifference = (decimal)(item.RealValue - planValue),
                    FactPercentageDifference = (decimal)(factPercentage - planPercentage),

                    YearValue = item.YearTotalValue,
                    YearPercentage = 0,
                    YearValueDifference = 0,
                    YearPercentageDifference = 0
                };
                monthlyProductionData.Add(m);
            }

            return monthlyProductionData;
        }

        private double GetPlanValue(UnitMonthlyData monthlyData, IList<ProductionPlanData> productionPlanDatas, DateTime targetMonth)
        {
            if (monthlyData.UnitMonthlyConfig.ProductionPlanConfig == null)
            {
                return 0.0;
            }

            var productionPlanData = productionPlanDatas.Where(x => x.ProductionPlanConfigId == monthlyData.UnitMonthlyConfig.ProductionPlanConfigId).FirstOrDefault();

            if (productionPlanData == null)
            {
                return 0.0;    
            }

            int daysInMonth = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);

            if (productionPlanData.ProductionPlanConfig.MaterialTypeId == CommonConstants.MaterialType)
            {
                var inputDictionary = new Dictionary<string, double>();
                inputDictionary.Add("pn", (double)productionPlanData.PercentagesPlan);
                inputDictionary.Add("pv", (double)productionPlanData.ProductionPlanConfig.ProcessUnit.PlanValues.Where(x => x.Month == targetMonth).FirstOrDefault().Value);
                return calculator.Calculate(productionPlanData.ProductionPlanConfig.MonthlyValuePlanFormula, "p", inputDictionary.Count, inputDictionary);
            }
            else
            {
                return (double)productionPlanData.QuanityPlan * daysInMonth;
            }
        }


        private double GetPlanPercentage(UnitMonthlyData monthlyData, IList<ProductionPlanData> productionPlanDatas, DateTime firstDayInMonth)
        {
            if (monthlyData.UnitMonthlyConfig.ProductionPlanConfig == null)
            {
                return 0.0;
            }

            var productionPlanData = productionPlanDatas.Where(x => x.ProductionPlanConfigId == monthlyData.UnitMonthlyConfig.ProductionPlanConfigId).FirstOrDefault();

            if (productionPlanData == null)
            {
                return 0.0;    
            }

            var planNorm = productionPlanData.ProductionPlanConfig.PlanNorms.Where(x => x.Month == firstDayInMonth).FirstOrDefault();
            if (planNorm == null)
            {
                return 0.0;
            }

            return (double)planNorm.Value;
        }

        private double CalculateFactPercentage(UnitMonthlyData monthlyData, List<ProductionPlanData> productionPlanDatas, Dictionary<string, MonthlyData> monthlyApprovedDatas)
        {
            var productionPlanData = productionPlanDatas.Where(x => x.ProductionPlanConfig.Id == monthlyData.UnitMonthlyConfig.ProductionPlanConfigId).FirstOrDefault();

            if (productionPlanData == null)
            {
                return 0.0;    
            }

            var inputDictionary = new Dictionary<string, double>();
            var relatedMonthlyDatas = productionPlanData.ProductionPlanConfig.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers;
            foreach (var relatedMonthlyData in relatedMonthlyDatas)
            {
                var value = (double)monthlyApprovedDatas[relatedMonthlyData.UnitMonthlyConfig.Code].MonthValue;
                inputDictionary.Add(string.Format("p{0}", relatedMonthlyData.Position - 1), value);  
            }
            return calculator.Calculate(productionPlanData.ProductionPlanConfig.MonthlyFactFractionFormula, "p", inputDictionary.Count, inputDictionary);
        }



        private decimal CalculateYearPercentage(UnitMonthlyData monthlyData, List<ProductionPlanData> productionPlanDatas)
        {
            var productionPlanData = productionPlanDatas.Where(x => x.ProductionPlanConfig.Id == monthlyData.UnitMonthlyConfig.ProductionPlanConfigId).FirstOrDefault();

            if (productionPlanData == null)
            {
                return 0.0m;    
            }

            return productionPlanData.ProductionPlanConfig.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers.Count();
        }






    }

    class MonthlyData 
    {
        public decimal MonthValue { get; set; }

        public decimal YearValue { get; set; }
    }
}
