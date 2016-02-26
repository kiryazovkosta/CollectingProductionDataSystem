/// <summary>
/// Summary description for UnitMothlyDataService
/// </summary>
namespace CollectingProductionDataSystem.Application.MonthlyServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using Ninject;
    using Resources = App_Resources;

    /// <summary>
    /// Summary description for UnitMothlyDataService
    /// </summary>
    public class UnitMothlyDataService : IUnitMothlyDataService
    {
        private readonly IProductionData data;
        private readonly IKernel kernel;
        private readonly ICalculatorService calculator;
        private readonly ITestUnitMonthlyCalculationService testUnitMonthlyCalculationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitMothlyDataService" /> class.
        /// </summary>
        /// <param name="dataParam">The data param.</param>
        /// <param name="kernelParam">The kernel param.</param>
        /// <param name="calculatorParam">The calculator param.</param>
        public UnitMothlyDataService(IProductionData dataParam, IKernel kernelParam, ICalculatorService calculatorParam, ITestUnitMonthlyCalculationService testUnitMonthlyCalculationServiceParam)
        {
            this.data = dataParam;
            this.kernel = kernelParam;
            this.calculator = calculatorParam;
            this.testUnitMonthlyCalculationService = testUnitMonthlyCalculationServiceParam;
        }

        /// <summary>
        /// Calculates the monthly data if not available.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="ReportTypeId"></param>
        /// <returns></returns>
        public IEfStatus CalculateMonthlyDataIfNotAvailable(DateTime date, int reportTypeId, string userName)
        {
            var targetMonth = this.GetTargetMonth(date);
            var status = this.CheckIfDailyAreReady(targetMonth, false);
            if (!status.IsValid)
            {
                return status;
            }

            if (status.IsValid
                && (!this.CheckIfMonthIsApproved(targetMonth, reportTypeId))
                && (!this.CheckExistsUnitMonthlyDatas(targetMonth, reportTypeId))
                && this.testUnitMonthlyCalculationService.TryBeginCalculation(new UnitMonthlyCalculationIndicator(targetMonth, reportTypeId)))
            {
                Exception exc = null;
                try
                {

                    var monthlyResult = this.CalculateMonthlyDataForReportType(targetMonth, false, reportTypeId);

                    if (monthlyResult.Count() > 0)
                    {
                        data.UnitMonthlyDatas.BulkInsert(monthlyResult, userName);
                        data.SaveChanges(userName);
                    }
                }
                catch (Exception ex)
                {
                    exc = ex;
                }
                finally
                {
                    int ix = 0;
                    while (!(this.testUnitMonthlyCalculationService.EndCalculation(new UnitMonthlyCalculationIndicator(targetMonth, reportTypeId)) || ix == 10))
                    {
                        ix++;
                    }

                    if (ix >= 10)
                    {
                        string message = string.Format("Cannot clear record for begun Process Unit Calculation For MonthlyReportType {0} and targetMonth {1}", reportTypeId, targetMonth);
                        exc = new InvalidOperationException();
                    }

                    if (exc != null)
                    {
                        throw exc;
                    }
                }
            }

            return status;
        }

        /// <summary>
        /// Calculates the type of the monthly data for report.
        /// </summary>
        /// <param name="inTargetMonth">The in target month.</param>
        /// <param name="isRecalculate">The is recalculate.</param>
        /// <param name="ReportTypeId">The report type id.</param>
        /// <returns></returns>
        public IEnumerable<UnitMonthlyData> CalculateMonthlyDataForReportType(DateTime inTargetMonth, bool isRecalculate, int reportTypeId)
        {
            DateTime targetMonth = GetTargetMonth(inTargetMonth);
            var targetUnitDailyRecordConfigs = data.UnitMonthlyConfigs.All()
                .Include(x => x.UnitDailyConfigUnitMonthlyConfigs)
                .Where(x => x.MonthlyReportTypeId == reportTypeId).ToList();
            Dictionary<string, UnitMonthlyData> resultMonthly = new Dictionary<string, UnitMonthlyData>();
            Dictionary<string, UnitMonthlyData> wholeMonthResult = new Dictionary<string, UnitMonthlyData>();

            if (!isRecalculate)
            {
                resultMonthly = CalculateMonthlyDataFromDailyData(targetUnitDailyRecordConfigs, targetMonth);
            }
            else
            {
                wholeMonthResult = GetMonthlyDataForMonth(targetMonth);
                resultMonthly = wholeMonthResult.Where(x =>
                    (x.Value.UnitMonthlyConfig.AggregationCurrentLevel == false) || (x.Value.UnitMonthlyConfig.IsManualEntry == true)).ToDictionary(x => x.Key, x => x.Value);
            }

            CalculateMonthlyDataFromRelatedMonthlyData(ref resultMonthly, targetMonth, isRecalculate, reportTypeId);
            ClearUnModifiedRecords(resultMonthly, wholeMonthResult);
            return resultMonthly.Select(x => x.Value);
        }

        /// <summary>
        /// Clears the un modified records.
        /// </summary>
        /// <param name="resultDaily">The result daily.</param>
        /// <param name="wholeMonthResult">The whole day result.</param>
        private void ClearUnModifiedRecords(Dictionary<string, UnitMonthlyData> resultDaily, Dictionary<string, UnitMonthlyData> wholeMonthResult)
        {
            foreach (var item in wholeMonthResult)
            {
                if (resultDaily.ContainsKey(item.Key))
                {
                    if (item.Value.RealValue == resultDaily[item.Key].RealValue
                        && resultDaily[item.Key].UnitManualMonthlyData != null
                        && item.Value.UnitManualMonthlyData != null)
                    {
                        resultDaily.Remove(item.Key);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the monthly data for month.
        /// </summary>
        /// <param name="targetMonth">The target month.</param>
        /// <returns></returns>
        private Dictionary<string, UnitMonthlyData> GetMonthlyDataForMonth(DateTime targetMonth, int changedRecordId = 0)
        {
            return this.data.UnitMonthlyDatas.All()
                        .Include(x => x.UnitMonthlyConfig)
                        .Include(x => x.UnitManualMonthlyData)
                        .Where(x => x.RecordTimestamp == targetMonth)
                                .ToDictionary(x => x.UnitMonthlyConfig.Code);
        }

        /// <summary>
        /// Calculates the monthly data from daily data.
        /// </summary>
        /// <param name="targetUnitMonthlyRecordConfigs">The target unit monthly record configs.</param>
        /// <param name="targetMonth">The target month.</param>
        /// <returns></returns>
        private Dictionary<string, UnitMonthlyData> CalculateMonthlyDataFromDailyData(IEnumerable<UnitMonthlyConfig> targetUnitMonthlyRecordConfigs, DateTime targetMonth)
        {
            var targetUnitDailyDatas = data.UnitsDailyDatas.All()
                  .Include(x => x.UnitsDailyConfig)
                  .Include(x => x.UnitsManualDailyData)
                  .Where(x => x.RecordTimestamp == targetMonth).ToList();

            Dictionary<string, UnitMonthlyData> result = new Dictionary<string, UnitMonthlyData>();

            foreach (var targetUnitMonthlyRecordConfig in targetUnitMonthlyRecordConfigs.Where(x => x.AggregationCurrentLevel == false && x.IsManualEntry == false))
            {
                var monthlyRecord = new UnitMonthlyData()
                {
                    RecordTimestamp = targetMonth,
                    UnitMonthlyConfigId = targetUnitMonthlyRecordConfig.Id
                };

                var positionDictionary = targetUnitMonthlyRecordConfig.UnitDailyConfigUnitMonthlyConfigs
                                        .Select(x => new { Id = x.UnitDailyConfigId, Position = x.Position }).Distinct()
                                        .ToDictionary(x => x.Id, x => x.Position);

                var unitDailyDatasByMontlyData = GetUnitDailyDatasForMonthlyData(targetUnitMonthlyRecordConfig, targetUnitDailyDatas);

                var monthlyValue = GetMonthlyValueFromRelatedDailyRecords(unitDailyDatasByMontlyData, targetUnitMonthlyRecordConfig, positionDictionary);


                monthlyRecord.Value = (decimal)monthlyValue;
                monthlyRecord.HasManualData = unitDailyDatasByMontlyData.Any(y => y.IsManual == true);
                result.Add(targetUnitMonthlyRecordConfig.Code, monthlyRecord);
            }

            // append manual records
            foreach (var targetUnitMonthlyRecordConfig in targetUnitMonthlyRecordConfigs.Where(x => x.IsManualEntry == true))
            {
                var monthlyRecord = new UnitMonthlyData()
                                           {
                                               RecordTimestamp = targetMonth,
                                               UnitMonthlyConfigId = targetUnitMonthlyRecordConfig.Id
                                           };

                result.Add(targetUnitMonthlyRecordConfig.Code, monthlyRecord);
            }
            return result;
        }

        /// <summary>
        /// Gets the unit daily datas for monthly data.
        /// </summary>
        /// <param name="targetUnitMonthlyRecordConfig">The target unit monthly record config.</param>
        /// <param name="targetUnitDailyData">The target unit daily data.</param>
        /// <returns></returns>
        private IEnumerable<UnitsDailyData> GetUnitDailyDatasForMonthlyData(UnitMonthlyConfig targetUnitMonthlyRecordConfig, IEnumerable<UnitsDailyData> targetUnitDailyData)
        {
            var targetUnitDailyCongifs = targetUnitMonthlyRecordConfig.UnitDailyConfigUnitMonthlyConfigs.Select(x => x.UnitDailyConfigId).ToList();
            return targetUnitDailyData.Where(x => targetUnitDailyCongifs.Any(y => y == x.UnitsDailyConfigId)).ToList();
        }

        /// <summary>
        /// Calculates the monthly data from related monthly data.
        /// </summary>
        /// <param name="resultMonthly">The result monthly.</param>
        /// <param name="targetMonth">The target month.</param>
        /// <param name="isRecalculate">The is recalculate.</param>
        /// <param name="reportTypeId">The report type id.</param>
        private void CalculateMonthlyDataFromRelatedMonthlyData(ref Dictionary<string, UnitMonthlyData> resultMonthly, DateTime targetMonth, bool isRecalculate, int reportTypeId)
        {
            Dictionary<string, UnitMonthlyData> calculationResult = new Dictionary<string, UnitMonthlyData>();

            var targetRecords = data.UnitMonthlyConfigs.All()
                .Include(x => x.RelatedUnitMonthlyConfigs)
                .Where(x => x.AggregationCurrentLevel == true
                        && x.IsManualEntry == false
                        && x.MonthlyReportTypeId == reportTypeId
                        //&& (changedRecordId == 0 || x.RelatedUnitMonthlyConfigs.Any(y => y.RelatedUnitMonthlyConfigId == changedRecordId))
                       ).ToList();

            Dictionary<string, UnitMonthlyData> targetUnitMonthlyRecords = new Dictionary<string, UnitMonthlyData>();

            if (isRecalculate)
            {
                targetUnitMonthlyRecords = data.UnitMonthlyDatas.All()
                  .Include(x => x.UnitMonthlyConfig)
                  .Include(x => x.UnitManualMonthlyData)
                  .Where(x => x.RecordTimestamp == targetMonth && x.UnitMonthlyConfig.MonthlyReportTypeId == reportTypeId).ToDictionary(x => x.UnitMonthlyConfig.Code);
            }

            foreach (var targetUnitMonthlyRecordConfig in targetRecords)
            {
                var positionDictionary = targetUnitMonthlyRecordConfig.RelatedUnitMonthlyConfigs
                                       .Select(x => new { Id = x.RelatedUnitMonthlyConfigId, Position = x.Position }).Distinct()
                                       .ToDictionary(x => x.Id, x => x.Position);

                var unitDailyDatasForPosition = GetUnitDatasForRelatedDailyData(targetUnitMonthlyRecordConfig, resultMonthly);
                double monthlyValue = GetMonthlyValueFromRelatedMounthlyRecords(unitDailyDatasForPosition, targetUnitMonthlyRecordConfig, positionDictionary);

                var monthlyRecord = new UnitMonthlyData()
                {
                    Id = (isRecalculate && targetUnitMonthlyRecords.ContainsKey(targetUnitMonthlyRecordConfig.Code)) ? targetUnitMonthlyRecords[targetUnitMonthlyRecordConfig.Code].Id : 0,
                    RecordTimestamp = targetMonth,
                    UnitMonthlyConfigId = targetUnitMonthlyRecordConfig.Id,
                    UnitManualMonthlyData = (isRecalculate && targetUnitMonthlyRecords.ContainsKey(targetUnitMonthlyRecordConfig.Code)) ? targetUnitMonthlyRecords[targetUnitMonthlyRecordConfig.Code].UnitManualMonthlyData : null
                };

                if (isRecalculate)
                {
                    if (monthlyRecord.UnitManualMonthlyData == null)
                    {
                        monthlyRecord.UnitManualMonthlyData = new UnitManualMonthlyData
                        {
                            Id = monthlyRecord.Id,
                            Value = (decimal)monthlyValue,
                        };
                    }
                    else
                    {
                        monthlyRecord.UnitManualMonthlyData.Value = (decimal)monthlyValue;
                    }
                }
                else
                {
                    monthlyRecord.Value = (decimal)monthlyValue;
                }

                resultMonthly.Add(targetUnitMonthlyRecordConfig.Code, monthlyRecord);
                if (isRecalculate)
                {
                    calculationResult.Add(targetUnitMonthlyRecordConfig.Code, monthlyRecord);
                }
            }

            if (isRecalculate)
            {
                resultMonthly = calculationResult;
            }
        }

        /// <summary>
        /// Gets the unit datas for related daily data.
        /// </summary>
        /// <param name="targetUnitMonRecordConfig">The target unit mon record config.</param>
        /// <param name="resultDaily">The result daily.</param>
        /// <returns></returns>
        private IEnumerable<UnitMonthlyData> GetUnitDatasForRelatedDailyData(UnitMonthlyConfig targetUnitMonRecordConfig, Dictionary<string, UnitMonthlyData> resultDaily)
        {
            var targetCodes = targetUnitMonRecordConfig.RelatedUnitMonthlyConfigs.OrderBy(x => x.Position).Select(x => x.RelatedUnitMonthlyConfig.Code);
            var result = new Dictionary<string, UnitMonthlyData>();

            foreach (var code in targetCodes)
            {
                result.Add(code, resultDaily[code]);
            }

            return result.Select(x => x.Value);
        }


        /// <summary>
        /// Gets the monthly value from related mounthly records.
        /// </summary>
        /// <param name="unitMonthlyDatasForPosition">The unit monthly datas for position.</param>
        /// <param name="unitMonthlyConfig">The unit monthly config.</param>
        /// <param name="positionDictionary">The position dictionary.</param>
        /// <returns></returns>
        private double GetMonthlyValueFromRelatedMounthlyRecords(IEnumerable<UnitMonthlyData> unitMonthlyDatasForPosition, UnitMonthlyConfig unitMonthlyConfig, Dictionary<int, int> positionDictionary)
        {
            var inputDictionary = new Dictionary<string, double>();

            foreach (var unit in unitMonthlyDatasForPosition)
            {
                int currentIndex = positionDictionary[unit.UnitMonthlyConfigId];
                inputDictionary.Add(string.Format("p{0}", currentIndex - 1), unit.RealValue);
            }

            return calculator.Calculate(unitMonthlyConfig.AggregationFormula, "p", inputDictionary.Count, inputDictionary);
        }

        /// <summary>
        /// Gets the monthly value.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <param name="unitMonthlyConfig">The unit monthly config.</param>
        /// <param name="positionDictionary">The position dictionary.</param>
        /// <returns></returns>
        private double GetMonthlyValueFromRelatedDailyRecords(IEnumerable<UnitsDailyData> records, UnitMonthlyConfig unitMonthlyConfig, Dictionary<int, int> positionDictionary)
        {
            // single record reference to single DailyRecord
            if (records.Count() == 1 && string.IsNullOrEmpty(unitMonthlyConfig.AggregationFormula))
            {
                var record = records.FirstOrDefault();
                if (record != null)
                {
                    return record.RealValueTillDay;
                }
                else
                {
                    return 0; // TODO See this may cause error
                }

            }

            var inputDictionary = new Dictionary<string, double>();
            foreach (var record in records)
            {
                int currentIndex = positionDictionary[record.UnitsDailyConfigId];
                inputDictionary.Add(string.Format("p{0}", currentIndex - 1), record.RealValueTillDay);
            }

            return calculator.Calculate(unitMonthlyConfig.AggregationFormula, "p", inputDictionary.Count, inputDictionary);
        }

        public DateTime GetTargetMonth(DateTime inTargetMonth)
        {
            DateTime date = inTargetMonth.Date;
            DateTime targetMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
//#if DEBUG
            targetMonth = new DateTime(inTargetMonth.Year, 2, 2);
//#endif
            return targetMonth;
        }

        /// <summary>
        /// Checks if day is approved.
        /// </summary>
        /// <param name="targetDate">The target date.</param>
        /// <param name="processUnitId">The process unit identifier.</param>
        /// <returns></returns>
        public bool CheckIfMonthIsApproved(DateTime targetDate, int monthlyReportTypeId)
        {
            var targetMonth = this.GetTargetMonth(targetDate);
            return this.data.UnitApprovedMonthlyDatas.All()
                .Where(x => x.RecordDate == targetMonth && x.MonthlyReportTypeId == monthlyReportTypeId).Any();
        }

        /// <summary>
        /// Checks existing of the unit monthly data.
        /// </summary>
        /// <param name="targetDate">The target date.</param>
        /// <param name="monthlyReportTypeId"></param>
        /// <returns></returns>
        public bool CheckExistsUnitMonthlyDatas(DateTime targetDate, int monthlyReportTypeId)
        {
            var targetMonth = this.GetTargetMonth(targetDate);
            var unitMonthlyDatas = this.data.UnitMonthlyDatas.All().Include(x => x.UnitMonthlyConfig)
                .Where(y => y.RecordTimestamp == targetMonth
                    && y.UnitMonthlyConfig.MonthlyReportTypeId == monthlyReportTypeId).Any();
            return unitMonthlyDatas;
        }

        /// <summary>
        /// Checks if previous days are ready.
        /// </summary>
        /// <param name="processUnitId">The process unit identifier.</param>
        /// <param name="targetDate">The target date.</param>
        /// <returns></returns>
        public IEfStatus CheckIfPreviousDaysAreReady(int processUnitId, DateTime targetDate, int materialTypeId)
        {
            var status = kernel.Get<IEfStatus>();
            if (targetDate.Day > 1)
            {
                var beginingOfTheMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
                var endOfObservedPeriod = targetDate.Date.AddDays(-1);

                var approvedDays = data.UnitsApprovedDailyDatas.All()
                                        .Where(x => x.ProcessUnitId == processUnitId
                                                && beginingOfTheMonth <= x.RecordDate
                                                && x.RecordDate <= endOfObservedPeriod
                                                && x.Approved == true).ToDictionary(x => x.RecordDate);

                int day = 0;
                var validationMaterialResults = new List<ValidationResult>() { new ValidationResult(@Resources.ErrorMessages.PreviousDaysMaterialConfirmationError) };
                var validationEnergyResults = new List<ValidationResult>() { new ValidationResult(@Resources.ErrorMessages.PreviousDaysEnergyConfirmationError) };

                while (day < endOfObservedPeriod.Day)
                {
                    var checkedDay = beginingOfTheMonth.AddDays(day);
                    if (!approvedDays.ContainsKey(checkedDay))
                    {
                        validationMaterialResults.Add(new ValidationResult(string.Format("\t\t- {0:dd.MM.yyyy г.}", checkedDay)));
                    }
                    else if ((!approvedDays[checkedDay].EnergyApproved)
                        && materialTypeId == CommonConstants.EnergyType
                        && (!Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ComplitionEnergyCheckDeactivared"])))
                    {
                        validationEnergyResults.Add(new ValidationResult(string.Format("\t\t- {0:dd.MM.yyyy г.}", checkedDay)));
                    }

                    day += 1;
                }

                var validationResults = new List<ValidationResult>();

                if (validationMaterialResults.Count > 1)
                {
                    validationMaterialResults.Add(new ValidationResult(string.Empty));

                    validationResults.AddRange(validationMaterialResults);
                }

                if (validationEnergyResults.Count > 1)
                {
                    validationResults.AddRange(validationEnergyResults);
                }

                if (validationResults.Count > 0)
                {
                    status.SetErrors(validationResults);
                }
            }

            return status;
        }

        /// <summary>
        /// Checks if shifts are ready.
        /// </summary>
        /// <param name="targetDate">The target date.</param>
        /// <param name="processUnitId">The process unit identifier.</param>
        /// <returns></returns>
        public IEfStatus CheckIfDailyAreReady(DateTime targetDate, bool IsEnergy)
        {
            var targetMonth = this.GetTargetMonth(targetDate);
            var currentApprovedDailyDatas = this.data.UnitsApprovedDailyDatas.All().Where(x => x.RecordDate == targetMonth && (!IsEnergy || x.EnergyApproved)).ToDictionary(x => x.ProcessUnitId, x => x);
            var processUnits = this.data.ProcessUnits.All();

            var validationResults = new List<ValidationResult>();
            foreach (var processUnit in processUnits)
            {
                if (!currentApprovedDailyDatas.ContainsKey(processUnit.Id))
                {
                    validationResults.Add(new ValidationResult(string.Format(Resources.ErrorMessages.DailyNotReady, targetMonth.ToString("dd.MM.yyyy г."), processUnit.ShortName)));
                }
            }

            var status = kernel.Get<IEfStatus>();

            if (validationResults.Count > 0)
            {
                status.SetErrors(validationResults);
            }

            return status;
        }


        private Dictionary<string, UnitsDailyData> GetRelatedData(int processUnitId, DateTime targetDay, int materialTypeId)
        {
            var relatedUnitsDailyData = new Dictionary<string, UnitsDailyData>();

            var relatedDailyDatasFromOtherProcessUnits = this.data.UnitsDailyConfigs
                .All()
                .Include(x => x.ProcessUnit)
                .Include(x => x.RelatedUnitDailyConfigs)
                .Where(x => x.ProcessUnitId == processUnitId && x.AggregationCurrentLevel == true)
                .SelectMany(y => y.RelatedUnitDailyConfigs);
            if (materialTypeId == CommonConstants.MaterialType)
            {
                relatedDailyDatasFromOtherProcessUnits = relatedDailyDatasFromOtherProcessUnits
                    .Where(z => z.RelatedUnitsDailyConfig.ProcessUnitId != processUnitId);
            }

            var relatedDailyDatasFromOtherProcessUnitsList = relatedDailyDatasFromOtherProcessUnits.ToList();

            foreach (var item in relatedDailyDatasFromOtherProcessUnitsList)
            {
                var relatedData = this.data.UnitsDailyDatas.All().Where(u => u.RecordTimestamp == targetDay && u.UnitsDailyConfigId == item.RelatedUnitsDailyConfigId).FirstOrDefault();
                if (relatedData != null && !relatedUnitsDailyData.ContainsKey(relatedData.UnitsDailyConfig.Code))
                {
                    relatedUnitsDailyData.Add(relatedData.UnitsDailyConfig.Code, relatedData);
                }
            }

            return relatedUnitsDailyData;
        }

        public IEfStatus CheckIfDayIsApprovedButEnergyNot(DateTime date, int processUnitId, out bool readyForCalculation)
        {
            var status = kernel.Get<IEfStatus>();
            readyForCalculation = false;
            var approvedRecord = this.data.UnitsApprovedDailyDatas.All().Where(x => x.RecordDate == date && x.ProcessUnitId == processUnitId).FirstOrDefault();

            if (approvedRecord == null)
            {
                status.SetErrors(new List<ValidationResult> { new ValidationResult(Resources.ErrorMessages.UnitDailyNotApproved) });
                return status;
            }

            if (!approvedRecord.EnergyApproved)
            {
                readyForCalculation = true;
            }

            return status;
        }

        public IEnumerable<UnitMonthlyData> GetDataForMonth(DateTime inTargetDate, int monthlyReportTypeId)
        {
            var targetMonth = this.GetTargetMonth(inTargetDate);
            return data.UnitMonthlyDatas.All()
                                .Include(x => x.UnitMonthlyConfig)
                                .Include(x => x.UnitMonthlyConfig.MeasureUnit)
                                .Include(x => x.UnitMonthlyConfig.ProcessUnit)
                                .Include(x => x.UnitMonthlyConfig.ProductType)
                                .Include(x => x.UnitMonthlyConfig.ProcessUnit.Factory)
                                .Include(x => x.UnitMonthlyConfig.MeasureUnit)
                                .Include(x => x.UnitManualMonthlyData)
                                .Where(x => x.RecordTimestamp == targetMonth
                                        && x.UnitMonthlyConfig.MonthlyReportTypeId == monthlyReportTypeId)
                                        .ToList();
        }

        public bool IsMonthlyReportConfirmed(DateTime inTargetDate, int monthlyReportTypeId)
        {
            var targetMonth = this.GetTargetMonth(inTargetDate);
            return this.data.UnitApprovedMonthlyDatas.All().Where(u => u.RecordDate == targetMonth && u.MonthlyReportTypeId == monthlyReportTypeId).Any();
        }
    }
}
