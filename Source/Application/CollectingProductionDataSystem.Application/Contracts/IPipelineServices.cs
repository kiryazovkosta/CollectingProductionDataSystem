namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Application.MonthlyServices;

    public interface IPipelineServices
    {
        IEnumerable<InnerPipelineDto> ReadDataForMonth(DateTime targetMonth);
    }
}
