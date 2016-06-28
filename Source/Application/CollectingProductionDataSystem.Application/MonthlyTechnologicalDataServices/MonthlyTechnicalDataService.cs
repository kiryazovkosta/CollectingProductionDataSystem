

namespace CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices
{
    using System.ComponentModel.DataAnnotations;
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
    using Ninject;
    using Resources = App_Resources;
    using CollectingProductionDataSystem.Data.Common;

    public class MonthlyTechnicalDataService : IMonthlyTechnicalDataService
    {
        private readonly IProductionData data;
        private readonly IKernel kernel;
        private readonly ICalculatorService calculator;

        public MonthlyTechnicalDataService(IProductionData dataParam, IKernel kernelParam, ICalculatorService calculatorParam)
        {
            this.data = dataParam;
            this.kernel = kernelParam;
            this.calculator = calculatorParam;
        }

        public IEnumerable<MonthlyTechnicalReportDataDto> ReadMonthlyTechnologicalData(DateTime month, int[] processUnits)
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
                .Include(x => x.MonthlyReportType)
                .Include(x => x.ProductType)
                .Where(x => x.IsAvailableInTechnologicalReport)
                .Where(x => processUnits.Contains(x.ProcessUnitId))
                //.Where(x => x.ProcessUnitId == 4)
                .ToList()
                .SelectMany(y => y.UnitMonthlyDatas.Where(z => z.RecordTimestamp == monthDate))
                .ToList();

            Dictionary<string, MonthlyData> monthlyData = data.UnitMonthlyDatas.All()
                .Include(x => x.UnitMonthlyConfig)
                .Include(x => x.UnitManualMonthlyData)
                .Where(x => x.RecordTimestamp == monthDate)
                .ToList()
                .ToDictionary(x => x.UnitMonthlyConfig.Code, y => new MonthlyData { MonthValue = (decimal)y.RealValue, YearValue = y.YearTotalValue });

            var result = CalculateMonthlyTechnologicalData(monthlyProductionDataList, productionPlanData, firstDayInMonth, monthlyData);
            return result;
        }

