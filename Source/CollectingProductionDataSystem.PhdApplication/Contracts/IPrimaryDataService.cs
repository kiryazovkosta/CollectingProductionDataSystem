namespace CollectingProductionDataSystem.PhdApplication.Contracts
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions;

    public interface IPrimaryDataService
    {
        int ReadAndSaveUnitsDataForShift(DateTime beginDateTime, int offsetInHours, PrimaryDataSourceType dataSource);
    }
}
