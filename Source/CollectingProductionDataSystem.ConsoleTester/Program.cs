﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Web.ModelBinding;
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
using CollectingProductionDataSystem.Models.Productions.Mounthly;
using CollectingProductionDataSystem.Extentions;
using Ninject;
using CollectingProductionDataSystem.Infrastructure.Contracts;

namespace CollectingProductionDataSystem.ConsoleTester
{
    class Program
    {
        private static UnitDailyDataService dailyService;
        private static IProductionData data;
        private static readonly TransactionOptions transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
        private static NinjectConfig ninject;
        private static IKernel kernel;
        //private static UnitMothlyDataService service;
        private static ITestUnitDailyCalculationService testUnitDailyCalculationService;
        private static IUnitMothlyDataService monthlyService;

        static Program()
        {
            ninject = new NinjectConfig();
            kernel = ninject.Kernel;
            data = new ProductionData(new CollectingDataSystemDbContext());
            //new UnitDailyDataService(data, kernel, new CalculatorService());
            //dailyService = new UnitDailyDataService(data, kernel, new CalculatorService());
            //data = new ProductionData(new CollectingDataSystemDbContext());
            //testUnitDailyCalculationService = kernel.Get<ITestUnitDailyCalculationService>();
            //service = kernel.Get<UnitMothlyDataService>();
            monthlyService = new UnitMothlyDataService(data, kernel, new CalculatorService(kernel.Get<ILogger>()), TestUnitMonthlyCalculationService.GetInstance());
        }


