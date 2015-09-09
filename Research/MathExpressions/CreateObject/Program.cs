﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathExpressions.Application;

namespace CreateObject
{
    class Program
    {
        static void Main(string[] args)
        {
            CalculatorDemo();
            PartialPrint(455, 20.4);
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
    }
}