        private IEnumerable<MonthlyTechnicalReportDataDto> CalculateMonthlyTechnologicalData(List<UnitMonthlyData> monthlyProductionDataList, List<ProductionPlanData> productionPlanData, DateTime firstDayInMonth, Dictionary<string, MonthlyData> monthlyData)
        {
            var processUnitsWithErrorInCalculation = new List<int>();
            var monthlyProductionData = new List<MonthlyTechnicalReportDataDto>();
            foreach (var item in monthlyProductionDataList)
            {
                if (processUnitsWithErrorInCalculation.Contains(item.UnitMonthlyConfig.ProcessUnitId))
                {
                    continue;
                }

                try
                {
                    var monthlyProductionDataRecord = CreateMonthlyTechnicalReportRecord(item, productionPlanData, firstDayInMonth, monthlyData);
                    if (monthlyProductionDataRecord != null)
                    {
                        monthlyProductionData.Add(monthlyProductionDataRecord);
                    }
                    else
                    {
                        if (!processUnitsWithErrorInCalculation.Contains(item.UnitMonthlyConfig.ProcessUnitId))
                        {
                            processUnitsWithErrorInCalculation.Add(item.UnitMonthlyConfig.ProcessUnitId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!processUnitsWithErrorInCalculation.Contains(item.UnitMonthlyConfig.ProcessUnitId))
                    {
                        processUnitsWithErrorInCalculation.Add(item.UnitMonthlyConfig.ProcessUnitId);
                    }
                }
            }

            var monthlyProductionDataResult = monthlyProductionData.Where(m => !processUnitsWithErrorInCalculation.Contains(m.ProcessUnitId));//.ToList();
            return monthlyProductionDataResult;
        }

        private MonthlyTechnicalReportDataDto CreateMonthlyTechnicalReportRecord(UnitMonthlyData item, List<ProductionPlanData> productionPlanData, DateTime firstDayInMonth, Dictionary<string, MonthlyData> monthlyData)
        {
            var monthlyTechnicalReportDataDto = new MonthlyTechnicalReportDataDto();

            bool isOnlyMonthFactValuePosition = item.UnitMonthlyConfig.IsOnlyMonthFactValuePosition;

            var planValue = isOnlyMonthFactValuePosition ? 0 : GetPlanValue(item, productionPlanData, firstDayInMonth);
            var planPercentage = GetPlanPercentage(item, productionPlanData, firstDayInMonth);
            var factPercentage = isOnlyMonthFactValuePosition ? 0 : CalculateFactPercentage(item, productionPlanData, monthlyData);
            if (factPercentage == double.MinValue)
            {
                monthlyTechnicalReportDataDto = null;
            }

            if (monthlyTechnicalReportDataDto != null)
            {
                var yearPercentage = isOnlyMonthFactValuePosition ? 0 : CalculateYearPercentage(item, productionPlanData, monthlyData);
                if (yearPercentage == double.MinValue)
                {
                    monthlyTechnicalReportDataDto = null;
                }

                if (monthlyTechnicalReportDataDto != null)
                {
                    if (item.UnitMonthlyConfig.MonthlyReportTypeId == CommonConstants.HydroCarbons)
                    {
                        factPercentage = Math.Round(factPercentage, 2);
                        yearPercentage = Math.Round(yearPercentage, 2);
                    }
                    else
                    {
                        factPercentage = Math.Round(factPercentage, 5);
                        yearPercentage = Math.Round(yearPercentage, 5);
                    }

                    monthlyTechnicalReportDataDto = new MonthlyTechnicalReportDataDto()
                    {
                        Id = item.UnitMonthlyConfig.Id,
                        Code = item.UnitMonthlyConfig.Code,
                        Name = item.UnitMonthlyConfig.Name,
                        FactoryId = item.UnitMonthlyConfig.ProcessUnit.FactoryId,
                        Factory = item.UnitMonthlyConfig.ProcessUnit.Factory.FactorySortableName,
                        ProcessUnitId = item.UnitMonthlyConfig.ProcessUnitId,
                        ProcessUnit = item.UnitMonthlyConfig.ProcessUnit.SortableName,
                        MaterialType = GetMaterialType(item.UnitMonthlyConfig.MonthlyReportTypeId),
                        DetailedMaterialType = GetDetailedMaterialType(item),
                        MeasurementUnit = item.UnitMonthlyConfig.MeasureUnit.Code,
                        PlanValue = (decimal)planValue,
                        PlanPercentage = (decimal)planPercentage,
                        FactValue = (decimal)item.RealValue,
                        FactPercentage = (decimal)factPercentage,
                        FactValueDifference = Math.Round((decimal)(item.RealValue - planValue), 5),
                        FactPercentageDifference = Math.Round((decimal)(factPercentage - planPercentage), 5),
                        YearValue = item.YearTotalValue + (decimal)item.RealValue,
                        YearPercentage = (decimal)yearPercentage,
                        YearValueDifference = 0,
                        YearPercentageDifference = 0
                    };
                }
            }

            return monthlyTechnicalReportDataDto;
        }

        private string GetDetailedMaterialType(UnitMonthlyData item)
        {
            if (item.UnitMonthlyConfig.MonthlyReportTypeId == CommonConstants.HydroCarbons)
            {
                return string.Format("{0:00}.{1}", item.UnitMonthlyConfig.ProductTypeId, item.UnitMonthlyConfig.ProductType.Name);
            }
            else
            {
                return string.Format("{0:00}.{1}", item.UnitMonthlyConfig.MonthlyReportTypeId, item.UnitMonthlyConfig.MonthlyReportType.Name);
            }
        }

        private string GetMaterialType(int materialTypeId)
        {
            if (materialTypeId == CommonConstants.HydroCarbons)
            {
                return "01.Въглеводороди";
            }
            else if (materialTypeId == CommonConstants.Chemicals)
            {
                return "03.Химикали";
            }
            else
            {
                return "02.Енергоресурси";
            }
        }

        public IEfStatus CheckIfAllMonthReportAreApproved(DateTime month)
        {
            var status = kernel.Get<IEfStatus>();
            var validationResults = new List<ValidationResult>() { new ValidationResult(@Resources.ErrorMessages.MonthReportTypesConfirmationError) };

            var targetMonth = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month), 0, 0, 0);
            var approvedMonthReports = this.data.UnitApprovedMonthlyDatas.All().Where(x => x.RecordDate == targetMonth).ToDictionary(x => x.MonthlyReportTypeId, y => y.RecordDate);
            var reportTypes = this.data.MonthlyReportTypes.All().ToList();

            foreach (var reportType in reportTypes)
            {
                if (!approvedMonthReports.ContainsKey(reportType.Id))
                {
                    validationResults.Add(new ValidationResult(string.Format("\t\t- {0}", reportType.Name)));
                }
            }

            if (validationResults.Count() > 1)
            {
                status.SetErrors(validationResults);
            }

            return status;
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
                var formula = "(p.pn/100.00)*p.pv";
                if (productionPlanData.ProductionPlanConfig.MonthlyValuePlanFormula != null)
                {
                    formula = productionPlanData.ProductionPlanConfig.MonthlyValuePlanFormula;
                }
                return calculator.Calculate(formula, "p", inputDictionary.Count, inputDictionary);
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

            if (!string.IsNullOrEmpty(productionPlanData.ProductionPlanConfig.MonthlyFactFractionFormula))
            {
                var inputDictionary = new Dictionary<string, double>();
                var relatedMonthlyDatas = productionPlanData.ProductionPlanConfig.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers;
                foreach (var relatedMonthlyData in relatedMonthlyDatas)
                {
                    var value = (double)monthlyApprovedDatas[relatedMonthlyData.UnitMonthlyConfig.Code].MonthValue;
                    inputDictionary.Add(string.Format("p{0}", relatedMonthlyData.Position - 1), value);
                }
                double result = calculator.Calculate(productionPlanData.ProductionPlanConfig.MonthlyFactFractionFormula, "p", inputDictionary.Count, inputDictionary);
                if (double.IsNaN(result) || double.IsInfinity(result))
                {
                    result = 0.0;
                }

                return result;
            }
            else
            {
                return double.MinValue;
            }
        }

