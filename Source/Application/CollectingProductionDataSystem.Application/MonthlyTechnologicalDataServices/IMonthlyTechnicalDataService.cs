namespace CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices
{
    using CollectingProductionDataSystem.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public interface IMonthlyTechnicalDataService
    {
        IEnumerable<MonthlyTechnicalReportDataDto> ReadMonthlyTechnologicalData(DateTime month, int[] processUnits);

        IEfStatus CheckIfAllMonthReportAreApproved(DateTime month);
    }
}
