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

        public IEfStatus CalculateAndSaveDailyData(int processUnitId, DateTime targetDay, string userName)
        {
            if (processUnitId < 1)
            {
                throw new ArgumentException(string.Format(App_Resources.ErrorMessages.InvalidParameter, "processUnitId", 1, int.MaxValue));
            }

            // Day is not ready
            if (!CheckIfAllShiftsAreReady(targetDay, processUnitId))
            {
                return this.kernel.Get(typeof(IEfStatus)) as IEfStatus;
            }

            // Day is ready and calculations begin

            var targetUnitDailyRecordConfigs = data.UnitsDailyConfigs.All()
               .Include(x => x.UnitConfigUnitDailyConfigs)
               .Include(x => x.UnitConfigUnitDailyConfigs.Select(y => y.UnitConfig).Select(z => z.UnitsDatas.Select(f => f.UnitsManualData)))
               .Include(x => x.UnitConfigUnitDailyConfigs.Select(y => y.UnitDailyConfig).Select(w => w.ProcessUnit))
               .Where(x => x.ProcessUnitId == processUnitId);
            //.ToList();

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                var status = CalculateAndSaveDailyUnitDataFromShiftData(targetUnitDailyRecordConfigs, targetDay, userName);
                if (!status.IsValid)
                {
                    return status;
                }
                
                status = CalculateAndSaveDailyUnitDataFromRelatedRecords(targetUnitDailyRecordConfigs, targetDay, userName);
                transaction.Dispose();
                return status;
            }
            // Console.WriteLine("{0}\t\t{1}\t\t{2}", targetUnitDailyRecordConfig.Code, targetUnitDailyRecordConfig.ProcessUnit.ShortName, targetUnitDailyRecordConfig.Name);
        }


        private IEfStatus CalculateAndSaveDailyUnitDataFromShiftData(IEnumerable<UnitDailyConfig> targetUnitDailyRecordConfigs, DateTime targetDay, string userName)
        {
            List<UnitsDailyData> result = new List<UnitsDailyData>();

            foreach (var targetUnitDailyRecordConfig in targetUnitDailyRecordConfigs.Where(x => x.AggregationCurrentLevel == false))
            {
                double dailyValue = 0;

                var dailyRecord = new UnitsDailyData()
                {
                    RecordTimestamp = targetDay,
                    UnitsDailyConfigId = targetUnitDailyRecordConfig.Id
                };

                var unitDatasByShift = GetUnitDatasForDailyData(targetUnitDailyRecordConfig, targetDay);

                foreach (var shift in unitDatasByShift)
                {
                    var shiftValue = GetShiftValue(shift, targetUnitDailyRecordConfig);
                    dailyValue += shiftValue;
                }

                dailyRecord.Value = (decimal)dailyValue;
                dailyRecord.HasManualData = unitDatasByShift.SelectMany(x => x).Any(y => y.IsManual);
                result.Add(dailyRecord);

                Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", targetUnitDailyRecordConfig.Code, targetUnitDailyRecordConfig.ProcessUnit.ShortName, targetUnitDailyRecordConfig.Name, dailyRecord.Value);

            }

            if (result.Count < 0)
            {
                return kernel.Get(typeof(IEfStatus)) as IEfStatus;
            }
            
            Console.WriteLine(targetUnitDailyRecordConfigs.FirstOrDefault(x => x.ProcessUnitId == 1).UnitsDailyDatas.Where(y=>y.RecordTimestamp == targetDay).Count());
            (data.DbContext.Set<UnitsDailyData>() as DbSet<UnitsDailyData>).AddRange(result);
            var res = this.data.SaveChanges(userName);
            Console.WriteLine(targetUnitDailyRecordConfigs.FirstOrDefault(x => x.ProcessUnitId == 1).UnitsDailyDatas.Where(y => y.RecordTimestamp == targetDay).Count());

            return res;//this.data.SaveChanges(userName);
        }

        /// <summary>
        /// Calculates the and save daily unit data from related records.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfigs">The target unit daily record configs.</param>
        /// <param name="targetDay">The target day.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        private IEfStatus CalculateAndSaveDailyUnitDataFromRelatedRecords(IEnumerable<UnitDailyConfig> targetUnitDailyRecordConfigs, DateTime targetDay, string userName)
        {
            List<UnitsDailyData> result = new List<UnitsDailyData>();

            Console.WriteLine();
            foreach (var targetUnitDailyRecordConfig in targetUnitDailyRecordConfigs.Where(x => x.AggregationCurrentLevel == true))
            {

                //Console.WriteLine("{0}\t\t{1}\t\t{2}", targetUnitDailyRecordConfig.Code, targetUnitDailyRecordConfig.ProcessUnit.ShortName, targetUnitDailyRecordConfig.Name);

                var unitDailyDatasForPosition = GetUnitDatasForRelatedDailyData(targetUnitDailyRecordConfig, targetDay);
                //foreach (var item in unitDailyDatasForPosition)
                //{
                //    Console.WriteLine("\t\t{0}\t\t{1}\t\t{2}\t\t{3}\t\t{4}\t\t{5}", item.Id, item.RecordTimestamp, item.UnitsDailyConfig.Code, item.UnitsDailyConfig.ProcessUnit.ShortName, item.UnitsDailyConfig.Name, item.Value); 
                //}
                //Console.WriteLine();


                double dailyValue = GetDailyValue(unitDailyDatasForPosition, targetUnitDailyRecordConfig);

                var dailyRecord = new UnitsDailyData()
                {
                    RecordTimestamp = targetDay,
                    UnitsDailyConfigId = targetUnitDailyRecordConfig.Id,
                    Value = (decimal)dailyValue
                };

                //result.Add(dailyRecord);

                Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", targetUnitDailyRecordConfig.Code, targetUnitDailyRecordConfig.ProcessUnit.ShortName, targetUnitDailyRecordConfig.Name, dailyRecord.Value);

            }

            if (result.Count < 0)
            {
                return kernel.Get(typeof(IEfStatus)) as IEfStatus;
            }

            return this.data.SaveChanges(userName);
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
        /// Gets the unit datas for related daily data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfig">The target unit daily record config.</param>
        /// <param name="targetDay">The target day.</param>
        /// <returns></returns>
        private IEnumerable<UnitsDailyData> GetUnitDatasForRelatedDailyData(UnitDailyConfig targetUnitDailyRecordConfig, DateTime targetDay)
        {
            return targetUnitDailyRecordConfig.RelatedUnitDailyConfigs.OrderBy(x => x.Position)
                .Select(x => x.RelatedUnitsDailyConfig)
                .SelectMany(y => y.UnitsDailyDatas)
                    .Where(z => z.RecordTimestamp == targetDay);
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
        /// Gets the unit datas for daily data.
        /// </summary>
        /// <param name="targetUnitDailyRecordConfig">The target unit daily record config.</param>
        /// <param name="targetDay">The target day.</param>
        /// <returns></returns>
        private IEnumerable<IGrouping<int, UnitsData>> GetUnitDatasForDailyData(UnitDailyConfig targetUnitDailyRecordConfig, DateTime targetDay)
        {
            return targetUnitDailyRecordConfig.UnitConfigUnitDailyConfigs.OrderBy(x => x.Position)
                .Select(x => x.UnitConfig)
                .SelectMany(y => y.UnitsDatas)
                    .Where(z => z.RecordTimestamp == targetDay)
                    .GroupBy(w => (int)w.ShiftId);
        }

        /// <summary>
        /// Checks if all shifts are ready.
        /// </summary>
        /// <param name="unitDatas">The unit datas.</param>
        /// <returns></returns>
        private bool CheckIfAllShiftsAreReady(DateTime targetDate, int ProcessUnitId)
        {
            var currentApprovedUnitDatasCount = this.data.UnitsApprovedDatas.All().Where(x => x.ProcessUnitId == ProcessUnitId && x.RecordDate == targetDate).Count();
            var shiftsCount = this.data.Shifts.All().Count();

            if (currentApprovedUnitDatasCount != shiftsCount)
            {
                return false;
            }

            return true;
        }
    }
}
