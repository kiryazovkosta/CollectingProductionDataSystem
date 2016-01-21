using System;
using System.Collections.Concurrent;
using CollectingProductionDataSystem.Application.UnitDailyDataServices;
using CollectingProductionDataSystem.Data.Contracts;

namespace CollectingProductionDataSystem.Application.Contracts
{
    public interface ITestUnitDailyCalculationService
    {
        ConcurrentDictionary<UnitDailyCalculationIndicator,int> Dictionary { get;}
        bool EndCalculation(UnitDailyCalculationIndicator calculation);
        bool TryBeginCalculation(UnitDailyCalculationIndicator calculation);
    }
}
