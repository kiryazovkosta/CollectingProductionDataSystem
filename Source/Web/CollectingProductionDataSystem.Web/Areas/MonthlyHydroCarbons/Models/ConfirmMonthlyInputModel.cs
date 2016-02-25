using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyHydroCarbons.Models
{
    public class ConfirmMonthlyInputModel : IEquatable<ConfirmMonthlyInputModel>
    {
        public DateTime date { get; set; }
        public int monthlyReportTypeId { get; set; }
        public bool IsConfirmed { get; set; }
        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(ConfirmMonthlyInputModel other)
        {
            return this.date == other.date
                && this.monthlyReportTypeId == other.monthlyReportTypeId
                && this.IsConfirmed == other.IsConfirmed;
        }
    }
}
