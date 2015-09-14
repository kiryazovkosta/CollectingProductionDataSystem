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

namespace CollectingProductionDataSystem.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var uow = new ProductionData(new CollectingDataSystemDbContext());
            var shifts = uow.ProductionShifts.All().ToArray();

            var timer = new Stopwatch();

            timer.Start();

            // need to use some kind of structure to store shifts begin and end timestamps
            // TODO: Remove AddDays(-1)
            var shift1BeginTimestamp = DateTime.Today.AddDays(-1).AddMinutes(shifts[0].BeginMinutes);
            var shift1EndTimestamp = DateTime.Today.AddDays(-1).AddMinutes(shifts[0].BeginMinutes + shifts[0].OffsetMinutes);

            var shift2BeginTimestamp = DateTime.Today.AddDays(-1).AddMinutes(shifts[1].BeginMinutes);
            var shift2EndTimestamp = DateTime.Today.AddDays(-1).AddMinutes(shifts[1].BeginMinutes + shifts[1].OffsetMinutes);

            var shift3BeginTimestamp = DateTime.Today.AddDays(-1).AddMinutes(shifts[2].BeginMinutes);
            var shift3EndTimestamp = DateTime.Today.AddDays(-1).AddMinutes(shifts[2].BeginMinutes + shifts[2].OffsetMinutes);

            // need to use Include or better to use service. Also need to sord data
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
                Shift1 = data.Where(y => y.RecordTimestamp > shift1BeginTimestamp & y.RecordTimestamp < shift1EndTimestamp & y.UnitConfig.ProcessUnitId == 1).ToList(),
                Shift2 = data.Where(y => y.RecordTimestamp > shift2BeginTimestamp & y.RecordTimestamp < shift2EndTimestamp & y.UnitConfig.ProcessUnitId == 1).ToList(),
                Shift3 = data.Where(y => y.RecordTimestamp > shift3BeginTimestamp & y.RecordTimestamp < shift3EndTimestamp & y.UnitConfig.ProcessUnitId == 1).ToList()
            }).FirstOrDefault();

            if (result != null)
            {
                for (int i = 0; i < result.Shift1.Count; i++)
                {
                    // get manual and also check if there are shift data
                    var f = result.Shift1[i].Value;
                    var s = (result.Shift2.Count > 0) ? result.Shift2[i].Value : null;
                    var t = (result.Shift3.Count > 0) ? result.Shift3[i].Value : null;

                    Console.WriteLine("{0} {1} {2} {3}", result.TimeStamp, f, s, t);   
                }

            }
            timer.Stop();
            Console.WriteLine("Estimated Time For Action: {0}", timer.Elapsed);
        }
    }

    public class MultiShift
    {
        public DateTime TimeStamp { get; set; }
        public int UnitConfigId { get; set; }
        public IList<UnitsData> Shift1 { get; set; }
        public IList<UnitsData> Shift2 { get; set; }
        public IList<UnitsData> Shift3 { get; set; }
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
            return (new { TimeStamp = obj.TimeStamp, UnitConfigId = obj.UnitConfigId }).GetHashCode();
        }
    }

}
