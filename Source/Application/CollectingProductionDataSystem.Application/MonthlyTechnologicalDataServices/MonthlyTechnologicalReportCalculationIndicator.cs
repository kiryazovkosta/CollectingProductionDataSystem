namespace CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices
{
    using System;
    using System.Collections.Generic;

    public class MonthlyTechnologicalReportCalculationIndicator : IEquatable<MonthlyTechnologicalReportCalculationIndicator>
    {
        public MonthlyTechnologicalReportCalculationIndicator(DateTime monthParam)
        {
            this.Month = monthParam;
        }

        public DateTime Month { get; set; }

        public bool Equals(MonthlyTechnologicalReportCalculationIndicator other)
        {
            if (this.Month == other.Month)
            {
                return true;
            }

            return false;
        }
    }

    
}
