using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathExpressions.Application;

namespace CreateObject
{
    class Program
    {
        private static Calculator calculator = new Calculator();
        static void Main(string[] args)
        {
            //CalculatorDemo();
            //PartialPrint(455, 20.4);
            //PartialPrintTest(455, 20.4);
            PartEntalpiaFormulaTest();
        }

        private static void CalculatorDemo()
        {
            var calculator = new Calculator();
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("a", 2);
            inputParams.Add("b", 8);
            Console.WriteLine(calculator.Calculate("Math.Pow(par.a ,par.b)", "par", 2, inputParams));
        }

        /// <summary>
        /// Partials the print.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="p1">The p1.</param>
        private static void PartialPrint(double t, double P)
        {
            var fullResult =(((0.758195 - (7.97826 / Math.Pow((0.001 * (t + 273.15)), 2)) - ((3.078455 * 0.001 * (t + 273.15) - 0.21549) / Math.Pow(0.001 * (t + 273.15) - 0.21, 3))) * 0.01 * P)+
                            (0.0644126 - (0.268671 / Math.Pow(0.001 * (t + 273.15), 8)) - (0.216661 / Math.Pow(0.001 * (t + 273.15), 14) / 100)) * Math.Pow(0.01 * P, 2)+
                            (503.43 + (11.02849 * Math.Log(((t + 273.15) / 647.6))) + (229.2569 * ((t + 273.15)/647.6)) + (37.93129 * Math.Pow(((t + 273.15) / 647.6), 2))))*0.001;

            Console.WriteLine("Entalpia -> {0}", fullResult);
            

        }

        private static void PartialPrintTest(double t, double P)
        {
            var fullResult = Math.Log(t*P);

            Console.WriteLine("Formula -> {0}", fullResult);


        }

        public static void PartEntalpiaFormulaTest()
        {
            //Arrange
            double t = 455;
            double P = 20.4;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("t", t);
            inputParams.Add("P", P);
            double expected = 0.804416784655262;

            string expr = @"(((0.758195 - (7.97826 / Math.Pow((0.001 * (par.t + 273.15)), 2)) - ((3.078455 * 0.001 * (par.t + 273.15) - 0.21549) / Math.Pow(0.001 * (par.t + 273.15) - 0.21, 3))) * 0.01 * par.P)+
                            (0.0644126 - (0.268671 / Math.Pow(0.001 * (par.t + 273.15), 8)) - (0.216661 / Math.Pow(0.001 * (par.t + 273.15), 14) / 100)) * Math.Pow(0.01 * par.P, 2)+
                            (503.43 + (11.02849 * Math.Log(((par.t + 273.15) / 647.6))) + (229.2569 * ((par.t + 273.15)/647.6)) + (37.93129 * Math.Pow(((par.t + 273.15) / 647.6), 2))))*0.001";
            //Act
            var time = new Stopwatch();
            time.Start();
            double actual = calculator.Calculate(expr, "par", 2, inputParams);
            time.Stop();
            Console.WriteLine(actual);
            Console.WriteLine("{0}", time.Elapsed);
            //Assert
            //.8044167846552613
            //Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }
    }
}
