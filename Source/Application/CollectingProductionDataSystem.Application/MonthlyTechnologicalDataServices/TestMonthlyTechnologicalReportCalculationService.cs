namespace CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices
{
    using System.Collections.Concurrent;

    public class TestMonthlyTechnologicalReportCalculationService : ITestMonthlyTechnologicalReportCalculationService
    {
        private static readonly ConcurrentDictionary<MonthlyTechnologicalReportCalculationIndicator, int> monthsInCalculation =
            new ConcurrentDictionary<MonthlyTechnologicalReportCalculationIndicator, int>(new MonthlyTechnologicalReportCalculationIndicatorComparer());
        private static readonly object myLock = new object();
        private static volatile TestMonthlyTechnologicalReportCalculationService testMonthlyTechnologicalReportCalculationService = null;

        private TestMonthlyTechnologicalReportCalculationService()
        {
        }

        public ConcurrentDictionary<MonthlyTechnologicalReportCalculationIndicator, int> Dictionary { get { return monthsInCalculation; } }

        public static TestMonthlyTechnologicalReportCalculationService GetInstance()
        {
            if (testMonthlyTechnologicalReportCalculationService == null)
            { // 1st check
                lock (myLock)
                {// 2nd check
                    if (testMonthlyTechnologicalReportCalculationService == null)
                    {
                        testMonthlyTechnologicalReportCalculationService = new TestMonthlyTechnologicalReportCalculationService();
                    }
                }
            }

            return testMonthlyTechnologicalReportCalculationService;
        }

        public bool TryBeginCalculation(MonthlyTechnologicalReportCalculationIndicator calculation)
        {
            return monthsInCalculation.TryAdd(calculation, value: 1);
        }

        public bool EndCalculation(MonthlyTechnologicalReportCalculationIndicator calculation)
        {
            int value;
            return monthsInCalculation.TryRemove(calculation, out value);
        }
    }
}
