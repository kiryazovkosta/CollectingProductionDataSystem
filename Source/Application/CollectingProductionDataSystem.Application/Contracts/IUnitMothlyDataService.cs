namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using Ninject;
    using Resources = App_Resources;

    public interface IUnitMothlyDataService
    {
        /// <summary>
        /// Calculates the monthly data if not available.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="hydroCarbons">The hydro carbons.</param>
        /// <returns></returns>
        IEfStatus CalculateMonthlyDataIfNotAvailable(DateTime date, int reportTypeId, string UserName);

        /// <summary>
        /// Calculates the type of the monthly data for report.
        /// </summary>
        /// <param name="inTargetMonth">The in target month.</param>
        /// <param name="isRecalculate">The is recalculate.</param>
        /// <param name="ReportTypeId">The report type id.</param>
        /// <returns></returns>
        IEnumerable<UnitMonthlyData> CalculateMonthlyDataForReportType(DateTime inTargetMonth, bool isRecalculate, int reportTypeId, int changedMonthlyConfigId = 0);

        /// <summary>
        /// Checks if all shifts are ready.
        /// </summary>
        /// <param name="unitDatas">The unit datas.</param>
        /// <returns></returns>
        IEfStatus CheckIfDailyAreReady(DateTime targetDate, bool IsEnergy);

        /// <summary>
        /// Checks if day is approved.
        /// </summary>
        /// <param name="targetDate">The target date.</param>
        /// <param name="processUnitId">The process unit identifier.</param>
        /// <returns></returns>
        bool CheckIfMonthIsApproved(DateTime targetDate, int monthlyReportTypeId);

        /// <summary>
        /// Checks existing of the unit monthly data.
        /// </summary>
        /// <param name="targetDate">The target date.</param>
        /// <param name="monthlyReportTypeId">The monthly report type id.</param>
        /// <returns></returns>
        bool CheckExistsUnitMonthlyDatas(DateTime targetDate, int monthlyReportTypeId);

        /// <summary>
        /// Checks if previous days are ready.
        /// </summary>
        /// <param name="processUnitId">The process unit identifier.</param>
        /// <param name="targetDate">The target date.</param>
        /// <returns></returns>
        IEfStatus CheckIfPreviousDaysAreReady(int processUnitId, DateTime targetDate, int materialTypeId);

        IEfStatus CheckIfDayIsApprovedButEnergyNot(DateTime date, int processUnitId, out bool readyForCalculation);

        /// <summary>
        /// Gets the data for month.
        /// </summary>
        /// <param name="inTargetDate">The in target date.</param>
        /// <param name="monthlyReportTypeId">The monthly report type id.</param>
        /// <returns></returns>
        IEnumerable<UnitMonthlyData> GetDataForMonth(DateTime inTargetDate, int monthlyReportTypeId);

        /// <summary>
        /// Gets the target month.
        /// </summary>
        /// <param name="inTargetMonth">The in target month.</param>
        /// <returns></returns>
        DateTime GetTargetMonth(DateTime inTargetMonth);

        /// <summary>
        /// Determines whether monthly report of the specified type is confirmed.
        /// </summary>
        /// <param name="inTargetDate">The in target date.</param>
        /// <param name="monthlyReportTypeId">The monthly report type id.</param>
        /// <returns></returns>
        bool IsMonthlyReportConfirmed(DateTime inTargetDate, int monthlyReportTypeId);
    }
}