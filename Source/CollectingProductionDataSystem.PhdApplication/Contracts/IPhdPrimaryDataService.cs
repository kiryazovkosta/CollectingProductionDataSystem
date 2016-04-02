namespace CollectingProductionDataSystem.PhdApplication.Contracts
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;

    public interface IPhdPrimaryDataService:IDisposable
    {
        void ClearTemporaryData();
        void FinalizeShiftObservation(DateTime targetRecordTimestamp, Shift targetShift);
        Shift GetObservedShiftByDateTime(DateTime targetDateTime);
        Shift GetShiftById(int shiftId);
        void ProcessInventoryTanksData();
        bool ProcessPrimaryProductionData(CollectingProductionDataSystem.Models.Productions.PrimaryDataSourceType dataSource, DateTime targetRecordTimestamp, CollectingProductionDataSystem.Models.Nomenclatures.Shift shift, bool isForcedResultCalculation, CollectingProductionDataSystem.Enumerations.TreeState isFirstPhdInteraceCompleted);

        IEnumerable<Shift> GetShifts();

        IEnumerable<UnitDatasTemp> ProcessCalculatedUnits(List<UnitConfig> unitsConfigsList,
                                                                    DateTime recordDataTime,
                                                                    int shift, List<UnitDatasTemp> unitsTempData,
                                                                    ref int expectedNumberOfRecords);
    }
}
