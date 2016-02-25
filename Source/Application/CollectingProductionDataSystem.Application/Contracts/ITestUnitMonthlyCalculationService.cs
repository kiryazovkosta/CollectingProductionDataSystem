namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.Collections.Concurrent;
    using CollectingProductionDataSystem.Application.MonthlyServices;
    using CollectingProductionDataSystem.Application.UnitDailyDataServices;

    public interface ITestUnitMonthlyCalculationService
    {
        ConcurrentDictionary<UnitMonthlyCalculationIndicator, int> Dictionary { get; }
        bool EndCalculation(UnitMonthlyCalculationIndicator calculation);
        bool TryBeginCalculation(UnitMonthlyCalculationIndicator calculation);
    }
}
