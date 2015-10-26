/// <summary>
/// Summary description for UnitDailyDataService
/// </summary>
namespace CollectingProductionDataSystem.Application.UnitDailyDataServices
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Transactions;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using Ninject;

    public class UnitDailyDataService
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

            // Day is not ready
            //if (!CheckIfAllShiftsAreReady(targetDay, processUnitId))
            //{
            //    return new List<UnitsDailyData>();
            //}

            // Day is ready and calculations begin
            var targetUnitDailyRecordConfigs = data.UnitsDailyConfigs.All()
                .Include(x => x.UnitConfigUnitDailyConfigs)
                //.Include(x => x.UnitConfigUnitDailyConfigs)
                //.Include(x => x.RelatedUnitDailyConfigs)
                //.Include(x => x.UnitConfigUnitDailyConfigs.Select(y => y.UnitConfig).Select(z => z.UnitsDatas.Select(f => f.UnitsManualData)))
                ////.Include(x => x.RelatedUnitDailyConfigs)
              .Where(x => x.ProcessUnitId == processUnitId).ToList();
            Dictionary<string, UnitsDailyData> resultDaily = CalculateDailyDataFromShiftData(targetUnitDailyRecordConfigs, targetDay, processUnitId);

            CalculateDailyDataFromRelatedDailyData(resultDaily, processUnitId, targetDay);
            return resultDaily.Select(x => x.Value);
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
            //targetUnitDailyRecordConfig.UnitConfigUnitDailyConfigs.OrderBy(x => x.Position)
            //.Select(x => x.UnitConfig)
            //.SelectMany(y => y.UnitsDatas)
            //    .Where(z => z.RecordTimestamp == targetDay)
            //    .GroupBy(w => (int)w.ShiftId);
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
                //.Include(x => x.UnitConfigUnitDailyConfigs)
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

       

        //public IEfStatus CalculateAndSaveDailyData(int processUnitId, DateTime targetDay, string userName)
        //{
        //    if (processUnitId < 1)
        //    {
        //        throw new ArgumentException(string.Format(App_Resources.ErrorMessages.InvalidParameter, "processUnitId", 1, int.MaxValue));
        //    }

        //    // Day is not ready
        //    if (!CheckIfAllShiftsAreReady(targetDay, processUnitId))
        //    {
        //        return this.kernel.Get(typeof(IEfStatus)) as IEfStatus;
        //    }

        //    // Day is ready and calculations begin
        //    var targetUnitDailyRecordConfigs = data.UnitsDailyConfigs.All()
        //       .Include(x => x.UnitConfigUnitDailyConfigs)
        //       .Include(x => x.UnitConfigUnitDailyConfigs.Select(y => y.UnitConfig).Select(z => z.UnitsDatas.Select(f => f.UnitsManualData)))
        //       .Include(x => x.RelatedUnitDailyConfigs.Select(y => y.RelatedUnitsDailyConfig).Select(z => z.UnitsDailyDatas.Select(w => w.UnitsManualDailyData)))
        //       .Include(x => x.UnitConfigUnitDailyConfigs.Select(y => y.UnitDailyConfig).Select(w => w.ProcessUnit))
        //       .Where(x => x.ProcessUnitId == processUnitId)
        //       .ToList();

        //    //Day is already calculated
        //    if (DayIsCalculated(targetUnitDailyRecordConfigs, targetDay))
        //    {
        //        return this.kernel.Get(typeof(IEfStatus)) as IEfStatus;
        //    }



        //    using (var transaction = new TransactionScope(TransactionScopeOption.Required,
        //        new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
        //    {
        //        var status = CalculateAndSaveDailyUnitDataFromShiftData(targetUnitDailyRecordConfigs, targetDay, userName);
        //        if (!status.IsValid)
        //        {
        //            return status;
        //        }

        //        var independantSecondLevelRecords = targetUnitDailyRecordConfigs.Where(x => (x.AggregationCurrentLevel == true) && (x.IsTotal == false));

        //        status = CalculateAndSaveDailyUnitDataFromRelatedRecords(independantSecondLevelRecords, targetDay, userName);

        //        if (!status.IsValid)
        //        {
        //            return status;
        //        }

        //        var totalRecordsFromSecondLevelRecords = targetUnitDailyRecordConfigs
        //                                                    .Where(x => (x.AggregationCurrentLevel == true) && (x.IsTotal == true));

        //        status = CalculateAndSaveDailyUnitDataFromRelatedRecords(totalRecordsFromSecondLevelRecords, targetDay, userName);


        //        if (!status.IsValid)
        //        {
        //            return status;
        //        }
        //        else
        //        {
        //            transaction.Complete();
        //        }

        //        return status;
        //    }

        //}

        /// <summary>
        /// Days the is calculated.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfigs">The target unit daily record configs.</param>
        /// <returns></returns>
        private bool DayIsCalculated(IEnumerable<UnitDailyConfig> targetUnitDailyRecordConfigs, DateTime targetDay)
        {
            return targetUnitDailyRecordConfigs.SelectMany(x => x.UnitsDailyDatas).Where(y => y.RecordTimestamp == targetDay).Count() > 0;
        }

        //private IEfStatus CalculateAndSaveDailyUnitDataFromShiftData(IEnumerable<UnitDailyConfig> targetUnitDailyRecordConfigs, DateTime targetDay, string userName)
        //{
        //    List<UnitsDailyData> result = new List<UnitsDailyData>();

        //    foreach (var targetUnitDailyRecordConfig in targetUnitDailyRecordConfigs.Where(x => x.AggregationCurrentLevel == false))
        //    {
        //        double dailyValue = 0;

        //        var dailyRecord = new UnitsDailyData()
        //        {
        //            RecordTimestamp = targetDay,
        //            UnitsDailyConfigId = targetUnitDailyRecordConfig.Id
        //        };

        //        var unitDatasByShift = GetUnitDatasForDailyData(targetUnitDailyRecordConfig, targetDay);

        //        foreach (var shift in unitDatasByShift)
        //        {
        //            var shiftValue = GetShiftValue(shift, targetUnitDailyRecordConfig);
        //            dailyValue += shiftValue;
        //        }

        //        dailyRecord.Value = (decimal)dailyValue;
        //        dailyRecord.HasManualData = unitDatasByShift.SelectMany(x => x).Any(y => y.IsManual);
        //        result.Add(dailyRecord);

        //        //Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", targetUnitDailyRecordConfig.Code, targetUnitDailyRecordConfig.ProcessUnit.ShortName, targetUnitDailyRecordConfig.Name, dailyRecord.Value);

        //    }

        //    // Check if anyone else already have been calculate the day
        //    var isCalculated = DayIsCalculated(targetUnitDailyRecordConfigs.Where(x => x.AggregationCurrentLevel == false), targetDay);

        //    if ((result.Count <= 0) || (isCalculated))
        //    {
        //        return kernel.Get(typeof(IEfStatus)) as IEfStatus;
        //    }

        //    //using (var transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
        //    //{
        //    //    data.UnitsDailyDatas.BulkInsert(result, userName);
        //    //    data.DbContext.DbContext.SaveChanges();
        //    //    transactionScope.Complete();
        //    //}
        //    (data.DbContext.Set<UnitsDailyData>() as DbSet<UnitsDailyData>).AddRange(result);

        //    return this.data.SaveChanges(userName);
        //}

        /// <summary>
        /// Calculates the and save daily unit data from related records.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfigs">The target unit daily record configs.</param>
        /// <param name="targetDay">The target day.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        //private IEfStatus CalculateAndSaveDailyUnitDataFromRelatedRecords(IEnumerable<UnitDailyConfig> targetUnitDailyRecordConfigs, DateTime targetDay, string userName)
        //{
        //    List<UnitsDailyData> result = new List<UnitsDailyData>();

        //    Console.WriteLine();
        //    foreach (var targetUnitDailyRecordConfig in targetUnitDailyRecordConfigs)
        //    {
        //        var unitDailyDatasForPosition = GetUnitDatasForRelatedDailyData(targetUnitDailyRecordConfig, targetDay);
        //        double dailyValue = GetDailyValue(unitDailyDatasForPosition, targetUnitDailyRecordConfig);

        //        var dailyRecord = new UnitsDailyData()
        //        {
        //            RecordTimestamp = targetDay,
        //            UnitsDailyConfigId = targetUnitDailyRecordConfig.Id,
        //            Value = (decimal)dailyValue
        //        };

        //        result.Add(dailyRecord);

        //        //Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", targetUnitDailyRecordConfig.Code, targetUnitDailyRecordConfig.ProcessUnit.ShortName, targetUnitDailyRecordConfig.Name, dailyRecord.Value);

        //    }

        //    var isCalculated = DayIsCalculated(targetUnitDailyRecordConfigs.Where(x => x.AggregationCurrentLevel == true), targetDay);

        //    if ((result.Count <= 0) || isCalculated)
        //    {
        //        return kernel.Get(typeof(IEfStatus)) as IEfStatus;
        //    }

        //    //data.UnitsDailyDatas.BulkInsert(result, userName);
        //    (data.DbContext.Set<UnitsDailyData>() as DbSet<UnitsDailyData>).AddRange(result);

        //    return this.data.SaveChanges(userName);
        //}

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
        /// Gets the unit datas for related daily data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfig">The target unit daily record config.</param>
        /// <param name="targetDay">The target day.</param>
        /// <returns></returns>
        //private IEnumerable<UnitsDailyData> GetUnitDatasForRelatedDailyData(UnitDailyConfig targetUnitDailyRecordConfig, DateTime targetDay)
        //{
        //    return targetUnitDailyRecordConfig.RelatedUnitDailyConfigs.OrderBy(x => x.Position)
        //        .Select(x => x.RelatedUnitsDailyConfig)
        //        .SelectMany(y => y.UnitsDailyDatas)
        //            .Where(z => z.RecordTimestamp == targetDay);
        //}

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
        /// Checks if all shifts are ready.
        /// </summary>
        /// <param name="unitDatas">The unit datas.</param>
        /// <returns></returns>
        public bool CheckIfDayIsApproved(DateTime targetDate, int processUnitId)
        {
            var currentApprovedUnitDailyDatas = this.data.UnitsApprovedDailyDatas.All().Where(x => x.ProcessUnitId == processUnitId && x.RecordDate == targetDate).FirstOrDefault();

            return currentApprovedUnitDailyDatas == null;
        }


        public void ClearUnitDailyDatas(DateTime targetDate, int processUnitId, string userName)
        {
            var unitDailyDatas = this.data.UnitsDailyDatas.All().Include(x=>x.UnitsDailyConfig)
                .Where(y => y.RecordTimestamp == targetDate && y.UnitsDailyConfig.ProcessUnitId == processUnitId).ToList();
            foreach (var record in unitDailyDatas)
            {
                this.data.UnitsDailyDatas.Delete(record);
            }

            this.data.SaveChanges(userName);
        }
    }
}