        private double CalculateYearPercentage(UnitMonthlyData monthlyData, List<ProductionPlanData> productionPlanDatas, Dictionary<string, MonthlyData> monthlyApprovedDatas)
        {
            var productionPlanData = productionPlanDatas.Where(x => x.ProductionPlanConfig.Id == monthlyData.UnitMonthlyConfig.ProductionPlanConfigId).FirstOrDefault();

            if (productionPlanData == null)
            {
                return 0.0;
            }

            if (!string.IsNullOrEmpty(productionPlanData.ProductionPlanConfig.MonthlyFactFractionFormula))
            {
                var inputDictionary = new Dictionary<string, double>();
                var relatedMonthlyDatas = productionPlanData.ProductionPlanConfig.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers;
                foreach (var relatedMonthlyData in relatedMonthlyDatas)
                {
                    var value = (double)monthlyApprovedDatas[relatedMonthlyData.UnitMonthlyConfig.Code].MonthValue + (double)monthlyApprovedDatas[relatedMonthlyData.UnitMonthlyConfig.Code].YearValue;
                    inputDictionary.Add(string.Format("p{0}", relatedMonthlyData.Position - 1), value);
                }

                double result = calculator.Calculate(productionPlanData.ProductionPlanConfig.MonthlyFactFractionFormula, "p", inputDictionary.Count, inputDictionary);
                if (double.IsNaN(result) || double.IsInfinity(result))
                {
                    result = 0.0;
                }
                return result;
            }
            else
            {
                return double.MinValue;
            }
        }






    }

    class MonthlyData
    {
        public decimal MonthValue { get; set; }

        public decimal YearValue { get; set; }
    }
}
