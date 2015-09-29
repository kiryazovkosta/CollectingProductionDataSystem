using System.Data.Entity;
using CollectingProductionDataSystem.Data;
using CollectingProductionDataSystem.Data.Concrete;
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
            string str = "test;;;;;;;;;;;;;";
            string[] split = str.Split(new[] { ";" },2, StringSplitOptions.None);//(new[]{";"},StringSplitOptions.None);
            for (int i = 0; i < split.Count(); i++)
            {
                Console.WriteLine("{0} => {1}", i, split[i]);
            }
            Console.WriteLine("---------------------");
            Console.WriteLine("split members count is {0}.", split.Count());
            //var uow = new ProductionData(new CollectingDataSystemDbContext());
            //TreeShiftsReports(uow);
            //SeedShiftsToDatabase(uow);
        }

        private static void SeedShiftsToDatabase(ProductionData uow)
        {
            var shifts = uow.ProductionShifts.All().ToArray();

            var timer = new Stopwatch();

            timer.Start();

            // need to use some kind of structure to store shifts begin and end timestamps
            // TODO: Remove AddDays(-1)
            var shift1BeginTimestamp = DateTime.Today.AddMinutes(shifts[0].BeginMinutes);
            var shift1EndTimestamp = DateTime.Today.AddMinutes(shifts[0].BeginMinutes + shifts[0].OffsetMinutes);

            var shift2BeginTimestamp = DateTime.Today.AddMinutes(shifts[1].BeginMinutes);
            var shift2EndTimestamp = DateTime.Today.AddMinutes(shifts[1].BeginMinutes + shifts[1].OffsetMinutes);

            var shift3BeginTimestamp = DateTime.Today.AddMinutes(shifts[2].BeginMinutes);
            var shift3EndTimestamp = DateTime.Today.AddMinutes(shifts[2].BeginMinutes + shifts[2].OffsetMinutes);

            uow.DbContext.DbContext.Configuration.AutoDetectChangesEnabled = false;
            var unitDatas = uow.UnitsData.All().Where(x => x.RecordTimestamp < new DateTime(2015, 9, 15, 5, 30, 0)).ToList();

            (unitDatas.Where(x => x.RecordTimestamp.Between(shift1BeginTimestamp, shift1EndTimestamp)).ToList() as ICollection<UnitsData>)
                .ForEach(x =>{ x.ShiftId = ShiftType.First; x.RecordTimestamp = x.RecordTimestamp.Date;})
                .ForEach(x =>
                {
                    var ent = uow.DbContext.Entry<UnitsData>(x);
                    if (ent.State == EntityState.Detached)
                    {
                        uow.DbContext.DbContext.Set<UnitsData>().Attach(x);
                    }
                    ent.State = EntityState.Modified;
                });

            (unitDatas.Where(x => x.RecordTimestamp.Between(shift2BeginTimestamp, shift2EndTimestamp)).ToList() as ICollection<UnitsData>)
                .ForEach(x => { x.ShiftId = ShiftType.Second; x.RecordTimestamp = x.RecordTimestamp.Date; })
                 .ForEach(x =>
                 {
                     var ent = uow.DbContext.Entry<UnitsData>(x);
                     if (ent.State == EntityState.Detached)
                     {
                         uow.DbContext.DbContext.Set<UnitsData>().Attach(x);
                     }
                     ent.State = EntityState.Modified;
                 });

            (unitDatas.Where(x => x.RecordTimestamp.Between(shift3BeginTimestamp, shift3EndTimestamp)).ToList() as ICollection<UnitsData>)
                .ForEach(x => { x.ShiftId = ShiftType.Third; x.RecordTimestamp = x.RecordTimestamp.AddDays(-1).Date; })
                 .ForEach(x =>
                 {
                     var ent = uow.DbContext.Entry<UnitsData>(x);
                     if (ent.State == EntityState.Detached)
                     {
                         uow.DbContext.DbContext.Set<UnitsData>().Attach(x);
                     }
                     ent.State = EntityState.Modified;
                 });
            uow.DbContext.DbContext.Configuration.AutoDetectChangesEnabled = false;
            uow.SaveChanges("Initial Loading");

        }

        private static void TreeShiftsReports(ProductionData uow)
        {
            var shifts = uow.ProductionShifts.All().ToArray();

            var timer = new Stopwatch();

            timer.Start();

            // need to use some kind of structure to store shifts begin and end timestamps
            // TODO: Remove AddDays(-1)
            var shift1BeginTimestamp = DateTime.Today.AddMinutes(shifts[0].BeginMinutes);
            var shift1EndTimestamp = DateTime.Today.AddMinutes(shifts[0].BeginMinutes + shifts[0].OffsetMinutes);

            var shift2BeginTimestamp = DateTime.Today.AddMinutes(shifts[1].BeginMinutes);
            var shift2EndTimestamp = DateTime.Today.AddMinutes(shifts[1].BeginMinutes + shifts[1].OffsetMinutes);

            var shift3BeginTimestamp = DateTime.Today.AddMinutes(shifts[2].BeginMinutes);
            var shift3EndTimestamp = DateTime.Today.AddMinutes(shifts[2].BeginMinutes + shifts[2].OffsetMinutes);

            var data = uow.UnitsData.All()
                .Include(x => x.UnitConfig)
                .Where(x => x.UnitConfig.ProcessUnitId == 1 &&
                    x.RecordTimestamp > shift1BeginTimestamp &&
                    x.RecordTimestamp < shift3EndTimestamp)
                .ToList();

            var result = data.Select(x => new MultiShift
            {
                TimeStamp = x.RecordTimestamp,
                UnitConfigId = x.UnitConfigId,
                Shift1 = data.Where(y => y.RecordTimestamp > shift1BeginTimestamp & y.RecordTimestamp < shift1EndTimestamp).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                Shift2 = data.Where(y => y.RecordTimestamp > shift2BeginTimestamp & y.RecordTimestamp < shift2EndTimestamp).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                Shift3 = data.Where(y => y.RecordTimestamp > shift3BeginTimestamp & y.RecordTimestamp < shift3EndTimestamp).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
            }).Distinct(new MultiShiftComparer()).ToList();

            Console.WriteLine("Estimated Time For Action: {0}", timer.Elapsed);

            foreach (var item in result)
            {
                Console.WriteLine("{0} {1} {2}", item.Shift1, item.Shift2, item.Shift3);
            }

            Console.WriteLine("Estimated Time For Action: {0}", timer.Elapsed);
            timer.Stop();
        }
    }

    public class MultiShift
    {
        public DateTime TimeStamp { get; set; }
        public int UnitConfigId { get; set; }
        public UnitsData Shift1 { get; set; }
        public UnitsData Shift2 { get; set; }
        public UnitsData Shift3 { get; set; }
    }

    public class MultiShiftComparer : IEqualityComparer<MultiShift>
    {
        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(MultiShift x, MultiShift y)
        {
            return this.GetHashCode(x) == this.GetHashCode(y);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public int GetHashCode(MultiShift obj)
        {
            return (new { obj.UnitConfigId }).GetHashCode();
        }
    }

}
