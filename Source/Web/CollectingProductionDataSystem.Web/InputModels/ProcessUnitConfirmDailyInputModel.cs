namespace CollectingProductionDataSystem.Web.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ProcessUnitConfirmDailyInputModel
    {
        public DateTime date { get; set; }
        public int processUnitId { get; set; }
        public bool IsConfirmed { get; set; }
        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(ProcessUnitConfirmShiftInputModel other)
        {
            return this.date == other.date
                && this.processUnitId == other.processUnitId
                && this.IsConfirmed == other.IsConfirmed;
        }
    }
}