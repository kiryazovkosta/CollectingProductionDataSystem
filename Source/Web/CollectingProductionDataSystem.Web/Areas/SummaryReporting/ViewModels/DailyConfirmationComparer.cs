namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.ViewModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class DailyConfirmationComparer : IEqualityComparer<DailyConfirmationViewModel>
    {
        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(DailyConfirmationViewModel x, DailyConfirmationViewModel y)
        {
            return x.Day == y.Day;
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public int GetHashCode(DailyConfirmationViewModel obj)
        {
            return obj.Day.GetHashCode() + obj.IsConfirmed.GetHashCode();
        }
    }
}