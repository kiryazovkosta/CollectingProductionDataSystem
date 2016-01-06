using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectingProductionDataSystem.Application.UnitDailyDataServices
{
    class UnitDailyToApprove : IEquatable<UnitDailyToApprove>
    {
        public int ProcessUnitId { get; set; }

        public DateTime RecordDate { get; set; }
        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(UnitDailyToApprove other)
        {
            if (this.ProcessUnitId == other.ProcessUnitId && RecordDate.Date == other.RecordDate.Date)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}
