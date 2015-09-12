using System;
using System.Collections.Generic;
using MathExpressions.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathExpressions.Tests
{
    [TestClass]
    public class FormulasTests
    {
        Calculator calculator = new Calculator();

        [TestMethod]
        public void EntalpiaFormulaTest()
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
            double actual = calculator.Calculate(expr,"par",2,inputParams);
            //Assert
            //.8044167846552613
            Assert.AreEqual(Convert.ToDecimal(expected),Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void CalculateTotally()
        {
            //Arrange
            double p01 = 13000.20;
            double p02 = 0.0;
            double p03 = 0.0;
            double p04 = 0.0;

            double p05 = 300.0;
            double p06 = 200.0;
            double p07 = 100.0;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("p01", p01);
            inputParams.Add("p02", p02);
            inputParams.Add("p03", p03);
            inputParams.Add("p04", p04);

            inputParams.Add("p05", p05);
            inputParams.Add("p06", p06);
            inputParams.Add("p07", p07);

            double expected = 626.0004;

            string expr = @"((par.p01 + par.p02 + par.p03 + par.p04) * 0.002) + par.p05 + par.p06 + par.p07";
            //Act
            double actual = calculator.Calculate(expr,"par", 7,inputParams);
            //Assert
            //626.0004
            Assert.AreEqual(Convert.ToDecimal(expected),Convert.ToDecimal(actual));
        }

    }
}

