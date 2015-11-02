namespace CollectingProductionDataSystem.PhdApplication.Contracts
{
    using System;
    using System.Linq;

    public interface IPrimaryDataService
    {
        int ReadAndSaveUnitsDataForShift(DateTime beginDateTime, int offsetInHours);
    }
}
