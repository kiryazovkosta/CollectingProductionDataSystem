using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Application.UnitDailyDataServices;

namespace CollectingProductionDataSystem.Application.MonthlyServices
{
    public class TestUnitMonthlyCalculationService : CollectingProductionDataSystem.Application.Contracts.ITestUnitMonthlyCalculationService
    {
        private static ConcurrentDictionary<UnitMonthlyCalculationIndicator, int> processUnitInCalculation =
            new ConcurrentDictionary<UnitMonthlyCalculationIndicator, int>(new UnitMonthlyCalculationIndicatorComparer());
        private static object myLock = new object();
        private static volatile TestUnitMonthlyCalculationService testUnitMonthlyCalculationService = null;

        private TestUnitMonthlyCalculationService()
        {
        }

        public ConcurrentDictionary<UnitMonthlyCalculationIndicator, int> Dictionary { get { return processUnitInCalculation; } }

        public static TestUnitMonthlyCalculationService GetInstance()
        {
            if (testUnitMonthlyCalculationService == null)
            { // 1st check
                lock (myLock)
                {// 2nd check
                    if (testUnitMonthlyCalculationService == null)
                    {
                        testUnitMonthlyCalculationService = new TestUnitMonthlyCalculationService();
                    }
                }
            }

            return testUnitMonthlyCalculationService;
        }

        public bool TryBeginCalculation(UnitMonthlyCalculationIndicator calculation)
        {
            return processUnitInCalculation.TryAdd(calculation, 1);
        }

        public bool EndCalculation(UnitMonthlyCalculationIndicator calculation)
        {
            int value;
            return processUnitInCalculation.TryRemove(calculation, out value);
        }
    }
}
