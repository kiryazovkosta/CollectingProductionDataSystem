/// <summary>
/// Summary description for TestUnitDailyCalculationService
/// </summary>
namespace CollectingProductionDataSystem.Application.UnitDailyDataServices
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;

    public class TestUnitDailyCalculationService : ITestUnitDailyCalculationService
    {
        private static ConcurrentDictionary<UnitDailyCalculationIndicator,int> processUnitInCalculation = 
            new ConcurrentDictionary<UnitDailyCalculationIndicator,int>(new UnitDailyCalculationIndicatorComparer());
        private static object myLock = new object();
        private static volatile TestUnitDailyCalculationService testUnitDailyCalculationService = null;

        private TestUnitDailyCalculationService()
        {
        }
        public ConcurrentDictionary<UnitDailyCalculationIndicator,int> Dictionary { get{return processUnitInCalculation;} }

        public static TestUnitDailyCalculationService GetInstance()
        {
            if (testUnitDailyCalculationService == null)
            { // 1st check
                lock (myLock)
                {// 2nd check
                    if (testUnitDailyCalculationService == null)
                    {
                        testUnitDailyCalculationService = new TestUnitDailyCalculationService();
                    }
                }
            }

            return testUnitDailyCalculationService;
        }

        public bool TryBeginCalculation(UnitDailyCalculationIndicator calculation) 
        {
            return processUnitInCalculation.TryAdd(calculation, 1);
        }

        public bool EndCalculation(UnitDailyCalculationIndicator calculation)
        {
            int value;
            return processUnitInCalculation.TryRemove(calculation, out value);
        }
    }
}
