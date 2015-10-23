using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollectingProductionDataSystem.Application.UnitDailyDataServices;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BufferWidth = 200;
            Console.WindowWidth = 200;
            var ninject = new NinjectConfig();
            //var timer = new Stopwatch();
            //timer.Start();
            var kernel = ninject.Kernel;

            var service = kernel.GetService(typeof(UnitDailyDataService)) as UnitDailyDataService;
            var processUnitId = 1;
            var result = service.CalculateAndSaveDailyData(processUnitId, DateTime.Today.AddDays(-4), "Test");
        }
    }
}
