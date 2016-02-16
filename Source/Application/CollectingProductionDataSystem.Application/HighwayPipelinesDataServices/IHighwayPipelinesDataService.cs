
namespace CollectingProductionDataSystem.Application.HighwayPipelinesDataServices
{
    using CollectingProductionDataSystem.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IHighwayPipelinesDataService
    {
        IEfStatus CheckIfPreviousDaysAreReady(DateTime targetDate);

        IEnumerable<HighwayPipelineDto> ReadDataForDay(DateTime targetDay);
    }
}
