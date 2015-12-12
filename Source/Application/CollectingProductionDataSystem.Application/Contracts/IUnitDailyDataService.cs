using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Infrastructure.Chart;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Application.Contracts
{
    public interface IUnitDailyDataService
    {
        IEnumerable<UnitsDailyData> CalculateDailyDataForProcessUnit(int processUnitId, DateTime targetDay);
        bool CheckIfAllShiftsAreReady(DateTime targetDate, int processUnitId);
        bool CheckIfDayIsApproved(DateTime targetDate, int processUnitId);
        IEfStatus ClearUnitDailyDatas(DateTime targetDate, int processUnitId, string userName);
        IEfStatus CheckIfShiftsAreReady(DateTime targetDate, int processUnitId);
        IEfStatus CheckIfPreviousDaysAreReady(int processUnitId, DateTime targetDate);
        ChartViewModel<DateTime, decimal> GetStatisticForProcessUnit(int processUnitId, DateTime targetDate);

    }
}
