namespace CollectingProductionDataSystem.PhdApplication.Contracts
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Enumerations;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;

    public interface IPrimaryDataService
    {
        int ReadAndSaveUnitsDataForShift(DateTime targetDate,
                                         Shift Shift,
                                         PrimaryDataSourceType dataSource,
                                         bool isForcedResultCalculation,
                                         ref bool lastOperationSucceeded,
                                         TreeState IsFirstPhdInteraceCompleted);
        Shift GetObservedShiftByDateTime(DateTime targetDateTime);
    }
}
