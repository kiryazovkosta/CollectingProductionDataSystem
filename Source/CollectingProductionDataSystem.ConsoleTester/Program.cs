using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Transactions;
using CollectingProductionDataSystem.Application.CalculatorService;
using CollectingProductionDataSystem.Application.Contracts;
using CollectingProductionDataSystem.Application.MonthlyServices;
using CollectingProductionDataSystem.Application.UnitDailyDataServices;
using CollectingProductionDataSystem.Constants;
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
        private static ITestUnitDailyCalculationService testUnitDailyCalculationService;

        static Program()
        {
            ninject = new NinjectConfig();
            kernel = ninject.Kernel;
            dailyService = new UnitDailyDataService(data, kernel, new CalculatorService());
            data = new ProductionData(new CollectingDataSystemDbContext());
            testUnitDailyCalculationService = kernel.Get<ITestUnitDailyCalculationService>();
        }

        static void Main(string[] args)
        {
            ////using (var transaction = new TransactionScope(TransactionScopeOption.Required, transantionOption))
            ////{
            ////    CalculateDailyDataIfNotAvailable(new DateTime(2015, 12, 4), 7);
            ////}
            ////Thread thread1 = new Thread(new ThreadStart(Calculate));
            ////Thread thread2 = new Thread(new ThreadStart(Calculate));
            ////thread1.Start();
            ////thread2.Start();
            ////thread1.Join();
            ////thread2.Join();
            ////Console.WriteLine("CalculationOver");
            //var str = string.Format("Some formula * {0}", (2.34).ToString(System.Globalization.CultureInfo.InvariantCulture));
            //Console.WriteLine(str);
            var service = kernel.Get<UnitMothlyDataService>();
            var result = service.CalculateMonthlyDataForReportType(new DateTime(2016, 1, 1), false, 1);
            Console.WriteLine("Ready");
        }

        private static void Calculate() 
        {
             using (var transaction = new TransactionScope(TransactionScopeOption.Required, transantionOption))
            {
                CalculateDailyDataIfNotAvailable(new DateTime(2016, 1, 1), 5);
            }
        }

        private static IEfStatus CalculateDailyDataIfNotAvailable(DateTime date, int processUnitId)
        {
            IEfStatus status = new EfStatus();
            testUnitDailyCalculationService = kernel.Get<ITestUnitDailyCalculationService>();
             dailyService = new UnitDailyDataService(data, kernel, new CalculatorService());
            if //(!dailyService.CheckIfDayIsApproved(date, processUnitId)
            //    && !dailyService.CheckExistsUnitDailyDatas(date, processUnitId) 
            //    && 
                (testUnitDailyCalculationService.TryBeginCalculation(new UnitDailyCalculationIndicator(date, processUnitId)))
            {
                  Exception exc = null;
                try
                {
                    IEnumerable<UnitsDailyData> dailyResult = new List<UnitsDailyData>();
                    status = dailyService.CheckIfShiftsAreReady(date, processUnitId);

                    if (status.IsValid)
                    {
                       
                            status = dailyService.CheckIfPreviousDaysAreReady(processUnitId, date, CommonConstants.MaterialType);
                      

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
                catch (Exception ex)
                {
                    exc = ex;
                    //string str = exc.Message + "\nWe were here!!!" ;
                    ////foreach (var records in this.testUnitDailyCalculationService.Dictionary)
                    ////{
                    ////    str += records + "\n";
                    ////}

                    //exc = new InvalidOperationException(str,exc) ;
                }
                finally 
                {
                    int ix = 0;
                    while (!(testUnitDailyCalculationService.EndCalculation(new UnitDailyCalculationIndicator(date, processUnitId)) || ix == 10)) 
                    {
                        ix++;
                    }

                    if (ix >= 10)
                    {
                        string message = string.Format("Cannot clear record for begun Process Unit Calculation For ProcessUnitId {0} and Date {1}", processUnitId,date);
                        exc = new InvalidOperationException();
                    }

                    if (exc != null)
                    {
                        throw exc;   
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
