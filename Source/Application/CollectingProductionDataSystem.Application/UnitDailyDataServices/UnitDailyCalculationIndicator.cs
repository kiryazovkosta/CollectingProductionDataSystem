/// <summary>
/// Summary description for UnitDailyCalculationIndicator
/// Class 
/// </summary>
namespace CollectingProductionDataSystem.Application.UnitDailyDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UnitDailyCalculationIndicator : IEquatable<UnitDailyCalculationIndicator>
    {
        public UnitDailyCalculationIndicator( DateTime dayParam, int processUnitId) 
        {
            this.Day = dayParam;
            this.ProcessUnitId = processUnitId;
        }
        public int ProcessUnitId { get; set; }

        public DateTime Day { get; set; }

        public bool Equals(UnitDailyCalculationIndicator other)
        {
            if (this.ProcessUnitId == other.ProcessUnitId && this.Day==other.Day)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class UnitDailyCalculationIndicatorComparer : IEqualityComparer<UnitDailyCalculationIndicator>
    {
        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(UnitDailyCalculationIndicator x, UnitDailyCalculationIndicator y)
        {
            if (x.ProcessUnitId == y.ProcessUnitId && x.Day==y.Day)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public int GetHashCode(UnitDailyCalculationIndicator obj)
        {
            return obj.ProcessUnitId ^ obj.Day.GetHashCode();
        }
    }
}
