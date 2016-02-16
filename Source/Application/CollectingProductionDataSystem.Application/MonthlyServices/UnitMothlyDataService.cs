﻿/// <summary>
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


    public class UnitMothlyDataService
    {

        private readonly IProductionData data;
        private readonly IKernel kernel;
        private readonly ICalculatorService calculator;

        public UnitMothlyDataService(IProductionData dataParam, IKernel kernelParam, ICalculatorService calculatorParam)
        {
            this.data = dataParam;
            this.kernel = kernelParam;
            this.calculator = calculatorParam;
        }

        public IEnumerable<UnitMonthlyData> CalculateMonthlyDataForReportType(DateTime inTargetMonth, bool isRecalculate, int ReportTypeId)
        {
            var date = inTargetMonth.Date;
            var targetMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
#if DEBUG
            targetMonth = new DateTime(date.Year, 2, 2);
#endif
            var targetUnitDailyRecordConfigs = data.UnitMonthlyConfigs.All()
                .Include(x => x.UnitDailyConfigUnitMonthlyConfigs)
                .Where(x => x.MonthlyReportTypeId == ReportTypeId).ToList();
            Dictionary<string, UnitMonthlyData> resultMonthly = new Dictionary<string, UnitMonthlyData>();
            Dictionary<string, UnitMonthlyData> wholeMonthResult = new Dictionary<string, UnitMonthlyData>();

            if (!isRecalculate)
            {
                resultMonthly = CalculateMonthlyDataFromDailyData(targetUnitDailyRecordConfigs, targetMonth);
            }
            //else
            //{
            //    wholeDayResult = GetDailyDataFromDailyData(targetDay, processUnitId);
            //    resultDaily = wholeDayResult.Where(x => x.Value.UnitsDailyConfig.AggregationCurrentLevel == false).ToDictionary(x => x.Key, x => x.Value);
            //}

            //Dictionary<string, UnitsDailyData> relatedRecords = GetRelatedData(processUnitId, targetDay, materialTypeId);
            //AppendRelatedRecords(relatedRecords, resultDaily);

            CalculateDailyDataFromRelatedDailyData(ref resultMonthly, targetMonth, isRecalculate, ReportTypeId);

            //ClearRelatedRecords(relatedRecords, resultDaily);

            //ClearUnModifiedRecords(resultDaily, wholeDayResult);

            //if (!isRecalculate)
            //{
            //    AppendTotalMonthQuantityToDailyRecords(resultDaily, processUnitId, targetDay);
            //}

            return resultMonthly.Select(x => x.Value);
        }

        /// <summary>
        /// Clears the un modified records.
        /// </summary>
        /// <param name="resultDaily">The result daily.</param>
        /// <param name="wholeDayResult">The whole day result.</param>
        private void ClearUnModifiedRecords(Dictionary<string, UnitsDailyData> resultDaily, Dictionary<string, UnitsDailyData> wholeDayResult)
        {
            foreach (var item in wholeDayResult)
            {
                if (resultDaily.ContainsKey(item.Key))
                {
                    if (item.Value.RealValue == resultDaily[item.Key].RealValue
                        && resultDaily[item.Key].UnitsManualDailyData != null
                        && item.Value.UnitsManualDailyData != null
                        && item.Value.UnitsManualDailyData.EditReasonId == resultDaily[item.Key].UnitsManualDailyData.EditReasonId)
                    {
                        resultDaily.Remove(item.Key);
                    }
                }
            }
        }

        private void ClearRelatedRecords(Dictionary<string, UnitsDailyData> relatedRecords, Dictionary<string, UnitsDailyData> resultDaily)
        {
            if (relatedRecords.Count() > 0)
            {
                foreach (var item in relatedRecords)
                {
                    resultDaily.Remove(item.Key);
                }
            }
        }

        private void AppendRelatedRecords(Dictionary<string, UnitsDailyData> relatedRecords, Dictionary<string, UnitsDailyData> resultDaily)
        {
            if (relatedRecords.Count() > 0)
            {
                foreach (var item in relatedRecords)
                {
                    if (!resultDaily.ContainsKey(item.Key))
                    {
                        resultDaily.Add(item.Key, item.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the daily data from shift data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfigs">The target unit daily record configs.</param>
        /// <param name="targetDay">The target day.</param>
        /// <param name="processUnitId">The process unit id.</param>
        /// <returns></returns>
        private Dictionary<string, UnitsDailyData> GetDailyDataFromDailyData(DateTime targetDay, int processUnitId)
        {
            return this.data.UnitsDailyDatas.All()
                        .Include(x => x.UnitsDailyConfig)
                        .Include(x => x.UnitsManualDailyData)
                        .Where(x => x.RecordTimestamp == targetDay
                                && x.UnitsDailyConfig.ProcessUnitId == processUnitId
                                )
                                .ToDictionary(x => x.UnitsDailyConfig.Code);
        }

        ///// <summary>
        ///// Appends the total month quantity to daily records.
        ///// </summary>
        ///// <param name="resultDaily">The result daily.</param>
        ///// <param name="processUnitId">The process unit id.</param>
        ///// <param name="targetDay">The target day.</param>
        //private void AppendTotalMonthQuantityToDailyRecords(Dictionary<string, UnitsDailyData> resultDaily, int processUnitId, DateTime targetDay)
        //{
        //    var beginningOfMonth = new DateTime(targetDay.Year, targetDay.Month, 1);
        //    var totalMonthQuantities = data.UnitsDailyDatas.All().Include(x => x.UnitsDailyConfig)
        //        .Join(data.UnitsApprovedDailyDatas.All(),
        //                units => new UnitDailyToApprove { ProcessUnitId = units.UnitsDailyConfig.ProcessUnitId, RecordDate = units.RecordTimestamp },
        //                appd => new UnitDailyToApprove { ProcessUnitId = appd.ProcessUnitId, RecordDate = appd.RecordDate },
        //                (units, appd) => new { Units = units, Appd = appd })
        //        .Where(x => x.Units.UnitsDailyConfig.ProcessUnitId == processUnitId &&
        //                !x.Units.UnitsDailyConfig.NotATotalizedPosition &&
        //                beginningOfMonth <= x.Units.RecordTimestamp &&
        //                x.Units.RecordTimestamp < targetDay &&
        //                x.Appd.Approved == true).GroupBy(x => x.Units.UnitsDailyConfig.Code).ToList()
        //                .Select(group => new { Code = group.Key, Value = group.Sum(x => x.Units.RealValue) }).ToDictionary(x => x.Code, x => x.Value);

        //    foreach (var item in resultDaily)
        //    {
        //        if (totalMonthQuantities.ContainsKey(item.Key))
        //        {
        //            resultDaily[item.Key].TotalMonthQuantity = (decimal)totalMonthQuantities[item.Key];
        //        }
        //        else
        //        {
        //            resultDaily[item.Key].TotalMonthQuantity = 0m;
        //        }
        //    }
        //    Debug.WriteLine("finish");
        //}

        /// <summary>
        /// Calculates the daily data from shift data.
        /// </summary>
        /// <param name="targetUnitMonthlyRecordConfigs">The target unit daily record configs.</param>
        /// <param name="targetDay">The target day.</param>
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

                var monthlyValue = GetMonthlyValue(unitDailyDatasByMontlyData, targetUnitMonthlyRecordConfig, positionDictionary);


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
        /// Gets the unit datas for daily data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfig">The target unit daily record config.</param>
        /// <param name="targetDay">The target day.</param>
        /// <returns></returns>
        private IEnumerable<UnitsDailyData> GetUnitDailyDatasForMonthlyData(UnitMonthlyConfig targetUnitMonthlyRecordConfig, IEnumerable<UnitsDailyData> targetUnitDailyData)
        {
            var targetUnitDailyCongifs = targetUnitMonthlyRecordConfig.UnitDailyConfigUnitMonthlyConfigs.Select(x => x.UnitDailyConfigId).ToList();
            return targetUnitDailyData.Where(x => targetUnitDailyCongifs.Any(y => y == x.UnitsDailyConfigId)).ToList();
        }

        /// <summary>
        /// Calculates the daily data from related daily data.
        /// </summary>
        /// <param name="targetRecords">The target records.</param>
        /// <param name="resultMonthly">The result daily.</param>
        /// <param name="targetDay">The target day.</param>
        private void CalculateDailyDataFromRelatedDailyData(ref Dictionary<string, UnitMonthlyData> resultMonthly, DateTime targetMonth, bool isRecalculate, int reportTypeId)
        {
            Dictionary<string, UnitMonthlyData> calculationResult = new Dictionary<string, UnitMonthlyData>();

            var targetRecords = data.UnitMonthlyConfigs.All()
                .Include(x => x.RelatedUnitMonthlyConfigs)
                .Where(x => x.AggregationCurrentLevel == true
                        && x.IsManualEntry == false
                        && x.MonthlyReportTypeId == reportTypeId).ToList();

            Dictionary<string, UnitMonthlyData> targetUnitMonthlyRecords = new Dictionary<string, UnitMonthlyData>();

            if (isRecalculate)
            {
                targetUnitMonthlyRecords = data.UnitMonthlyDatas.All()
                  .Include(x => x.UnitMonthlyConfig)
                  .Include(x => x.UnitManualMonthlyData)
                  .Where(x => x.RecordTimestamp == targetMonth).ToDictionary(x => x.UnitMonthlyConfig.Code);
            }

            foreach (var targetUnitMonthlyRecordConfig in targetRecords)
            {
                var positionDictionary = targetUnitMonthlyRecordConfig.RelatedUnitMonthlyConfigs
                                       .Select(x => new { Id = x.RelatedUnitMonthlyConfigId, Position = x.Position }).Distinct()
                                       .ToDictionary(x => x.Id, x => x.Position);

                var unitDailyDatasForPosition = GetUnitDatasForRelatedDailyData(targetUnitMonthlyRecordConfig, resultMonthly);
                double monthlyValue = GetMonthlyValue(unitDailyDatasForPosition, targetUnitMonthlyRecordConfig, positionDictionary);

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
        /// <param name="targetUnitDailyRecordConfig">The target unit daily record config.</param>
        /// <param name="resultDaily">The result daily.</param>
        /// <param name="targetDay">The target day.</param>
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
        /// Gets the daily value.
        /// </summary>
        /// <param name="unitDailyDatasForPosition">The unit daily datas for position.</param>
        /// <param name="targetUnitDailyRecordConfig">The target unit daily record config.</param>
        /// <returns></returns>
        private double GetMonthlyValue(IEnumerable<UnitMonthlyData> unitMonthlyDatasForPosition, UnitMonthlyConfig unitDailyConfig, Dictionary<int, int> positionDictionary)
        {
            var inputDictionary = new Dictionary<string, double>();

            foreach (var unit in unitMonthlyDatasForPosition)
            {
                int currentIndex = positionDictionary[unit.UnitMonthlyConfigId];
                inputDictionary.Add(string.Format("p{0}", currentIndex - 1), unit.RealValue);
            }

            return calculator.Calculate(unitDailyConfig.AggregationFormula, "p", inputDictionary.Count, inputDictionary);
        }

        /// <summary>
        /// Gets the shift value.
        /// </summary>
        /// <param name="shift">The shift.</param>
        /// <returns></returns>
        private double GetMonthlyValue(IEnumerable<UnitsDailyData> records, UnitMonthlyConfig unitMonthlyConfig, Dictionary<int, int> positionDictionary)
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

        /// <summary>
        /// Checks if all shifts are ready.
        /// </summary>
        /// <param name="unitDatas">The unit datas.</param>
        /// <returns></returns>
        public bool CheckIfAllShiftsAreReady(DateTime targetDate, int processUnitId)
        {
            var currentApprovedUnitDatasCount = this.data.UnitsApprovedDatas.All().Where(x => x.ProcessUnitId == processUnitId && x.RecordDate == targetDate).ToList();
            var shiftsCount = this.data.Shifts.All().ToList();

            if (currentApprovedUnitDatasCount.Count != shiftsCount.Count)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if day is approved.
        /// </summary>
        /// <param name="targetDate">The target date.</param>
        /// <param name="processUnitId">The process unit identifier.</param>
        /// <returns></returns>
        public bool CheckIfDayIsApproved(DateTime targetDate, int processUnitId)
        {
            return this.data.UnitsApprovedDailyDatas.All()
                .Where(x => x.RecordDate == targetDate && x.ProcessUnitId == processUnitId).Any();
        }

        /// <summary>
        /// Clears the unit daily datas.
        /// </summary>
        /// <param name="targetDate">The target date.</param>
        /// <param name="processUnitId">The process unit identifier.</param>
        /// <param name="userName">Name of the user.</param>
        public IEfStatus ClearUnitDailyDatas(DateTime targetDate, int processUnitId, string userName)
        {
            var unitDailyDatas = this.data.UnitsDailyDatas.All().Include(x => x.UnitsDailyConfig)
                .Where(y => y.RecordTimestamp == targetDate && y.UnitsDailyConfig.ProcessUnitId == processUnitId).ToList();

            foreach (var record in unitDailyDatas)
            {
                this.data.UnitsDailyDatas.Delete(record);
            }

            return this.data.SaveChanges(userName);
        }


        /// <summary>
        /// Checks the exists unit daily datas.
        /// </summary>
        /// <param name="targetDate">The target date.</param>
        /// <param name="processUnitId">The process unit identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool CheckExistsUnitDailyDatas(DateTime targetDate, int processUnitId, int materialTypeId)
        {
            var unitDailyDatas = this.data.UnitsDailyDatas.All().Include(x => x.UnitsDailyConfig)
                .Where(y => y.RecordTimestamp == targetDate
                    && y.UnitsDailyConfig.ProcessUnitId == processUnitId
                    && y.UnitsDailyConfig.MaterialTypeId == materialTypeId).Any();
            return unitDailyDatas;
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
        public IEfStatus CheckIfShiftsAreReady(DateTime targetDate, int processUnitId)
        {
            var currentApprovedUnitDatas = this.data.UnitsApprovedDatas.All().Where(x => x.ProcessUnitId == processUnitId && x.RecordDate == targetDate).ToList();
            var shifts = this.data.Shifts.All().ToList();

            var validationResults = new List<ValidationResult>();
            foreach (var shift in shifts)
            {
                if (!currentApprovedUnitDatas.Any(x => x.ShiftId == shift.Id))
                {
                    validationResults.Add(new ValidationResult(string.Format(Resources.ErrorMessages.ShiftNotReady, shift.Name)));
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

        //public ChartViewModel<DateTime, decimal> GetStatisticForProcessUnit(int processUnitId, DateTime targetDate, int? materialTypeId = null)
        //{
        //    var beginingOfTheMonth = new DateTime(targetDate.Year, targetDate.Month, 1);

        //    var statistic = this.data.ProductionPlanDatas.All().Include(x => x.ProductionPlanConfig)
        //                        .Where(x => beginingOfTheMonth <= x.RecordTimestamp
        //                                && x.RecordTimestamp < targetDate
        //                                && x.ProcessUnitId == processUnitId
        //                                && x.ProductionPlanConfig.MaterialTypeId == (materialTypeId ?? x.ProductionPlanConfig.MaterialTypeId))
        //                        .GroupBy(x => x.ProductionPlanConfigId).ToList();
        //    var charts = new List<DataSery<DateTime, decimal>>();

        //    foreach (var parameter in statistic)
        //    {
        //        if (parameter.Count() > 0)
        //        {
        //            IEnumerable<DataSery<DateTime, decimal>> series = GetSeriesFromData(parameter, beginingOfTheMonth, targetDate);
        //            charts.AddRange(series);
        //        }
        //    }

        //    return new ChartViewModel<DateTime, decimal>() { DataSeries = charts };
        //}

        ///// <summary>
        ///// Gets the series from data.
        ///// </summary>
        ///// <param name="parameter">The parameter.</param>
        ///// <param name="beginingOfTheMonth">The begining of the month.</param>
        ///// <param name="targetDate">The target date.</param>
        ///// <returns></returns>
        //private IEnumerable<DataSery<DateTime, decimal>> GetSeriesFromData(IEnumerable<ProductionPlanData> parameter, DateTime beginDate, DateTime endDate)
        //{
        //    var model = parameter.First();

        //    var paramTransformed = parameter.ToDictionary(x => x.RecordTimestamp);

        //    DataSery<DateTime, decimal> planPercent = new DataSery<DateTime, decimal>("area", string.Format("{0} план(%)", model.Name));
        //    DataSery<DateTime, decimal> factPercent = new DataSery<DateTime, decimal>("line", string.Format("{0} факт(%)", model.Name));
        //    DataSery<DateTime, decimal> plan = new DataSery<DateTime, decimal>("area", string.Format("{0} план(T)", model.Name));
        //    DataSery<DateTime, decimal> fact = new DataSery<DateTime, decimal>("line", string.Format("{0} факт(T)", model.Name));

        //    for (DateTime target = beginDate; target <= endDate; target = target.AddDays(1))
        //    {
        //        if (paramTransformed.ContainsKey(target.Date))
        //        {
        //            planPercent.Values.Add(new Pair<DateTime, decimal>(target.Date, paramTransformed[target.Date].PercentagesPlan));
        //            factPercent.Values.Add(new Pair<DateTime, decimal>(target.Date, paramTransformed[target.Date].PercentagesFact));
        //            plan.Values.Add(new Pair<DateTime, decimal>(target.Date, paramTransformed[target.Date].QuanityPlan));
        //            fact.Values.Add(new Pair<DateTime, decimal>(target.Date, paramTransformed[target.Date].QuantityFact));
        //        }
        //        else
        //        {
        //            planPercent.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
        //            factPercent.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
        //            plan.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
        //            fact.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
        //        }
        //    }

        //    return new List<DataSery<DateTime, decimal>>() { planPercent, factPercent, plan, fact };
        //}

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

        private class RecordsCollectionId
        {
            public DateTime RecordTimestamp { get; set; }
            public int Id { get; set; }
        }

    }
}