        static void Main(string[] args)
        {

            var records = data.UnitMonthlyDatas.All().Include(x => x.UnitMonthlyConfig)
                .Where(x => x.RecordTimestamp == new DateTime(2016, 5, 31))
                .ToDictionary(x => x.UnitMonthlyConfig.Code);

            monthlyService.CalculateAnualAccumulation(ref records, new DateTime(2016, 5, 31));
            List<UnitMonthlyData> newRecords = new List<UnitMonthlyData>();
            foreach (var item in records.Values.Where(x => x.Id == 0))
            {
                newRecords.Add(item);
            }
            data.UnitMonthlyDatas.BulkInsert(newRecords, "Test");
            data.SaveChanges("Test");




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

            //dailyService.CheckExistsUnitDailyDatas(new DateTime(2016, 3, 6), 35, 1);

            //var timer = new Stopwatch();
            //var resultDaily = new Dictionary<string, UnitsDailyData>();

            ////service.GetDataForMonth(DateTime.Now,1);
            //timer.Start();
            //dailyService.AppendTotalMonthQuantityToDailyRecords(resultDaily, 35, new DateTime(2016, 3, 2),2);

            ////Update(service);
            //timer.Stop();
            //Console.WriteLine("Ready!\n Estimated time per operation {0}", timer.Elapsed);
            try
            {
                //var unitMothlyConfigs = data.UnitMonthlyConfigs.All()
                //                               .Include(x => x.UnitDailyConfigUnitMonthlyConfigs)
                //                               .Where(x => x.UnitDailyConfigUnitMonthlyConfigs.Count != 0)
                //                               .ToDictionary(x => string.Join("@", x.UnitDailyConfigUnitMonthlyConfigs.Select(y => y.UnitDailyConfig.Code).ToArray()));
                //var productionPlanConfigs = data.ProductionPlanConfigs.All().Where(x => x.Code != "4RMD0040" && x.Code != "4RMD0050" && !string.IsNullOrEmpty(x.QuantityFactMembers))
                //                                .ToDictionary(x => x.QuantityFactMembers, x => x.Id);
                ////IEnumerable<Data> productionPlanConfigs, unitMothlyConfigs;
                //using (var tran = new TransactionScope(TransactionScopeOption.Required, transantionOption))
                //{

                //    foreach (var key in unitMothlyConfigs.Keys)
                //    {
                //        if (productionPlanConfigs.ContainsKey(key))
                //        {
                //            Console.WriteLine(key);
                //            unitMothlyConfigs[key].ProductionPlanConfigId = productionPlanConfigs[key];
                //        }
                //    }

                //    data.SaveChanges("Georgiev.Georgi.V");

                //    tran.Complete();
                //}

                //WriteResultResultToFile(@"d:\result\unitMonthly.txt", unitMothlyConfigs);

                //WriteResultResultToFile(@"d:\result\productionPlan.txt", productionPlanConfigs);

                //var result = dailyService.GetStatisticForProcessUnitLoadAsync(29, new DateTime(2016, 4, 1), new DateTime(2016, 4, 3), 2).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();
        }

        private class Data
        {
            public string Code { get; set; }
            public string Members { get; set; }

            public override string ToString()
            {
                return string.Format("{0}|{1}", this.Code, this.Members);
            }
        }

        private static void WriteResultResultToFile(string fileName, IEnumerable<Data> collection)
        {
            //String fileName = "d:\\result.txt";

            try
            {

                using (StreamWriter file = new System.IO.StreamWriter(fileName, false, Encoding.GetEncoding("windows-1251")))
                {
                    foreach (var record in collection)
                    {
                        file.WriteLine(record.ToString());
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }

        private static void Update(UnitMothlyDataService monthlyService, string userName = "Test")
        {
            var ModelState = new ModelStateDictionary();
            decimal value = 5000M;

            var newManualRecord = new UnitManualMonthlyData
            {
                Id = 5,
                Value = value,
            };

            var existManualRecord = data.UnitManualMonthlyDatas.GetById(newManualRecord.Id);
            if (existManualRecord == null)
            {
                data.UnitManualMonthlyDatas.Add(newManualRecord);
            }
            else
            {
                UpdateRecord(existManualRecord, value);
            }
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, transantionOption))
                {
                    var result = data.SaveChanges(userName);
                    if (!result.IsValid)
                    {
                        foreach (ValidationResult error in result.EfErrors)
                        {
                            ModelState.AddModelError(error.MemberNames.ToList()[0], error.ErrorMessage);
                        }
                    }

                    var updatedRecords = monthlyService.CalculateMonthlyDataForReportType(
                        inTargetMonth: new DateTime(2016, 2, 2),
                        isRecalculate: true,
                        reportTypeId: 1,
                        changedMonthlyConfigId: 6
                        );
                    var status = UpdateResultRecords(updatedRecords, userName);

                    if (!status.IsValid)
                    {
                        status.ToModelStateErrors(ModelState);
                    }
                    else
                    {
                        //transaction.Complete();
                    }
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("ManualValue", "Записът не можа да бъде осъществен. Моля опитайте на ново!");
            }
        }

        private static IEfStatus UpdateResultRecords(IEnumerable<UnitMonthlyData> updatedRecords, string userName)
        {
            foreach (var record in updatedRecords)
            {
                var manualRecord = data.UnitManualMonthlyDatas.GetById(record.Id);
                if (manualRecord != null)
                {
                    manualRecord.Value = (decimal)record.RealValue;
                    data.UnitManualMonthlyDatas.Update(manualRecord);
                }
                else
                {
                    data.UnitManualMonthlyDatas.Add(new UnitManualMonthlyData { Id = record.Id, Value = (decimal)record.RealValue });
                }
            }
            var changesCount = data.DbContext.DbContext.ChangeTracker.Entries().Where(
                         e => (e.State == EntityState.Modified)).Count();
            return data.SaveChanges(userName);
        }

        private static void UpdateRecord(UnitManualMonthlyData existManualRecord, decimal value)
        {
            existManualRecord.Value = value;
            data.UnitManualMonthlyDatas.Update(existManualRecord);
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
            dailyService = new UnitDailyDataService(data, kernel, new CalculatorService(kernel.Get<ILogger>()));
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
                        string message = string.Format("Cannot clear record for begun Process Unit Calculation For ProcessUnitId {0} and Date {1}", processUnitId, date);
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
