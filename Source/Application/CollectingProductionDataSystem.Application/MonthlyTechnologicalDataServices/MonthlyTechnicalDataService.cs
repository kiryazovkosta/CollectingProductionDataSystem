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
    using System.Diagnostics;
    using System.Threading.Tasks;
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

        public async Task<IEnumerable<MonthlyTechnicalReportDataDto>> ReadMonthlyTechnologicalDataAsync(DateTime month, int[] processUnits)
        {

            var monthDate = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month), 0, 0, 0);
            var firstDayInMonth = new DateTime(month.Year, month.Month, 1, 0, 0, 0);

            Dictionary<int, ProductionPlanDataDto> productionPlanData = await GetProductionPlanDataAsync(monthDate, firstDayInMonth);
            List<UnitMonthlyData> monthlyProductionDataList = await GetMonthlyProductDataListAsync(processUnits, monthDate);
            Dictionary<string, MonthlyData> monthlyData = await GetMonthlyDataAsync(monthDate);

            IEnumerable<MonthlyTechnicalReportDataDto> result = await CalculateMonthlyTechnologicalDataAsync(monthlyProductionDataList, productionPlanData, firstDayInMonth, monthlyData);
            return result;
        }

        private async Task<Dictionary<string, MonthlyData>> GetMonthlyDataAsync(DateTime monthDate)
        {
            // ToDo: Re-factor this query to simplify result set
            return await data.UnitMonthlyDatas.All()
                                       .Include(x => x.UnitMonthlyConfig)
                                       .Include(x => x.UnitManualMonthlyData)
                                       .Where(x => x.RecordTimestamp == monthDate)
                                       .ToDictionaryAsync(x => x.UnitMonthlyConfig.Code,
                                       y => new MonthlyData
                                       {
                                           MonthValue = (decimal) y.RealValue,
                                           YearValue = y.YearTotalValue
                                       });
        }

        private async Task<List<UnitMonthlyData>> GetMonthlyProductDataListAsync(int[] processUnits, DateTime monthDate)
        {
            var dbResult = await data.UnitMonthlyConfigs
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
                       .ToListAsync()
; return dbResult.SelectMany(y => y.UnitMonthlyDatas.Where(z => z.RecordTimestamp == monthDate)).ToList();
        }

        private async Task<Dictionary<int, ProductionPlanDataDto>> GetProductionPlanDataAsync(DateTime monthDate, DateTime firstDayInMonth)
        {
            var productionPlanConfigs = await this.data.ProductionPlanConfigs.AllAnual(monthDate.Year)
                                        .Include(x => x.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers)
                                        .Include(x => x.ProductionPlanConfigUnitMonthlyConfigPlanMembers).ToArrayAsync();

            var productionPlanData = (from ppc in productionPlanConfigs
                                      join pd in data.ProductionPlanDatas.All()
                                                     .Where(x => x.RecordTimestamp == monthDate)
                                        on ppc.Id equals pd.ProductionPlanConfigId
                                      join pn in data.PlanNorms.AllWithDeleted()
                                                     .Where(x => x.Month == firstDayInMonth)
                                                     .Select(x => new { x.ProductionPlanConfigId, x.Value })
                                        on ppc.Id equals pn.ProductionPlanConfigId
                                      join pv in data.PlanValues.AllWithDeleted()
                                                     .Where(x => x.Month == firstDayInMonth)
                                                     .Select(x => new { x.ProcessUnitId, x.Value, x.ValueLiquid })
                                      on ppc.ProcessUnitId equals pv.ProcessUnitId
                                      select new ProductionPlanDataDto(
                                          productionPlanConfig: ppc,
                                          productionPlanData: pd,
                                          planNorm: pn.Value,
                                          planValue: pv.Value,
                                          planLiquid: pv.ValueLiquid))
                                      .ToDictionary(x => x.ProductionPlanConfig.Id);

            return productionPlanData;
        }

        private async Task<IEnumerable<MonthlyTechnicalReportDataDto>> CalculateMonthlyTechnologicalDataAsync(List<UnitMonthlyData> monthlyProductionDataList, Dictionary<int, ProductionPlanDataDto> productionPlanDatas, DateTime firstDayInMonth, Dictionary<string, MonthlyData> monthlyData)
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
                    ProductionPlanDataDto productionPlanData = null;
                    int productionPlanConfigId = item.UnitMonthlyConfig.ProductionPlanConfigId ?? 0;
                    if (productionPlanConfigId != 0)
                    {
                        productionPlanData = productionPlanDatas.FirstOrDefault(x => x.Key == productionPlanConfigId).Value;
                    }
                    var monthlyProductionDataRecord = await CreateMonthlyTechnicalReportRecord(item, productionPlanData, firstDayInMonth, monthlyData);
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

            var monthlyProductionDataResult = monthlyProductionData.Where(m => !processUnitsWithErrorInCalculation.Contains(m.ProcessUnitId));
            return monthlyProductionDataResult;
        }

        private async Task<MonthlyTechnicalReportDataDto> CreateMonthlyTechnicalReportRecord(UnitMonthlyData item, ProductionPlanDataDto productionPlanData,
            DateTime firstDayInMonth, Dictionary<string, MonthlyData> monthlyData)
        {
            var monthlyTechnicalReportDataDto = new MonthlyTechnicalReportDataDto();

            var planValue = await Task<double>.Run(() => GetPlanValue(item, productionPlanData, firstDayInMonth));
            var planPercentage = await Task<double>.Run(() => GetPlanPercentage(item, productionPlanData));
            var factPercentage = await Task<double>.Run(() => CalculateFactPercentage(item, productionPlanData, monthlyData));
            if (factPercentage == double.MinValue)
            {
                monthlyTechnicalReportDataDto = null;
            }

            if (monthlyTechnicalReportDataDto != null)
            {
                var yearPercentage = await Task<double>.Run(() => CalculateYearPercentage(item, productionPlanData, monthlyData));
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
                        PlanValue = (decimal) planValue,
                        PlanPercentage = (decimal) planPercentage,
                        FactValue = (decimal) item.RealValue,
                        FactPercentage = (decimal) factPercentage,
                        FactValueDifference = Math.Round((decimal) (item.RealValue - planValue), 5),
                        FactPercentageDifference = Math.Round((decimal) (factPercentage - planPercentage), 5),
                        YearValue = item.YearTotalValue + (decimal) item.RealValue,
                        YearPercentage = (decimal) yearPercentage,
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

        private double GetPlanValue(UnitMonthlyData monthlyData, ProductionPlanDataDto productionPlanData, DateTime targetMonth)
        {
            int materialType = productionPlanData?.ProductionPlanConfig?.MaterialTypeId ?? 0;
            if (monthlyData.UnitMonthlyConfig.ProductionPlanConfig == null || materialType == 0)
            {
                return 0.0;
            }

            double result;

            if (materialType == CommonConstants.MaterialType)
            {
                var inputDictionary = new Dictionary<string, double>();
                inputDictionary.Add("pn", (double) productionPlanData.PlanNorm);
                inputDictionary.Add("pv", (double) productionPlanData.PlanValue);
                inputDictionary.Add("pl", (double) ((productionPlanData.PlanLiquid == null) ? productionPlanData.PlanValue : productionPlanData.PlanLiquid.Value));
                var formula = "(p.pn/100.00)*p.pv";
                if (productionPlanData.ProductionPlanConfig.MonthlyValuePlanFormula != null)
                {
                    formula = productionPlanData.ProductionPlanConfig.MonthlyValuePlanFormula;
                }
                result = calculator.Calculate(formula, "p", inputDictionary.Count,
                                              inputDictionary, productionPlanData.ProductionPlanConfig.Code)
                                              .FilterNotANumberValues();
            }
            else
            {
                int daysInMonth = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);
                result = (double) productionPlanData.ProductionPlanData.QuanityPlan * daysInMonth;
            }

            return result;
        }

        private double GetPlanPercentage(UnitMonthlyData monthlyData, ProductionPlanDataDto productionPlanData)
        {
            if (monthlyData.UnitMonthlyConfig.ProductionPlanConfig == null)
            {
                return 0;
            }

            return (double) (productionPlanData?.PlanNorm ?? 0);
        }

        /// <summary>
        /// Calculates the fact percentage.
        /// </summary>
        /// <param name="monthlyData">The monthly data.</param>
        /// <param name="productionPlanData">The production plan data.</param>
        /// <param name="monthlyApprovedDatas">The monthly approved datas.</param>
        /// <returns></returns>
        private double CalculateFactPercentage(UnitMonthlyData monthlyData, ProductionPlanDataDto productionPlanData,
                                               Dictionary<string, MonthlyData> monthlyApprovedDatas)
        {
            //var productionPlanData = productionPlanDatas.Where(x => x.ProductionPlanConfig.Id == monthlyData.UnitMonthlyConfig.ProductionPlanConfigId).FirstOrDefault();

            if (productionPlanData == null)
            {
                return 0.0;
            }

            double result;
            if (!string.IsNullOrEmpty(productionPlanData.ProductionPlanConfig.MonthlyFactFractionFormula))
            {
                var inputDictionary = new Dictionary<string, double>();
                var relatedMonthlyDatas = productionPlanData.ProductionPlanConfig.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers;
                foreach (var relatedMonthlyData in relatedMonthlyDatas)
                {
                    var value = (double) monthlyApprovedDatas[relatedMonthlyData.UnitMonthlyConfig.Code].MonthValue;
                    inputDictionary.Add($"p{relatedMonthlyData.Position - 1}", value);
                }

                result = calculator.Calculate(productionPlanData.ProductionPlanConfig.MonthlyFactFractionFormula, "p",
                                              inputDictionary.Count, inputDictionary, productionPlanData.ProductionPlanConfig.Code)
                                              .FilterNotANumberValues();
            }
            else
            {
                result = double.MinValue;
            }

            return result;
        }

        /// <summary>
        /// Calculates the year percentage.
        /// </summary>
        /// <param name="monthlyData">The monthly data.</param>
        /// <param name="productionPlanData">The production plan data.</param>
        /// <param name="monthlyApprovedDatas">The monthly approved datas.</param>
        /// <returns></returns>
        private double CalculateYearPercentage(UnitMonthlyData monthlyData, ProductionPlanDataDto productionPlanData,
            Dictionary<string, MonthlyData> monthlyApprovedDatas)
        {
            double result = 0;
            if (productionPlanData != null)
            {
                if (!string.IsNullOrEmpty(productionPlanData?.ProductionPlanConfig?.MonthlyFactFractionFormula))
                {
                    var inputDictionary = new Dictionary<string, double>();
                    var relatedMonthlyDatas = productionPlanData.ProductionPlanConfig.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers;
                    foreach (var relatedMonthlyData in relatedMonthlyDatas)
                    {
                        var value = (double) monthlyApprovedDatas[relatedMonthlyData.UnitMonthlyConfig.Code].MonthValue
                                    + (double) monthlyApprovedDatas[relatedMonthlyData.UnitMonthlyConfig.Code].YearValue;
                        inputDictionary.Add(string.Format("p{0}", relatedMonthlyData.Position - 1), value);
                    }

                    result = calculator.Calculate(productionPlanData.ProductionPlanConfig.MonthlyFactFractionFormula, "p",
                            inputDictionary.Count, inputDictionary, productionPlanData.ProductionPlanConfig.Code)
                            .FilterNotANumberValues();
                }
                else
                {
                    result = double.MinValue;
                }
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        private class ProductionPlanDataDto
        {
            public ProductionPlanDataDto(ProductionPlanConfig productionPlanConfig,
                ProductionPlanData productionPlanData,
                decimal planNorm, decimal planValue, decimal? planLiquid)
            {
                this.ProductionPlanConfig = productionPlanConfig;
                this.ProductionPlanData = productionPlanData;
                this.PlanNorm = planNorm;
                this.PlanValue = planValue;
                this.PlanLiquid = planLiquid;
            }
            public ProductionPlanConfig ProductionPlanConfig { get; }
            public ProductionPlanData ProductionPlanData { get; }
            public decimal PlanNorm { get; }
            public decimal PlanValue { get; }
            public decimal? PlanLiquid { get; }
        }
    }



    class MonthlyData
    {
        public decimal MonthValue { get; set; }

        public decimal YearValue { get; set; }
    }
}
