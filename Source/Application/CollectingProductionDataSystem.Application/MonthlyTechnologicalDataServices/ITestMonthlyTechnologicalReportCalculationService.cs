namespace CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices
{
    using System.Collections.Concurrent;

    public interface ITestMonthlyTechnologicalReportCalculationService
    {
        ConcurrentDictionary<MonthlyTechnologicalReportCalculationIndicator, int> Dictionary { get; }
        bool EndCalculation(MonthlyTechnologicalReportCalculationIndicator calculation);
        bool TryBeginCalculation(MonthlyTechnologicalReportCalculationIndicator calculation);
    }
}
