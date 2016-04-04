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
        IEnumerable<UnitsDailyData> CalculateDailyDataForProcessUnit(int processUnitId, DateTime targetDay, bool isRecalculate=false, int editReasonId = 0, int materialTypeId = 0);
        bool CheckIfAllShiftsAreReady(DateTime targetDate, int processUnitId);
        bool CheckIfDayIsApproved(DateTime targetDate, int processUnitId);
        bool CheckExistsUnitDailyDatas(DateTime targetDate, int processUnitId, int materialTypeId);
        IEfStatus ClearUnitDailyDatas(DateTime targetDate, int processUnitId, string userName);
        IEfStatus CheckIfShiftsAreReady(DateTime targetDate, int processUnitId);
        IEfStatus CheckIfPreviousDaysAreReady(int processUnitId, DateTime targetDate, int materialTypeId);
        ChartViewModel<DateTime, decimal> GetStatisticForProcessUnit(int processUnitId, DateTime targetDate, int? materialTypeId = null);

        Task<ChartViewModel<DateTime, decimal>> GetStatisticForProcessUnitAsync(int processUnitId, DateTime beginDate, DateTime endDate, int? materialTypeId = null);

        IEfStatus CheckIfDayIsApprovedButEnergyNot(DateTime date, int processUnitId, out bool readyForCalculation);

    }
}
