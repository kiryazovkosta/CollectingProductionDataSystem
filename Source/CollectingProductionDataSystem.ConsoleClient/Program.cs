using System.Data.Entity;
using CollectingProductionDataSystem.Application.FileServices;
using CollectingProductionDataSystem.Data;
using CollectingProductionDataSystem.Data.Concrete;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Infrastructure.Extentions;

namespace CollectingProductionDataSystem.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var ninject = new NinjectConfig();
            //var timer = new Stopwatch();
            //timer.Start();
            var kernel = ninject.Kernel;

            var data = kernel.GetService(typeof(IProductionData)) as IProductionData;
            TransformUnitDailyConfigTable(data);
            TransformUnitConfigTable(data);
            //var fileName = @"d:\Proba\ХО-2-Конфигурация инсталации.csv";
            //var fileUploader = kernel.GetService(typeof(IFileUploadService)) as IFileUploadService;
            //timer.Stop();
            //Console.WriteLine("Time for ninject init {0}.", timer.Elapsed);
            //timer.Reset();
            //timer.Start();
            //var result = fileUploader.UploadFileToDatabase(fileName, ";");
            //timer.Stop();
            //if (result.IsValid)
            //{
            //    Console.WriteLine("File was uploaded successfully!!!");
            //    Console.WriteLine("Estimated time for action {0}", timer.Elapsed);
            //}
            //else
            //{
            //    result.EfErrors.ForEach(x =>
            //        Console.WriteLine("{0} => {1}", x.MemberNames.FirstOrDefault(), x.ErrorMessage)
            //        );
            //}
            //TreeShiftsReports(DateTime.Today.AddDays(-2), 1);
            //SeedShiftsToDatabase(uow);
        }
 
        /// <summary>
        /// Transforms the unit config table.
        /// </summary>
        /// <param name="data">The data.</param>
        private static void TransformUnitConfigTable(IProductionData data)
        {
            var records = data.UnitConfigs.All().Where(x => x.IsConverted == false);
            TransformRecords(records, data);
        }

        private static void TransformUnitDailyConfigTable(IProductionData data)
        {
            var records = data.UnitsDailyConfigs.All().Where(x => x.IsConverted == false);
            var recordsDependOnUnitConfig = records.Where(x => x.AggregationCurrentLevel == false);
            var recordsDependOnUnitDailyConfig = records.Where(x => x.AggregationCurrentLevel == true);
            TransformRecordsDependOnUnitConfig(recordsDependOnUnitConfig, data);
            TransformRecordsDependOnUnitDailyConfig(recordsDependOnUnitDailyConfig, data);
        }

        /// <summary>
        /// Transforms the records depend on unit daily config.
        /// </summary>
        /// <param name="recordsDependOnUnitDailyConfig">The records depend on unit daily config.</param>
        private static void TransformRecordsDependOnUnitDailyConfig(IQueryable<UnitDailyConfig> recordsDependOnUnitDailyConfig, IProductionData data)
        {
            foreach (var record in recordsDependOnUnitDailyConfig)
            {
                var depedentRecords = FindDependantRecords<UnitDailyConfig>(record.AggregationMembers, data);
                if (depedentRecords.Count() > 0)
                {
                    record.RelatedUnitDailyConfigs.Clear();
                    record.RelatedUnitDailyConfigs.AddRange(depedentRecords.Select((x, ix) =>
                        new RelatedUnitDailyConfigs
                        {
                            UnitsDailyConfigId = record.Id,
                            RelatedUnitsDailyConfigId = x.Id,
                            Position = ix + 1
                        }).ToList());
                    record.IsConverted = true;
                    record.ProductId = record.ProductId == 0 ? 1 : record.ProductId;

                    data.UnitsDailyConfigs.Update(record);
                }
            }

            data.SaveChanges("InitialLoading");
        }


        /// <summary>
        /// Transforms the records depend on unit config.
        /// </summary>
        /// <param name="recordsDependOnUnitConfig">The records depend on unit config.</param>
        private static void TransformRecordsDependOnUnitConfig(IQueryable<UnitDailyConfig> recordsDependOnUnitConfig, IProductionData data)
        {
            foreach (var record in recordsDependOnUnitConfig)
            {
                var depedentRecords = FindDependantRecords<UnitConfig>(record.AggregationMembers, data);
                if (depedentRecords.Count() > 0)
                {
                    record.UnitConfigUnitDailyConfigs.Clear();
                    record.UnitConfigUnitDailyConfigs.AddRange(depedentRecords.Select((x, ix) =>
                        new UnitConfigUnitDailyConfig
                        {
                            UnitDailyConfigId = record.Id,
                            UnitConfigId = x.Id,
                            Position = ix + 1
                        }).ToList());
                    record.IsConverted = true;
                    record.ProductId = record.ProductId == 0 ? 1 : record.ProductId;

                    data.UnitsDailyConfigs.Update(record);
                }
            }

            data.SaveChanges("InitialLoading");
        }

        /// <summary>
        /// Transforms the records.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <param name="data">The data.</param>
        private static void TransformRecords(IQueryable<UnitConfig> records, IProductionData data)
        {
            foreach (var record in records)
            {
                var depedentRecords = FindDependantRecords<UnitConfig>(record.AggregationMembers, data);
                if (depedentRecords.Count() > 0)
                {
                    record.UnitConfigUnitDailyConfigs.Clear();
                    record.UnitConfigUnitDailyConfigs.AddRange(depedentRecords.Select((x, ix) =>
                        new UnitConfigUnitDailyConfig
                        {
                            UnitDailyConfigId = record.Id,
                            UnitConfigId = x.Id,
                            Position = ix + 1
                        }).ToList());
                    record.IsConverted = true;
                    record.ProductId = record.ProductId == 0 ? 1 : record.ProductId;

                    data.UnitConfigs.Update(record);
                }
            }

            data.SaveChanges("InitialLoading");
        }

        /// <summary>
        /// Finds the dependant records.
        /// </summary>
        /// <param name="aggregationMembers">The aggregation members.</param>
        /// <returns></returns>
        private static IEnumerable<T> FindDependantRecords<T>(string aggregationMembers, IProductionData data)
            where T : class, IEntity, IAggregatable
        {

            var recordCodes = aggregationMembers.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            var records = data.DbContext.Set<T>().Where(x => recordCodes.Any(y => y == x.Code));

            return records;
        }

       


        //private static void SeedShiftsToDatabase(ProductionData uow, DateTime dateParam)
        //{
        //    var shifts = uow.ProductionShifts.All().ToArray();

        //    var timer = new Stopwatch();

        //    timer.Start();

        //    // need to use some kind of structure to store shifts begin and end timestamps
        //    // TODO: Remove AddDays(-1)
        //    //var shift1BeginTimestamp = DateTime.Today.AddMinutes(shifts[0].BeginMinutes);
        //    //var shift1EndTimestamp = DateTime.Today.AddMinutes(shifts[0].BeginMinutes + shifts[0].OffsetMinutes);

        //    //var shift2BeginTimestamp = DateTime.Today.AddMinutes(shifts[1].BeginMinutes);
        //    //var shift2EndTimestamp = DateTime.Today.AddMinutes(shifts[1].BeginMinutes + shifts[1].OffsetMinutes);

        //    //var shift3BeginTimestamp = DateTime.Today.AddMinutes(shifts[2].BeginMinutes);
        //    //var shift3EndTimestamp = DateTime.Today.AddMinutes(shifts[2].BeginMinutes + shifts[2].OffsetMinutes);

        //    uow.DbContext.DbContext.Configuration.AutoDetectChangesEnabled = false;
        //    var unitDatas = uow.UnitsData.All().Where(x => x.RecordTimestamp < new DateTime(2015, 9, 15, 5, 30, 0)).ToList();

        //    (unitDatas.Where(x => x.RecordTimestamp == dateParam)).ToList() as ICollection<UnitsData>)
        //        .ForEach(x => { x.ShiftId = ShiftType.First; x.RecordTimestamp = x.RecordTimestamp.Date; })
        //        .ForEach(x =>
        //        {
        //            var ent = uow.DbContext.Entry<UnitsData>(x);
        //            if (ent.State == EntityState.Detached)
        //            {
        //                uow.DbContext.DbContext.Set<UnitsData>().Attach(x);
        //            }
        //            ent.State = EntityState.Modified;
        //        });

        //    (unitDatas.Where(x => x.RecordTimestamp.Between(shift2BeginTimestamp, shift2EndTimestamp)).ToList() as ICollection<UnitsData>)
        //        .ForEach(x => { x.ShiftId = ShiftType.Second; x.RecordTimestamp = x.RecordTimestamp.Date; })
        //         .ForEach(x =>
        //         {
        //             var ent = uow.DbContext.Entry<UnitsData>(x);
        //             if (ent.State == EntityState.Detached)
        //             {
        //                 uow.DbContext.DbContext.Set<UnitsData>().Attach(x);
        //             }
        //             ent.State = EntityState.Modified;
        //         });

        //    (unitDatas.Where(x => x.RecordTimestamp.Between(shift3BeginTimestamp, shift3EndTimestamp)).ToList() as ICollection<UnitsData>)
        //        .ForEach(x => { x.ShiftId = ShiftType.Third; x.RecordTimestamp = x.RecordTimestamp.AddDays(-1).Date; })
        //         .ForEach(x =>
        //         {
        //             var ent = uow.DbContext.Entry<UnitsData>(x);
        //             if (ent.State == EntityState.Detached)
        //             {
        //                 uow.DbContext.DbContext.Set<UnitsData>().Attach(x);
        //             }
        //             ent.State = EntityState.Modified;
        //         });
        //    uow.DbContext.DbContext.Configuration.AutoDetectChangesEnabled = false;
        //    uow.SaveChanges("Initial Loading");

        //}

        private static void TreeShiftsReports(DateTime dateParam, int processUnitIdParam)
        {
            using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
            {
                var timer = new Stopwatch();
                timer.Start();
                var data = context.UnitsData.All()
                    .Include(x => x.UnitConfig)
                    .Where(x => x.UnitConfig.ProcessUnitId == processUnitIdParam && x.RecordTimestamp == dateParam)
                    .ToList();

                var result = data.Select(x => new MultiShift
                {
                    TimeStamp = x.RecordTimestamp,
                    Code = x.UnitConfig.Code,
                    Position = x.UnitConfig.Position,
                    UnitConfigId = x.UnitConfigId,
                    UnitName = x.UnitConfig.Name,
                    Shift1 = data.Where(y => y.RecordTimestamp == dateParam && y.ShiftId == ShiftType.First).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                    Shift2 = data.Where(y => y.RecordTimestamp == dateParam && y.ShiftId == ShiftType.Second).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                    Shift3 = data.Where(y => y.RecordTimestamp == dateParam && y.ShiftId == ShiftType.Third).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                }).Distinct(new MultiShiftComparer()).ToList();

                Console.WriteLine("Estimated Time For Action: {0}", timer.Elapsed);

                foreach (var item in result)
                {
                    Console.WriteLine("{0} {1} {2} {3} {4} {5}", item.TimeStamp, item.UnitName, item.Shift1, item.Shift2, item.Shift3.RealValue, item.TotalQuantityValue);
                }

                Console.WriteLine("Estimated Time For Action: {0}", timer.Elapsed);
                timer.Stop();
            }
        }
    }

}
