namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.Collections.Generic;
    using Models.Contracts;
    using Models.Productions.Mounthly;

    public interface IHistoricalService
    {
        void SetHistoricalProcessUnitParams(IEnumerable<IProcessUnitCangeable> entity, DateTime targetDate);
        void SetHistoricalProcessUnitParams(IEnumerable<IConfigable> entity, DateTime targetDate);
    }
}