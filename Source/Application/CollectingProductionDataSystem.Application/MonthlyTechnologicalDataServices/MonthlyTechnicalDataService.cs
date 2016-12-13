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
    using log4net;
    using System.Globalization;
    using Models.Productions.Technological;

    public class MonthlyTechnicalDataService : IMonthlyTechnicalDataService
    {
        private readonly IProductionData data;
        private readonly IKernel kernel;
        private readonly ICalculatorService calculator;
        private readonly ITestMonthlyTechnologicalReportCalculationService testCalulation;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyTechnicalDataService" /> class.
        /// Dependency Injection Constructor
        /// </summary>
        /// <param name="dataParam">The data parameter.</param>
        /// <param name="kernelParam">The kernel parameter.</param>
        /// <param name="calculatorParam">The calculator parameter.</param>
        public MonthlyTechnicalDataService(
            IProductionData dataParam, 
            IKernel kernelParam, 
            ICalculatorService calculatorParam,
            ITestMonthlyTechnologicalReportCalculationService testCalculatorParam)
        {
            this.data = dataParam;
            this.kernel = kernelParam;
            this.calculator = calculatorParam;
            this.testCalulation = testCalculatorParam;
        }

        /// <summary>
        /// Reads the monthly technological data asynchronous.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="processUnits">The process units.</param>
        /// <returns></returns>
        public IEnumerable<MonthlyTechnicalReportDataDto> ReadMonthlyTechnologicalDataAsync(DateTime month, int[] processUnits)
        {
            var monthDate = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month), hour: 0, minute: 0, second: 0);
            var firstDayInMonth = new DateTime(month.Year, month.Month, day: 1, hour: 0, minute: 0, second: 0);

            bool exsistingDataForTheMonth = this.data.UnitTechnologicalMonthlyDatas.All().Where(x => x.RecordTimestamp == monthDate).Any();
            if (!exsistingDataForTheMonth
                && this.testCalulation.TryBeginCalculation(new MonthlyTechnologicalReportCalculationIndicator(monthDate)))
            {
                Exception exc = null;
                try
                {
                    GenerateTechnologicalData(monthDate, firstDayInMonth);
                }
                catch (Exception ex)
                {
                    exc = ex;
                }
                finally
                {
                    int ix = 0;
                    while (!(this.testCalulation.EndCalculation(new MonthlyTechnologicalReportCalculationIndicator(monthDate)) || ix == 10))
                    {
                        ix++;
                    }

                    if (ix >= 10)
                    {
                        string message = $"Cannot clear record for begun Monthly Technological Report Calculation For {monthDate}";
                        exc = new InvalidOperationException();
                    }

                    if (exc != null)
                    {
                        throw exc;
                    }
                }
            }

            IEnumerable<MonthlyTechnicalReportDataDto> result = ReadMonthlyTechnologicalData(monthDate, processUnits);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="processUnits"></param>
        /// <returns></returns>
        private IEnumerable<MonthlyTechnicalReportDataDto> ReadMonthlyTechnologicalData(DateTime month, int[] processUnits)
        {
            List<UnitTechnologicalMonthlyData> dbResult = this.data.UnitTechnologicalMonthlyDatas.All().Where(x => x.RecordTimestamp == month).Where(x => processUnits.Contains(x.ProcessUnitId)).ToList();
            var result = new List<MonthlyTechnicalReportDataDto>();
            foreach (UnitTechnologicalMonthlyData record in dbResult)
            {
                result.Add(new MonthlyTechnicalReportDataDto
                {
                    Id = record.UnitMonthlyConfigId,
                    Code = record.Code,
                    Name = record.Name,
                    Factory = record.Factory,
                    FactoryId = record.FactoryId,
                    ProcessUnit = record.ProcessUnit,
                    ProcessUnitId = record.ProcessUnitId,
                    MaterialType = record.MaterialType,
                    MeasurementUnit = record.MeasurementUnit,
                    DetailedMaterialType = record.DetailedMaterialType,
                    PlanValue = record.PlanValue,
                    PlanPercentage = record.PlanPercentage,
                    FactValue = record.FactValue,
                    FactPercentage = record.FactPercentage,
                    FactValueDifference = record.FactValueDifference,
                    FactPercentageDifference = record.FactPercentageDifference,
                    YearValue = record.YearValue,
                    YearPercentage = record.YearPercentage,
                    YearValueDifference = record.YearValueDifference,
                    YearPercentageDifference = record.YearPercentageDifference
                });
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthDate"></param>
        /// <param name="firstDayInMonth"></param>
        private void GenerateTechnologicalData(DateTime monthDate, DateTime firstDayInMonth)
        {
            int[] processUnitsIds = this.data.ProcessUnits.All().Select(x => x.Id).ToArray();
            Dictionary<int, ProductionPlanDataDto> productionPlanData = GetProductionPlanDataAsync(monthDate, firstDayInMonth);
            List<UnitMonthlyData> monthlyProductionDataList = GetMonthlyProductDataListAsync(processUnitsIds, monthDate);
            Dictionary<string, MonthlyData> monthlyData = GetMonthlyDataAsync(monthDate);
            IEnumerable<MonthlyTechnicalReportDataDto> technologicalData = CalculateMonthlyTechnologicalDataAsync(monthlyProductionDataList, productionPlanData, firstDayInMonth, monthlyData);
            var result = new List<UnitTechnologicalMonthlyData>();
            foreach (MonthlyTechnicalReportDataDto record in technologicalData)
            {
                result.Add(new UnitTechnologicalMonthlyData
                {
                    UnitMonthlyConfigId = record.Id,
                    RecordTimestamp = monthDate,
                    Code = record.Code,
                    Name = record.Name,
                    Factory = record.Factory,
                    FactoryId = record.FactoryId,
                    ProcessUnit = record.ProcessUnit,
                    ProcessUnitId = record.ProcessUnitId,
                    MaterialType = record.MaterialType,
                    MeasurementUnit = record.MeasurementUnit,
                    DetailedMaterialType = record.DetailedMaterialType,
                    PlanValue = record.PlanValue,
                    PlanPercentage = record.PlanPercentage,
                    FactValue = record.FactValue,
                    FactPercentage = record.FactPercentage,
                    FactValueDifference = record.FactValueDifference,
                    FactPercentageDifference = record.FactPercentageDifference,
                    YearValue = record.YearValue,
                    YearPercentage = record.YearPercentage,
                    YearValueDifference = record.YearValueDifference,
                    YearPercentageDifference = record.YearPercentageDifference
                });
            }

            this.data.UnitTechnologicalMonthlyDatas.BulkInsert(result, userName: "Initial Loading");
        }

        /// <summary>
        /// Gets the monthly data asynchronous.
        /// </summary>
        /// <param name="monthDate">The month date.</param>
        /// <returns></returns>
        private Dictionary<string, MonthlyData> GetMonthlyDataAsync(DateTime monthDate)
        {
            // ToDo: Re-factor this query to simplify result set
            return data.UnitMonthlyDatas.All()
                                       .Include(x => x.UnitMonthlyConfig)
                                       .Include(x => x.UnitManualMonthlyData)
                                       .Where(x => x.RecordTimestamp == monthDate)
                                       .ToDictionary(x => x.UnitMonthlyConfig.Code,
                                       y => new MonthlyData
                                       {
                                           MonthValue = (decimal) y.RealValue,
                                           YearValue = y.YearTotalValue
                                       });
        }

        /// <summary>
        /// Gets the monthly product data list asynchronous.
        /// </summary>
        /// <param name="processUnits">The process units.</param>
        /// <param name="monthDate">The month date.</param>
        /// <returns></returns>
        private List<UnitMonthlyData> GetMonthlyProductDataListAsync(int[] processUnits, DateTime monthDate)
        {
            var dbResult = data.UnitMonthlyConfigs
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
                       .ToList();

            return dbResult.SelectMany(y => y.UnitMonthlyDatas.Where(z => z.RecordTimestamp == monthDate)).ToList();
        }

        /// <summary>
        /// Gets the production plan data asynchronous.
        /// </summary>
        /// <param name="monthDate">The month date.</param>
        /// <param name="firstDayInMonth">The first day in month.</param>
        /// <returns></returns>
        private Dictionary<int, ProductionPlanDataDto> GetProductionPlanDataAsync(DateTime monthDate, DateTime firstDayInMonth)
        {
            var productionPlanConfigs = this.data.ProductionPlanConfigs.AllAnual(monthDate.Year)
                                        .Include(x => x.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers)
                                        .Include(x => x.ProductionPlanConfigUnitMonthlyConfigPlanMembers).ToArray();

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

        /// <summary>
        /// Calculates the monthly technological data asynchronous.
        /// </summary>
        /// <param name="monthlyProductionDataList">The monthly production data list.</param>
        /// <param name="productionPlanDatas">The production plan datas.</param>
        /// <param name="firstDayInMonth">The first day in month.</param>
        /// <param name="monthlyData">The monthly data.</param>
        /// <returns></returns>
        private IEnumerable<MonthlyTechnicalReportDataDto> CalculateMonthlyTechnologicalDataAsync(List<UnitMonthlyData> monthlyProductionDataList, Dictionary<int, ProductionPlanDataDto> productionPlanDatas, DateTime firstDayInMonth, Dictionary<string, MonthlyData> monthlyData)
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
                    var monthlyProductionDataRecord = CreateMonthlyTechnicalReportRecordAsync(item, productionPlanData, firstDayInMonth, monthlyData);
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

        /// <summary>
        /// Creates the monthly technical report record asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="productionPlanData">The production plan data.</param>
        /// <param name="firstDayInMonth">The first day in month.</param>
        /// <param name="monthlyData">The monthly data.</param>
        /// <returns></returns>
        private MonthlyTechnicalReportDataDto CreateMonthlyTechnicalReportRecordAsync(UnitMonthlyData item, ProductionPlanDataDto productionPlanData,
            DateTime firstDayInMonth, Dictionary<string, MonthlyData> monthlyData)
        {
            var monthlyTechnicalReportDataDto = new MonthlyTechnicalReportDataDto();

            var planValue = GetPlanValue(item, productionPlanData, firstDayInMonth, monthlyData);
            var planPercentage = GetPlanPercentage(item, productionPlanData);
            var factPercentage = CalculateFactPercentage(item, productionPlanData, monthlyData);
            if (factPercentage == double.MinValue)
            {
                monthlyTechnicalReportDataDto = null;
            }

            if (monthlyTechnicalReportDataDto != null)
            {
                var yearPercentage = CalculateYearPercentage(item, productionPlanData, monthlyData);
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
                        FactValueDifference = item.UnitMonthlyConfig.IsOnlyMonthFactValuePosition ? 0 : Math.Round((decimal) (item.RealValue - planValue), 5),
                        FactPercentageDifference = item.UnitMonthlyConfig.IsOnlyMonthFactValuePosition ? 0 : Math.Round((decimal) (factPercentage - planPercentage), 5),

                        YearValue = item.UnitMonthlyConfig.IsOnlyMonthFactValuePosition ? 0 : item.YearTotalValue + (decimal) item.RealValue,
                        YearPercentage = (decimal) yearPercentage,
                        YearValueDifference = 0,
                        YearPercentageDifference = 0
                    };
                }
            }

            return monthlyTechnicalReportDataDto;
        }

        /// <summary>
        /// Gets the type of the detailed material.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private string GetDetailedMaterialType(UnitMonthlyData item)
        {
            if (item.UnitMonthlyConfig.MonthlyReportTypeId == CommonConstants.HydroCarbons)
            {
                return $"{item.UnitMonthlyConfig.ProductTypeId:00}.{item.UnitMonthlyConfig.ProductType.Name}";
            }
            else
            {
                return $"{item.UnitMonthlyConfig.MonthlyReportTypeId:00}.{item.UnitMonthlyConfig.MonthlyReportType.Name}";
            }
        }

        /// <summary>
        /// Gets the type of the material.
        /// </summary>
        /// <param name="materialTypeId">The material type id.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if all month report are approved.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the plan value.
        /// </summary>
        /// <param name="monthlyData">The monthly data.</param>
        /// <param name="productionPlanData">The production plan data.</param>
        /// <param name="targetMonth">The target month.</param>
        /// <returns></returns>
        private double GetPlanValue(UnitMonthlyData monthlyData, ProductionPlanDataDto productionPlanData, DateTime targetMonth, Dictionary<string, MonthlyData> monthlyDatas)
        {
            int materialType = monthlyData.UnitMonthlyConfig?.ProductionPlanConfig?.MaterialTypeId ?? 0;
            if (monthlyData.UnitMonthlyConfig.ProductionPlanConfig == null || materialType == 0 || monthlyData.UnitMonthlyConfig.IsOnlyMonthFactValuePosition)
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
                if (productionPlanData.ProductionPlanConfig.MonthlyValuePlanFormula != null)
                {
                    var formula = productionPlanData.ProductionPlanConfig.MonthlyValuePlanFormula;
                    var inputDictionary = new Dictionary<string, double>();
                    inputDictionary.Add("pn", (double) productionPlanData.PlanNorm);
                    inputDictionary.Add("pv", (double) productionPlanData.PlanValue);
                    inputDictionary.Add("pl", (double) ((productionPlanData.PlanLiquid == null) ? productionPlanData.PlanValue : productionPlanData.PlanLiquid.Value));
                    var relatedPositions = productionPlanData.ProductionPlanConfig.ProductionPlanConfigUnitMonthlyConfigPlanMembers.ToList();
                    foreach (var item in relatedPositions)
                    {
                        inputDictionary.Add(string.Format("p{0}", item.Position - 1), (double)(monthlyDatas[item.UnitMonthlyConfig.Code].MonthValue));
                    }
                    result = calculator.Calculate(formula, "p", inputDictionary.Count, inputDictionary, productionPlanData.ProductionPlanConfig.Code).FilterNotANumberValues();
                }
                else
                {
                    int daysInMonth = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);
                    result = (double) productionPlanData.ProductionPlanData.QuanityPlan * daysInMonth;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the plan percentage.
        /// </summary>
        /// <param name="monthlyData">The monthly data.</param>
        /// <param name="productionPlanData">The production plan data.</param>
        /// <returns></returns>
        private double GetPlanPercentage(UnitMonthlyData monthlyData, ProductionPlanDataDto productionPlanData)
        {
            if (monthlyData.UnitMonthlyConfig.ProductionPlanConfig == null || monthlyData.UnitMonthlyConfig.IsOnlyMonthFactValuePosition)
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

            if (productionPlanData == null || monthlyData.UnitMonthlyConfig.IsOnlyMonthFactValuePosition)
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
            if (productionPlanData != null && 
                !monthlyData.UnitMonthlyConfig.IsOnlyMonthFactValuePosition)
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
