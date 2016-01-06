using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using CollectingProductionDataSystem.Application.CalculatorService;
using CollectingProductionDataSystem.Application.Contracts;
using CollectingProductionDataSystem.Application.UnitDailyDataServices;
using CollectingProductionDataSystem.Data.Common;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Application.UnitsDataServices;
using CollectingProductionDataSystem.Data.Concrete;
using CollectingProductionDataSystem.Data;
using Ninject;

namespace CollectingProductionDataSystem.ConsoleTester
{
    class Program
    {
        private static IUnitDailyDataService dailyService;
        private static IProductionData data;
        private static TransactionOptions transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
        private static NinjectConfig ninject;
        private static IKernel kernel;

        static Program()
        {
            ninject = new NinjectConfig();
            kernel = ninject.Kernel;
            dailyService = new UnitDailyDataService(data, kernel, new CalculatorService());
            data = new ProductionData(new CollectingDataSystemDbContext());
        }

        static void Main(string[] args)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, transantionOption))
            {
                CalculateDailyDataIfNotAvailable(new DateTime(2015, 12, 4), 7);
            }
        }

        private static IEfStatus CalculateDailyDataIfNotAvailable(DateTime date, int processUnitId)
        {
            IEfStatus status = new EfStatus();
             dailyService = new UnitDailyDataService(data, kernel, new CalculatorService());
            if (!dailyService.CheckIfDayIsApproved(date, processUnitId)
                && !dailyService.CheckExistsUnitDailyDatas(date, processUnitId))
            {
                IEnumerable<UnitsDailyData> dailyResult = new List<UnitsDailyData>();
                status = dailyService.CheckIfShiftsAreReady(date, processUnitId);

                if (status.IsValid)
                {
                    if (status.IsValid)
                    {
                        status = IsRelatedDataExists(date, processUnitId);

                        if (status.IsValid)
                        {
                            dailyResult = dailyService.CalculateDailyDataForProcessUnit(processUnitId, date);

                            if (dailyResult.Count() > 0)
                            {
                                data.UnitsDailyDatas.BulkInsert(dailyResult, "Test");
                                status = data.SaveChanges("Test");
                            }
                        }
                    }
                }
            }

            return status;
        }

         private static IEfStatus IsRelatedDataExists(DateTime date, int processUnitId)
        {
            var validationResult = new List<ValidationResult>();

            var relatedDailyDatasFromOtherProcessUnits = data.UnitsDailyConfigs
                                                             .All()
                                                             .Include(x => x.ProcessUnit)
                                                             .Include(x => x.RelatedUnitDailyConfigs)
                                                             .Where(x => x.ProcessUnitId == processUnitId && x.AggregationCurrentLevel == true)
                                                             .SelectMany(y => y.RelatedUnitDailyConfigs)
                                                             .Where(z => z.RelatedUnitsDailyConfig.ProcessUnitId != processUnitId)
                                                             .ToList();

            foreach (var item in relatedDailyDatasFromOtherProcessUnits)
            {
                var relatedData = data.UnitsDailyDatas.All()
                                            .Where(u => u.RecordTimestamp == date
                                                    && u.UnitsDailyConfigId == item.RelatedUnitsDailyConfigId)
                                            .Any();

                if (!relatedData)
                {
                    validationResult.Add(new ValidationResult(
                        string.Format("Не са налични дневни данни за позиция: {0}", item.UnitsDailyConfig.Name)));
                }
            }

            var status = new EfStatus();

            if (validationResult.Count() > 0)
            {
                status.SetErrors(validationResult);
            }

            return status;
        }

    }
}
