namespace CollectingProductionDataSystem.Application.MonthlyServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class UnitMonthlyCalculationIndicator : IEquatable<UnitMonthlyCalculationIndicator>
    {
        public UnitMonthlyCalculationIndicator(DateTime dayParam, int mothlyReportTypeId)
        {
            this.Month = this.GetTargetMonth(dayParam);
            this.MonthlyReportType = mothlyReportTypeId;
        }
        public int MonthlyReportType { get; set; }

        public DateTime Month { get; set; }

        public bool Equals(UnitMonthlyCalculationIndicator other)
        {
            if (this.MonthlyReportType == other.MonthlyReportType && this.Month == other.Month)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private DateTime GetTargetMonth(DateTime inTargetMonth)
        {
            DateTime date = inTargetMonth.Date;
            DateTime targetMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
#if DEBUG
            targetMonth = new DateTime(inTargetMonth.Year, 2, 2);
#endif
            return targetMonth;
        }
    }

    public class UnitMonthlyCalculationIndicatorComparer : IEqualityComparer<UnitMonthlyCalculationIndicator>
    {
        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(UnitMonthlyCalculationIndicator x, UnitMonthlyCalculationIndicator y)
        {
            if (x.MonthlyReportType == y.MonthlyReportType && x.Month == y.Month)
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
        public int GetHashCode(UnitMonthlyCalculationIndicator obj)
        {
            return obj.MonthlyReportType ^ obj.Month.GetHashCode();
        }
    }
}