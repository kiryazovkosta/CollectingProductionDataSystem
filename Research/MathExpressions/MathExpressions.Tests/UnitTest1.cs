using System;
using System.Collections.Generic;
using MathExpressions.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathExpressions.Tests
{
    [TestClass]
    public class UnitTest1
    {
        Calculator calculator = new Calculator();

        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            decimal t = 455M;
            decimal P = 20.4M;
            decimal S = 1323M;

                var inputParams = new Dictionary<string, decimal>();
                inputParams.Add("t", t);
                inputParams.Add("P", P);
                double expected = ((0.758195-(7.97826/(Math.Pow((0.001*(Convert.ToDouble(t)+273.15)),2)))-((3.078455*0.001*(Convert.ToDouble(t)+273.15)-0.21549)/(Math.Pow((0.001*(Convert.ToDouble(t)+273.15)-0.21),3)))*0.01*Convert.ToDouble(P))
                    + (0.0644126 - (0.268671 / (Math.Pow((0.001 * (Convert.ToDouble(t) + 273.15)), 8))) - (0.216661 / (Math.Pow((0.001 * (Convert.ToDouble(t) + 273.15)) , 14)) / 100)) * Math.Pow(((0.01 * Convert.ToDouble(P))), 2)
+(503.43+(11.02849*Math.Log10(((Convert.ToDouble(t)+273.15)/647.6)))+(229.2569*((Convert.ToDouble(t)+273.15)/647.6))+(37.93129*(Math.Pow((((Convert.ToDouble(t)+273.15)/647.6)),2)))))*0.001;

                string expr = "((0.758195-(7.97826/(Math.Pow((0.001*(Convert.ToDouble(par.t)+273.15)),2)))-((3.078455*0.001*(Convert.ToDouble(par.t)+273.15)-0.21549)/(Math.Pow((0.001*(Convert.ToDouble(par.t)+273.15)-0.21),3)))*0.01*Convert.ToDouble(par.P))+ (0.0644126 - (0.268671 / (Math.Pow((0.001 * (Convert.ToDouble(par.t) + 273.15)), 8))) - (0.216661 / (Math.Pow((0.001 * (Convert.ToDouble(par.t) + 273.15)) , 14)) / 100)) * Math.Pow(((0.01 * Convert.ToDouble(par.P))), 2)+(503.43+(11.02849*Math.Log10(((Convert.ToDouble(par.t)+273.15)/647.6)))+(229.2569*((Convert.ToDouble(par.t)+273.15)/647.6))+(37.93129*(Math.Pow((((Convert.ToDouble(par.t)+273.15)/647.6)),2)))))*0.001";
            //Act
                decimal actual = calculator.Calculate(expr,"par",2,inputParams);
            //Assert

            Assert.AreEqual(Convert.ToDecimal(expected),actual);
        }
    }
}
