namespace CollectingProductionDataSystem.Application.UnitsDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions;

    public interface IUnitsDataService
    {
        /// <summary>
        /// Gets the units data for daily record.
        /// </summary>
        /// <param name="unitsData">The units data.</param>
        /// <returns></returns>
        IEnumerable<UnitsData> GetUnitsDataForDailyRecord(int unitsData);

        IQueryable<UnitsData> GetUnitsDataForDateTime(DateTime? date, int? processUnitId, int? shiftId);

        IQueryable<UnitsDailyData> GetUnitsDailyDataForDateTime(DateTime? date, int? processUnitId);
    }
}