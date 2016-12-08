namespace CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices
{
    using System.Collections.Generic;

    public class MonthlyTechnologicalReportCalculationIndicatorComparer : IEqualityComparer<MonthlyTechnologicalReportCalculationIndicator>
    {
        public bool Equals(MonthlyTechnologicalReportCalculationIndicator x, MonthlyTechnologicalReportCalculationIndicator y)
        {
            if (x.Month == y.Month)
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(MonthlyTechnologicalReportCalculationIndicator obj)
        {
            return (obj.Month.Year * obj.Month.Month) ^ obj.Month.GetHashCode();
        }
    }
}
