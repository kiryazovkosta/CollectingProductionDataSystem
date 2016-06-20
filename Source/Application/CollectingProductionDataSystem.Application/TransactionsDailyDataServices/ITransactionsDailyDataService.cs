namespace CollectingProductionDataSystem.Application.TransactionsDailyDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Transactions;

    public interface ITransactionsDailyDataService
    {
        HashSet<MeasuringPointsConfigsReportData> ReadTransactionsDailyData(DateTime date, int flowDirection);
    }
}
