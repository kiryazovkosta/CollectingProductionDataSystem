﻿/// <summary>
/// Summary description for UnitDailyDataService
/// </summary>
namespace CollectingProductionDataSystem.Application.UnitDailyDataServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Constants;
    using Data.Contracts;
    using Infrastructure.Chart;
    using Infrastructure.Extentions;
    using Models.Productions;
    using Ninject;
    using Resources = App_Resources;

    public class UnitDailyDataService : IUnitDailyDataService
    {
        private readonly IProductionData data;
        private readonly IKernel kernel;
        private readonly ICalculatorService calculator;

        public UnitDailyDataService(IProductionData dataParam, IKernel kernelParam, ICalculatorService calculatorParam)
        {
            this.data = dataParam;
            this.kernel = kernelParam;
            this.calculator = calculatorParam;
        }

        public IEnumerable<UnitsDailyData> CalculateDailyDataForProcessUnit(int processUnitId, DateTime targetDay, bool isRecalculate = false, int editReasonId = 0, int materialTypeId = 0)
        {
            if (processUnitId < 1)
            {
                string message = App_Resources.ErrorMessages.InvalidParameter;
                throw new ArgumentException(string.Format(message, "processUnitId", 1, int.MaxValue));
            }

            if (materialTypeId < 1)
            {
                string message = App_Resources.ErrorMessages.InvalidParameter;
                throw new ArgumentException(string.Format(message, "materialTypeId", 1, int.MaxValue));
            }

            // Month is ready and calculations begin
            var targetUnitDailyRecordConfigs = this.data.UnitsDailyConfigs.All()
                .Include(x => x.UnitConfigUnitDailyConfigs)
                .Where(x => x.ProcessUnitId == processUnitId
                            && x.MaterialTypeId == materialTypeId).ToList();
            Dictionary<string, UnitsDailyData> resultDaily = new Dictionary<string, UnitsDailyData>();
            Dictionary<string, UnitsDailyData> wholeDayResult = new Dictionary<string, UnitsDailyData>();

            if (!isRecalculate)
            {
                resultDaily = this.CalculateDailyDataFromShiftData(targetUnitDailyRecordConfigs, targetDay, processUnitId);
            }
            else
            {
                wholeDayResult = this.GetDailyDataFromDailyData(targetDay, processUnitId);
                resultDaily = wholeDayResult.Where(x => x.Value.UnitsDailyConfig.AggregationCurrentLevel == false).ToDictionary(x => x.Key, x => x.Value);
            }

            Dictionary<string, UnitsDailyData> relatedRecords = this.GetRelatedData(processUnitId, targetDay, materialTypeId);
            this.AppendRelatedRecords(relatedRecords, resultDaily);

            this.CalculateDailyDataFromRelatedDailyData(ref resultDaily, processUnitId, targetDay, isRecalculate, editReasonId, materialTypeId);

            this.ClearRelatedRecords(relatedRecords, resultDaily);

            this.ClearUnModifiedRecords(resultDaily, wholeDayResult);

            if (!isRecalculate)
            {
                this.AppendTotalMonthQuantityToDailyRecords(resultDaily, processUnitId, targetDay, materialTypeId);
            }

            return resultDaily.Select(x => x.Value);
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
            if (relatedRecords.Any())
            {
                foreach (var item in relatedRecords)
                {
                    resultDaily.Remove(item.Key);
                }
            }
        }

        private void AppendRelatedRecords(Dictionary<string, UnitsDailyData> relatedRecords, Dictionary<string, UnitsDailyData> resultDaily)
        {
            if (relatedRecords.Any())
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

        /// <summary>
        /// Appends the total month quantity to daily records.
        /// </summary>
        /// <param name="resultDaily">The result daily.</param>
        /// <param name="processUnitId">The process unit id.</param>
        /// <param name="targetDay">The target day.</param>
        public void AppendTotalMonthQuantityToDailyRecords(Dictionary<string, UnitsDailyData> resultDaily, int processUnitId, DateTime targetDay, int materialTypeId)
        {
            var beginningOfMonth = new DateTime(targetDay.Year, targetDay.Month, 1);
            var endOfObservedPeriod = new DateTime(targetDay.Year, targetDay.Month, targetDay.Day);

            var totalMonthQuantities = this.data.UnitsDailyDatas.All().Include(x => x.UnitsDailyConfig).Include(x => x.UnitsManualDailyData)
               .Where(x => x.UnitsDailyConfig.ProcessUnitId == processUnitId &&
                       !x.UnitsDailyConfig.NotATotalizedPosition &&
                       beginningOfMonth <= x.RecordTimestamp &&
                       x.RecordTimestamp < endOfObservedPeriod &&
                       x.UnitsDailyConfig.MaterialTypeId == materialTypeId).ToList()
                       .GroupBy(x => x.UnitsDailyConfig.Code)
                       .Select(group => new { Code = group.Key, Value = group.Sum(x => x.RealValue) }).ToDictionary(x => x.Code, x => x.Value);

            foreach (var item in resultDaily)
            {
                if (totalMonthQuantities.ContainsKey(item.Key))
                {
                    resultDaily[item.Key].TotalMonthQuantity = (decimal) totalMonthQuantities[item.Key];
                }
                else
                {
                    resultDaily[item.Key].TotalMonthQuantity = 0m;
                }
            }
        }

        /// <summary>
        /// Calculates the daily data from shift data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfigs">The target unit daily record configs.</param>
        /// <param name="targetDay">The target day.</param>
        /// <returns></returns>
        private Dictionary<string, UnitsDailyData> CalculateDailyDataFromShiftData(IEnumerable<UnitDailyConfig> targetUnitDailyRecordConfigs, DateTime targetDay, int processUnitId)
        {

            var targetUnitDatas = this.data.UnitsData.All()
                  .Include(x => x.UnitConfig)
                  .Include(x => x.UnitsManualData)
                  .Where(x => x.RecordTimestamp == targetDay).ToList();

            Dictionary<string, UnitsDailyData> result = new Dictionary<string, UnitsDailyData>();

            foreach (var targetUnitDailyRecordConfig in targetUnitDailyRecordConfigs.Where(x => x.AggregationCurrentLevel == false))
            {
                double dailyValue = 0;

                var dailyRecord = new UnitsDailyData()
                {
                    RecordTimestamp = targetDay,
                    UnitsDailyConfigId = targetUnitDailyRecordConfig.Id
                };

                var positionDictionary = targetUnitDailyRecordConfig.UnitConfigUnitDailyConfigs
                                        .Select(x => new { Id = x.UnitConfigId, Position = x.Position }).Distinct()
                                        .ToDictionary(x => x.Id, x => x.Position);

                var unitDatasByShift = this.GetUnitDatasForDailyData(targetUnitDailyRecordConfig, targetUnitDatas);

                foreach (var shift in unitDatasByShift)
                {
                    var shiftValue = this.GetShiftValue(shift, targetUnitDailyRecordConfig, positionDictionary);
                    dailyValue += shiftValue;
                }

                dailyRecord.Value = (decimal) dailyValue;
                dailyRecord.HasManualData = unitDatasByShift.SelectMany(x => x).Any(y => y.IsManual);
                result.Add(targetUnitDailyRecordConfig.Code, dailyRecord);
            }
            return result;
        }

        /// <summary>
        /// Gets the unit datas for daily data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfig">The target unit daily record config.</param>
        /// <param name="targetUnitData">The target unit data</param>
        /// <returns></returns>
        private IEnumerable<IGrouping<int, UnitsData>> GetUnitDatasForDailyData(UnitDailyConfig targetUnitDailyRecordConfig, IEnumerable<UnitsData> targetUnitData)
        {
            var targetUnitCongifs = targetUnitDailyRecordConfig.UnitConfigUnitDailyConfigs.Select(x => x.UnitConfigId).ToList();
            return targetUnitData.Where(x => targetUnitCongifs.Any(y => y == x.UnitConfigId)).ToList().GroupBy(z => (int) z.ShiftId);
        }

        /// <summary>
        /// Calculates the daily data from related daily data.
        /// </summary>
        /// <param name="resultDaily">The result daily.</param>
        /// <param name="targetPtocessUnitId"></param>
        /// <param name="targetDay">The target day.</param>
        /// <param name="isRecalculate">Recalculation prevents creation of new records</param>
        /// <param name="editReasonId">Copy the reason of the origin of changes</param>
        /// <param name="materialTypeId">The Material Type Id</param>
        private void CalculateDailyDataFromRelatedDailyData(ref Dictionary<string, UnitsDailyData> resultDaily, int targetPtocessUnitId, DateTime targetDay, bool isRecalculate, int editReasonId, int materialTypeId)
        {
            Dictionary<string, UnitsDailyData> calculationResult = new Dictionary<string, UnitsDailyData>();

            var targetRecords = this.data.UnitsDailyConfigs.All()
                .Include(x => x.RelatedUnitDailyConfigs)
                .Where(x => x.ProcessUnitId == targetPtocessUnitId
                        && x.AggregationCurrentLevel == true
                        && x.MaterialTypeId == materialTypeId).ToList();

            Dictionary<string, UnitsDailyData> targetUnitDailyRecords = new Dictionary<string, UnitsDailyData>();

            if (isRecalculate)
            {
                targetUnitDailyRecords = this.data.UnitsDailyDatas.All()
                  .Include(x => x.UnitsDailyConfig)
                  .Include(x => x.UnitsManualDailyData)
                  .Where(x => x.RecordTimestamp == targetDay && x.UnitsDailyConfig.ProcessUnitId == targetPtocessUnitId).ToDictionary(x => x.UnitsDailyConfig.Code);
            }

            foreach (var targetUnitDailyRecordConfig in targetRecords)
            {
                var positionDictionary = targetUnitDailyRecordConfig.RelatedUnitDailyConfigs
                                       .Select(x => new { Id = x.RelatedUnitsDailyConfigId, Position = x.Position }).Distinct()
                                       .ToDictionary(x => x.Id, x => x.Position);

                var unitDailyDatasForPosition = this.GetUnitDatasForRelatedDailyData(targetUnitDailyRecordConfig, resultDaily);
                double dailyValue = this.GetDailyValue(unitDailyDatasForPosition, targetUnitDailyRecordConfig, positionDictionary);

                var dailyRecord = new UnitsDailyData()
                {
                    Id = (isRecalculate && targetUnitDailyRecords.ContainsKey(targetUnitDailyRecordConfig.Code)) ? targetUnitDailyRecords[targetUnitDailyRecordConfig.Code].Id : 0,
                    RecordTimestamp = targetDay,
                    UnitsDailyConfigId = targetUnitDailyRecordConfig.Id,
                    UnitsManualDailyData = (isRecalculate && targetUnitDailyRecords.ContainsKey(targetUnitDailyRecordConfig.Code)) ? targetUnitDailyRecords[targetUnitDailyRecordConfig.Code].UnitsManualDailyData : null
                };

                if (isRecalculate)
                {
                    if (dailyRecord.UnitsManualDailyData == null)
                    {
                        dailyRecord.UnitsManualDailyData = new UnitsManualDailyData
                        {
                            Id = dailyRecord.Id,
                            Value = (decimal) dailyValue,
                            EditReasonId = editReasonId
                        };
                    }
                    else
                    {
                        dailyRecord.UnitsManualDailyData.Value = (decimal) dailyValue;
                        dailyRecord.UnitsManualDailyData.EditReasonId = editReasonId;
                    }
                }
                else
                {
                    dailyRecord.Value = (decimal) dailyValue;
                }

                resultDaily.Add(targetUnitDailyRecordConfig.Code, dailyRecord);
                if (isRecalculate)
                {
                    calculationResult.Add(targetUnitDailyRecordConfig.Code, dailyRecord);
                }
            }

            if (isRecalculate)
            {
                resultDaily = calculationResult;
            }
        }

        /// <summary>
        /// Gets the unit datas for related daily data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfig">The target unit daily record config.</param>
        /// <param name="resultDaily">The result daily.</param>
        /// <param name="targetDay">The target day.</param>
        /// <returns></returns>
        private IEnumerable<UnitsDailyData> GetUnitDatasForRelatedDailyData(UnitDailyConfig targetUnitDailyRecordConfig, Dictionary<string, UnitsDailyData> resultDaily)
        {
            var targetCodes = targetUnitDailyRecordConfig.RelatedUnitDailyConfigs.OrderBy(x => x.Position).Select(x => x.RelatedUnitsDailyConfig.Code);
            var result = new Dictionary<string, UnitsDailyData>();
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
        private double GetDailyValue(IEnumerable<UnitsDailyData> unitDailyDatasForPosition, UnitDailyConfig unitDailyConfig, Dictionary<int, int> positionDictionary)
        {
            var inputDictionary = new Dictionary<string, double>();

            foreach (var unit in unitDailyDatasForPosition)
            {
                int currentIndex = positionDictionary[unit.UnitsDailyConfigId];
                inputDictionary.Add(string.Format("p{0}", currentIndex - 1), unit.RealValue);
            }

            return this.calculator.Calculate(unitDailyConfig.AggregationFormula, "p", inputDictionary.Count, inputDictionary, unitDailyConfig?.Code);
        }

        /// <summary>
        /// Gets the shift value.
        /// </summary>
        /// <param name="shift">The shift.</param>
        /// <returns></returns>
        private double GetShiftValue(IGrouping<int, UnitsData> shift, UnitDailyConfig unitDailyConfig, Dictionary<int, int> positionDictionary)
        {
            var inputDictionary = new Dictionary<string, double>();
            foreach (var unit in shift)
            {
                int currentIndex = positionDictionary[unit.UnitConfigId];
                inputDictionary.Add(string.Format("p{0}", currentIndex - 1), unit.RealValue);
            }

            return this.calculator.Calculate(unitDailyConfig.AggregationFormula, "p", inputDictionary.Count, inputDictionary, unitDailyConfig.Code);
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
            var status = this.kernel.Get<IEfStatus>();
            if (targetDate.Day > 1)
            {
                var beginingOfTheMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
                var endOfObservedPeriod = targetDate.Date.AddDays(-1);

                var approvedDays = this.data.UnitsApprovedDailyDatas.All()
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

            var status = this.kernel.Get<IEfStatus>();

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

        public ChartViewModel<DateTime, decimal> GetStatisticForProcessUnit(int processUnitId, DateTime targetDate, int? materialTypeId = null)
        {
            var beginingOfTheMonth = new DateTime(targetDate.Year, targetDate.Month, 1);

            var statistic = this.data.ProductionPlanDatas.All().Include(x => x.ProductionPlanConfig)
                                .Where(x => beginingOfTheMonth <= x.RecordTimestamp
                                        && x.RecordTimestamp < targetDate
                                        && x.ProcessUnitId == processUnitId
                                        && x.ProductionPlanConfig.MaterialTypeId == (materialTypeId ?? x.ProductionPlanConfig.MaterialTypeId))
                                .GroupBy(x => x.ProductionPlanConfigId).ToList();
            var charts = new List<DataSery<DateTime, decimal>>();

            foreach (var parameter in statistic)
            {
                if (parameter.Count() > 0)
                {
                    IEnumerable<DataSery<DateTime, decimal>> series = this.GetSeriesFromData(parameter, beginingOfTheMonth, targetDate);
                    charts.AddRange(series);
                }
            }

            var orderedCharts = charts.OrderByDescending(x => x.Values.FirstOrDefault().Y).ThenBy(x => x.Label.Replace("план", "").Replace("факт", "")).ToList();
            return new ChartViewModel<DateTime, decimal>() { DataSeries = orderedCharts };
        }

        public async Task<ChartViewModel<DateTime, decimal>> GetStatisticForProcessUnitAsync(int processUnitId, DateTime beginDate, DateTime endDate, int? materialTypeId = null)
        {
            if ((endDate - beginDate) > TimeSpan.FromDays(CommonConstants.MaxDateDifference))
            {
                throw new ArgumentOutOfRangeException();
            }

            var statistic = await this.data.ProductionPlanDatas.All().Include(x => x.ProductionPlanConfig)
                                .Where(x => beginDate <= x.RecordTimestamp
                                        && x.RecordTimestamp <= endDate
                                        && x.ProcessUnitId == processUnitId
                                        && x.ProductionPlanConfig.MaterialTypeId == (materialTypeId ?? x.ProductionPlanConfig.MaterialTypeId))
                                .GroupBy(x => x.ProductionPlanConfigId).ToListAsync();
            var charts = new List<DataSery<DateTime, decimal>>();

            foreach (var parameter in statistic.OrderByDescending(x => x.Key))
            {
                if (parameter.Count() > 0)
                {
                    IEnumerable<DataSery<DateTime, decimal>> series = await Task.Factory.StartNew(() => this.GetSeriesFromData(parameter, beginDate, endDate));
                    charts.AddRange(series);
                }
            }

            var orderedCharts = charts.OrderByDescending(x => x.Values.FirstOrDefault().Y).ThenBy(x => x.Label.Replace("план", "").Replace("факт", "")).ToList();
            return new ChartViewModel<DateTime, decimal>() { DataSeries = orderedCharts };
        }


        public async Task<ChartViewModel<DateTime, decimal>> GetStatisticForProcessUnitLoadAsync(int processUnitId, DateTime beginDate, DateTime endDate, int? materialTypeId = null)
        {
            if ((endDate - beginDate) > TimeSpan.FromDays(CommonConstants.MaxDateDifference))
            {
                throw new ArgumentOutOfRangeException();
            }
            var beginingOfTheMonth = beginDate;

            var processingDailyRecords = await this.data.ProductionPlanDatas.All().Include(x => x.ProductionPlanConfig)
                                .Where(x => beginingOfTheMonth <= x.RecordTimestamp
                                        && x.RecordTimestamp <= endDate
                                        && x.ProcessUnitId == processUnitId
                                        && x.ProductionPlanConfig.IsSummaryOfProcessing).ToDictionaryAsync(x => x.RecordTimestamp);
            var workHoursDalyRecords = await this.data.UnitsDailyDatas.All().Include(x => x.UnitsDailyConfig)
                                .Where(x => beginingOfTheMonth <= x.RecordTimestamp
                                        && x.RecordTimestamp <= endDate
                                        && x.UnitsDailyConfig.ProcessUnitId == processUnitId
                                        && x.UnitsDailyConfig.IsTotalWorkTime).ToDictionaryAsync(x => x.RecordTimestamp);
            var data = new Dictionary<DateTime, ProcessUnitLoadRecord>();
            for (DateTime date = beginDate; date <= endDate; date = date.AddDays(1))
            {
                decimal workTime = 0m;

                if (workHoursDalyRecords.ContainsKey(date))
                {
                    workTime = Convert.ToDecimal(workHoursDalyRecords[date].RealValue);
                }

                if (workTime == 0m)
                {
                    workTime = 24m;
                }

                ProductionPlanData record = new ProductionPlanData();
                if (processingDailyRecords.ContainsKey(date))
                {
                    record = processingDailyRecords[date];
                }

                var loadPerHourFact = record.QuantityFact / workTime;
                var loadPerHourPlan = record.QuanityPlan / 24m;
                data.Add(date, new ProcessUnitLoadRecord(date, loadPerHourPlan, loadPerHourFact));
            }
            var chart = new List<DataSery<DateTime, decimal>>();

            if (data.Count > 0)
            {
                IEnumerable<DataSery<DateTime, decimal>> series = await Task.Factory.StartNew(() => this.GetSeriesFromLoadData(data, beginDate, endDate));
                chart.AddRange(series);
            }

            if (chart.Count < 1)
            {
                chart.Add(new DataSery<DateTime, decimal>());
            }
            return new ChartViewModel<DateTime, decimal>() { DataSeries = chart };
        }

        private IEnumerable<DataSery<DateTime, decimal>> GetSeriesFromLoadData(Dictionary<DateTime, ProcessUnitLoadRecord> data, DateTime beginDate, DateTime endDate)
        {
            var model = data.First();
            DataSery<DateTime, decimal> plan = new DataSery<DateTime, decimal>("area", string.Format("{0} план(T/h)", "Натоварване"), "thone", null);
            DataSery<DateTime, decimal> fact = new DataSery<DateTime, decimal>("line", string.Format("{0} факт(T/h)", "Натоварване"), "thone", null);

            for (DateTime target = beginDate; target <= endDate; target = target.AddDays(1))
            {
                if (data.ContainsKey(target.Date))
                {
                    plan.Values.Add(new Pair<DateTime, decimal>(target.Date, data[target.Date].LoadPlan));
                    fact.Values.Add(new Pair<DateTime, decimal>(target.Date, data[target.Date].LoadFact));
                }
                else
                {
                    plan.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
                    fact.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
                }
            }

            return new List<DataSery<DateTime, decimal>>() { plan, fact };
        }

        /// <summary>
        /// Gets the series from data.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="beginingOfTheMonth">The begining of the month.</param>
        /// <param name="targetDate">The target date.</param>
        /// <returns></returns>
        private IEnumerable<DataSery<DateTime, decimal>> GetSeriesFromData(IEnumerable<ProductionPlanData> parameter, DateTime beginDate, DateTime endDate)
        {
            var model = parameter.First();

            var paramTransformed = parameter.ToDictionary(x => x.RecordTimestamp);

            DataSery<DateTime, decimal> planPercent = new DataSery<DateTime, decimal>("area", string.Format("{0} план(%)", model.Name), "percent", null, model.ProductionPlanConfig.IsPercentagesPlanVisibleInChast);
            DataSery<DateTime, decimal> factPercent = new DataSery<DateTime, decimal>("line", string.Format("{0} факт(%)", model.Name), "percent", null, model.ProductionPlanConfig.IsPercentagesFactVisibleInChast);
            DataSery<DateTime, decimal> plan = new DataSery<DateTime, decimal>("area", string.Format("{0} план(T)", model.Name), "thone", null, model.ProductionPlanConfig.IsQuanityPlanVisibleInChast);
            DataSery<DateTime, decimal> fact = new DataSery<DateTime, decimal>("line", string.Format("{0} факт(T)", model.Name), "thone", null, model.ProductionPlanConfig.IsQuantityFactVisibleInChast);

            for (DateTime target = beginDate; target <= endDate; target = target.AddDays(1))
            {
                if (paramTransformed.ContainsKey(target.Date))
                {
                    planPercent.Values.Add(new Pair<DateTime, decimal>(target.Date, paramTransformed[target.Date].PercentagesPlan));
                    factPercent.Values.Add(new Pair<DateTime, decimal>(target.Date, paramTransformed[target.Date].PercentagesFact));
                    plan.Values.Add(new Pair<DateTime, decimal>(target.Date, paramTransformed[target.Date].QuanityPlan));
                    fact.Values.Add(new Pair<DateTime, decimal>(target.Date, paramTransformed[target.Date].QuantityFact));
                }
                else
                {
                    planPercent.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
                    factPercent.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
                    plan.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
                    fact.Values.Add(new Pair<DateTime, decimal>(target.Date, 0m));
                }
            }

            return new List<DataSery<DateTime, decimal>>() { planPercent, factPercent, plan, fact };
        }

        public IEfStatus CheckIfDayIsApprovedButEnergyNot(DateTime date, int processUnitId, out bool readyForCalculation)
        {
            var status = this.kernel.Get<IEfStatus>();
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

    class ProcessUnitLoadRecord
    {
        public ProcessUnitLoadRecord(DateTime timestamp, decimal loadPlan, decimal loadFact)
        {
            this.RecordTimeStamp = timestamp;
            this.LoadPlan = loadPlan;
            this.LoadFact = loadFact;
        }
        public DateTime RecordTimeStamp { get; set; }

        public decimal LoadPlan { get; set; }

        public decimal LoadFact { get; set; }
    }
}
