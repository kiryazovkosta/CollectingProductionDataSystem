namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.Contracts;
    using Models.Productions;
    using Models.Productions.Mounthly;

    public interface IHistoricalService
    {
        void SetHistoricalProcessUnitParams(IEnumerable<IProcessUnitCangeable> entity, DateTime targetDate);
        void SetHistoricalProcessUnitParams(IEnumerable<IConfigable> entity, DateTime targetDate);
        IEnumerable<Factory> GetActualFactories(DateTime targetDate, int? factoryId = null);
        IEnumerable<ProcessUnit> GetActualProcessUnits(DateTime targetDate);
    }
}