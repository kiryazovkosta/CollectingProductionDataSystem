namespace CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMonthlyTechnicalDataService
    {
        IEnumerable<MonthlyTechnicalReportDataDto> CalculateMonthlyTechnologicalData(DateTime month);
    }
}
