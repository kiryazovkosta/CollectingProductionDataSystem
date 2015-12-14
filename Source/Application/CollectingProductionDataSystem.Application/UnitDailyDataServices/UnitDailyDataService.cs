/// <summary>
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
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Chart;
    using CollectingProductionDataSystem.Models.Productions;
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

        public IEnumerable<UnitsDailyData> CalculateDailyDataForProcessUnit(int processUnitId, DateTime targetDay)
        {
            if (processUnitId < 1)
            {
                string message = App_Resources.ErrorMessages.InvalidParameter;
                throw new ArgumentException(string.Format(message, "processUnitId", 1, int.MaxValue));
            }

            // Day is ready and calculations begin
            var targetUnitDailyRecordConfigs = data.UnitsDailyConfigs.All()
                .Include(x => x.UnitConfigUnitDailyConfigs)
                .Where(x => x.ProcessUnitId == processUnitId).ToList();
            Dictionary<string, UnitsDailyData> resultDaily = CalculateDailyDataFromShiftData(targetUnitDailyRecordConfigs, targetDay, processUnitId);
            Dictionary<string, UnitsDailyData> relatedRecords = GetRelatedData(processUnitId, targetDay);
            if (relatedRecords.Count() > 0)
            {
                foreach (var item in relatedRecords)
                {
                    resultDaily.Add(item.Key, item.Value);
                }
            }

            CalculateDailyDataFromRelatedDailyData(resultDaily, processUnitId, targetDay);

            if (relatedRecords.Count() > 0)
            {
                foreach (var item in relatedRecords)
                {
                    resultDaily.Remove(item.Key);
                }
            }

            AppendTotalMonthQuantityToDailyRecords(resultDaily, processUnitId, targetDay);

            return resultDaily.Select(x => x.Value);
        }

        /// <summary>
        /// Appends the total month quantity to daily records.
        /// </summary>
        /// <param name="resultDaily">The result daily.</param>
        /// <param name="processUnitId">The process unit id.</param>
        /// <param name="targetDay">The target day.</param>
        private void AppendTotalMonthQuantityToDailyRecords(Dictionary<string, UnitsDailyData> resultDaily, int processUnitId, DateTime targetDay)
        {
            var beginningOfMonth = new DateTime(targetDay.Year, targetDay.Month, 1);
            var TotalMonthQuantities = data.UnitsDailyDatas.All().Include(x => x.UnitsDailyConfig)
                .Join(data.UnitsApprovedDailyDatas.All(),
                        units => new UnitDailyToApprove { ProcessUnitId = units.UnitsDailyConfig.ProcessUnitId, RecordDate = units.RecordTimestamp },
                        appd => new UnitDailyToApprove { ProcessUnitId = appd.ProcessUnitId, RecordDate = appd.RecordDate },
                        (units, appd) => new { Units = units, Appd = appd })
                .Where(x => x.Units.UnitsDailyConfig.ProcessUnitId == processUnitId &&
                        beginningOfMonth <= x.Units.RecordTimestamp &&
                        x.Units.RecordTimestamp < targetDay &&
                        x.Appd.Approved == true).GroupBy(x => x.Units.UnitsDailyConfig.Code).ToList()
                        .Select(group => new { Code = group.Key, Value = group.Sum(x => x.Units.RealValue) }).ToDictionary(x => x.Code, x => x.Value);

            foreach (var item in resultDaily)
            {
                if (TotalMonthQuantities.ContainsKey(item.Key))
                {
                    resultDaily[item.Key].TotalMonthQuantity = (decimal)TotalMonthQuantities[item.Key];
                }
                else
                {
                    resultDaily[item.Key].TotalMonthQuantity = 0m;
                }
            }
            Debug.WriteLine("finish");
        }

        /// <summary>
        /// Calculates the daily data from shift data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfigs">The target unit daily record configs.</param>
        /// <param name="targetDay">The target day.</param>
        /// <returns></returns>
        private Dictionary<string, UnitsDailyData> CalculateDailyDataFromShiftData(IEnumerable<UnitDailyConfig> targetUnitDailyRecordConfigs, DateTime targetDay, int processUnitId)
        {

            var targetUnitDatas = data.UnitsData.All()
                  .Include(x => x.UnitConfig)
                  .Include(x => x.UnitsManualData)
                  .Where(x => x.RecordTimestamp == targetDay && x.UnitConfig.ProcessUnitId == processUnitId).ToList();

            Dictionary<string, UnitsDailyData> result = new Dictionary<string, UnitsDailyData>();

            foreach (var targetUnitDailyRecordConfig in targetUnitDailyRecordConfigs.Where(x => x.AggregationCurrentLevel == false))
            {
                double dailyValue = 0;

                var dailyRecord = new UnitsDailyData()
                {
                    RecordTimestamp = targetDay,
                    UnitsDailyConfigId = targetUnitDailyRecordConfig.Id
                };

                var unitDatasByShift = GetUnitDatasForDailyData(targetUnitDailyRecordConfig, targetUnitDatas);

                foreach (var shift in unitDatasByShift)
                {
                    var shiftValue = GetShiftValue(shift, targetUnitDailyRecordConfig);
                    dailyValue += shiftValue;
                }

                dailyRecord.Value = (decimal)dailyValue;
                dailyRecord.HasManualData = unitDatasByShift.SelectMany(x => x).Any(y => y.IsManual);
                result.Add(targetUnitDailyRecordConfig.Code, dailyRecord);

                //Console.WriteLine("{0}\t\t{2}\t\t{1}", targetUnitDailyRecordConfig.Code, targetUnitDailyRecordConfig.Name, dailyRecord.Value);
            }
            return result;
        }

        /// <summary>
        /// Gets the unit datas for daily data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfig">The target unit daily record config.</param>
        /// <param name="targetDay">The target day.</param>
        /// <returns></returns>
        private IEnumerable<IGrouping<int, UnitsData>> GetUnitDatasForDailyData(UnitDailyConfig targetUnitDailyRecordConfig, IEnumerable<UnitsData> targetUnitData)
        {
            var targetUnitCongifs = targetUnitDailyRecordConfig.UnitConfigUnitDailyConfigs.Select(x => x.UnitConfigId).ToList();
            return targetUnitData.Where(x => targetUnitCongifs.Any(y => y == x.UnitConfigId)).ToList().GroupBy(z => (int)z.ShiftId);
        }

        /// <summary>
        /// Calculates the daily data from related daily data.
        /// </summary>
        /// <param name="targetRecords">The target records.</param>
        /// <param name="resultDaily">The result daily.</param>
        /// <param name="targetDay">The target day.</param>
        private void CalculateDailyDataFromRelatedDailyData(Dictionary<string, UnitsDailyData> resultDaily, int targetPtocessUnitId, DateTime targetDay)
        {
            var targetRecords = data.UnitsDailyConfigs.All()
                .Include(x => x.RelatedUnitDailyConfigs)
                .Where(x => x.ProcessUnitId == targetPtocessUnitId && x.AggregationCurrentLevel == true).ToList();

            foreach (var targetUnitDailyRecordConfig in targetRecords)
            {
                var unitDailyDatasForPosition = GetUnitDatasForRelatedDailyData(targetUnitDailyRecordConfig, resultDaily);
                double dailyValue = GetDailyValue(unitDailyDatasForPosition, targetUnitDailyRecordConfig);

                var dailyRecord = new UnitsDailyData()
                {
                    RecordTimestamp = targetDay,
                    UnitsDailyConfigId = targetUnitDailyRecordConfig.Id,
                    Value = (decimal)dailyValue
                };

                resultDaily.Add(targetUnitDailyRecordConfig.Code, dailyRecord);

                //Console.WriteLine("{0}\t\t{2}\t\t{1}", targetUnitDailyRecordConfig.Code, targetUnitDailyRecordConfig.Name, dailyRecord.Value);
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
        private double GetDailyValue(IEnumerable<UnitsDailyData> unitDailyDatasForPosition, UnitDailyConfig unitDailyConfig)
        {
            var inputDictionary = new Dictionary<string, double>();
            var ix = 0;

            foreach (var unit in unitDailyDatasForPosition)
            {
                inputDictionary.Add(string.Format("p{0}", ix++), unit.RealValue);
            }

            return calculator.Calculate(unitDailyConfig.AggregationFormula, "p", inputDictionary.Count, inputDictionary);
        }

        /// <summary>
        /// Gets the shift value.
        /// </summary>
        /// <param name="shift">The shift.</param>
        /// <returns></returns>
        private double GetShiftValue(IGrouping<int, UnitsData> shift, UnitDailyConfig unitDailyConfig)
        {
            var inputDictionary = new Dictionary<string, double>();
            var ix = 0;

            foreach (var unit in shift)
            {
                inputDictionary.Add(string.Format("p{0}", ix++), unit.RealValue);
            }

            return calculator.Calculate(unitDailyConfig.AggregationFormula, "p", inputDictionary.Count, inputDictionary);
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
                .Where(x => x.ProcessUnitId == processUnitId && x.RecordDate == targetDate).Any();
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
        /// Checks if previous days are ready.
        /// </summary>
        /// <param name="processUnitId">The process unit identifier.</param>
        /// <param name="targetDate">The target date.</param>
        /// <returns></returns>
        public IEfStatus CheckIfPreviousDaysAreReady(int processUnitId, DateTime targetDate)
        {
            var beginingOfTheMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
            var endOfObservedPeriod = targetDate.Date.AddDays(-1);

            var approvedDays = data.UnitsApprovedDailyDatas.All()
                                    .Where(x => x.ProcessUnitId == processUnitId
                                            && beginingOfTheMonth <= x.RecordDate
                                            && x.RecordDate <= endOfObservedPeriod)
                                    .Select(x => x.RecordDate).ToDictionary(x => x);

            int day = 0;
            var validationResults = new List<ValidationResult>() { new ValidationResult(@Resources.ErrorMessages.PreviousDaysConfirmationError) };
            while (day < endOfObservedPeriod.Day)
            {
                var checkedDay = beginingOfTheMonth.AddDays(day);
                if (!approvedDays.ContainsKey(checkedDay))
                {
                    validationResults.Add(new ValidationResult(string.Format("\t\t- {0:dd.MM.yyyy г.}", checkedDay)));
                }

                day += 1;
            }

            var status = kernel.Get<IEfStatus>();
            if (validationResults.Count > 1)
            {
                status.SetErrors(validationResults);
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

        private Dictionary<string, UnitsDailyData> GetRelatedData(int processUnitId, DateTime targetDay)
        {
            var relatedUnitsDailyData = new Dictionary<string, UnitsDailyData>();

            var relatedDailyDatasFromOtherProcessUnits = this.data.UnitsDailyConfigs
                .All()
                .Include(x => x.ProcessUnit)
                .Include(x => x.RelatedUnitDailyConfigs)
                .Where(x => x.ProcessUnitId == processUnitId && x.AggregationCurrentLevel == true)
                .SelectMany(y => y.RelatedUnitDailyConfigs)
                .Where(z => z.RelatedUnitsDailyConfig.ProcessUnitId != processUnitId)
                .ToList();

            foreach (var item in relatedDailyDatasFromOtherProcessUnits)
            {
                var relatedData = this.data.UnitsDailyDatas.All().Where(u => u.RecordTimestamp == targetDay && u.UnitsDailyConfigId == item.RelatedUnitsDailyConfigId).FirstOrDefault();
                if (relatedData != null)
                {
                    relatedUnitsDailyData.Add(relatedData.UnitsDailyConfig.Code, relatedData);
                }
            }

            return relatedUnitsDailyData;
        }

        public ChartViewModel<DateTime, decimal> GetStatisticForProcessUnit(int processUnitId, DateTime targetDate)
        {
            var beginingOfTheMonth = new DateTime(targetDate.Year, targetDate.Month, 1);

            var statistic = this.data.ProductionPlanDatas.All()
                                .Where(x => beginingOfTheMonth <= x.RecordTimestamp
                                        && x.RecordTimestamp < targetDate
                                        && x.ProcessUnitId == processUnitId)
                                .GroupBy(x => x.ProductionPlanConfigId).ToList();
            var charts = new List<DataSery<DateTime, decimal>>();

            foreach (var parameter in statistic)
            {
                if (parameter.Count() > 0)
                {
                    IEnumerable<DataSery<DateTime, decimal>> series = GetSeriesFromData(parameter, beginingOfTheMonth, targetDate);
                    charts.AddRange(series);
                }
            }

            return new ChartViewModel<DateTime, decimal>() { DataSeries = charts };
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

            DataSery<DateTime, decimal> planPercent = new DataSery<DateTime, decimal>("area", string.Format("{0} план(%)", model.Name));
            DataSery<DateTime, decimal> factPercent = new DataSery<DateTime, decimal>("line", string.Format("{0} факт(%)", model.Name));
            DataSery<DateTime, decimal> plan = new DataSery<DateTime, decimal>("area", string.Format("{0} план(T)", model.Name));
            DataSery<DateTime, decimal> fact = new DataSery<DateTime, decimal>("line", string.Format("{0} факт(T)", model.Name));

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

        private class RecordsCollectionId
        {
            public DateTime RecordTimestamp { get; set; }
            public int Id { get; set; }
        }
    }
}
