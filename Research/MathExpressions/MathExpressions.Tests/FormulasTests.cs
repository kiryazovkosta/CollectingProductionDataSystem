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

        /**********************************************************/
        /***                                                    ***/
        /***    P A R T S of the Formulas                       ***/
        /***                                                    ***/
        /**********************************************************/

        [TestMethod]
        public void PartFormulaF()
        {
            //Arrange
            double d2 = 15;
            double pl = 20;
            double res = 2.5631944659559421;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("d2", d2);
            inputParams.Add("pl", pl);
            inputParams.Add("res", res);
            double expected = 7.689583397867826;

            string expr = @"((par.d2*par.pl*par.res)/100)";
            //Act
            double actual = calculator.Calculate(expr, "par", 3, inputParams);
            //Assert
            //7.6895833978678265
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaC()
        {
            //Arrange
            double d = 50;
            double al = 0.001163;
            double t = 90;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("d", d);
            inputParams.Add("al", al);
            inputParams.Add("t", t);
            double expected = 49.91859;

            string expr = @"(par.d-(par.al*(par.t-20)))";
            //Act
            double actual = calculator.Calculate(expr, "par", 3, inputParams);
            //Assert
            //49.91859
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaC1()
        {
            //Arrange
            double p = 20;
            double t = 4;
            double qpt3 = 0.9787;

            t = t + 273;
            p = p + 1;
            if ((p > 0) & (p < 4))
            {
                p = 1;
            }
            if ((p > 3) & (p < 8))
            {
                p = 5;
            }
            if ((p > 7) & (p < 13))
            {
                p = 10;
            }
            if ((p > 12) & (p < 18))
            {
                p = 15;
            }
            if (p > 17)
            {
                p = 20;
            }
            if (t > 343)
            {
                t = 343;
            }
            

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("qpt3", qpt3);
            double expected = 0.9787;

            string expr = @"(par.qpt3)";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //0.9787
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaA1()
        {
            //Arrange
            double p = 20;
            double t = 20;
            double d5 = 2;
            double d6 = 2;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("t", t);
            inputParams.Add("p", p);
            inputParams.Add("d5", d5);
            inputParams.Add("d6", d6);
            double expected = 2.5631944659559421;

            string expr = @"Math.Sqrt(((par.p+1)*(par.d6+273))/((par.d5+1)*(par.t+273)))";
            //Act
            double actual = calculator.Calculate(expr, "par", 4, inputParams);
            //Assert
            //2.563194465955942
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaA2()
        {
            //Arrange
            double p = 15;
            double t = 40;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double df = 0;
            double d = 50;
            double al = 0.001163;
            df = GetValueFormulaC(t, d, al);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("t", t);
            inputParams.Add("p", p);
            inputParams.Add("d4", d4);
            inputParams.Add("d5", d5);
            inputParams.Add("d6", d6);
            inputParams.Add("df", df);
            double expected = 3.873394225260469;

            string expr = @"Math.Sqrt(((par.p+1)*(par.d6+273)*par.df)/((par.d5+1)*(par.t+273)*par.d4))";
            //Act
            double actual = calculator.Calculate(expr, "par", 6, inputParams);
            //Assert
            //3.8733942252604692
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaA3()
        {
            //Arrange
            double t = 40;
            double d4 = 5;
            double d6 = 50;
            double df = 20;
            
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("t", t);
            inputParams.Add("d4", d4);
            inputParams.Add("d6", d6);
            inputParams.Add("df", df);
            double expected = 2.0316976958092337;

            string expr = @"Math.Sqrt(((par.d6+273)*par.df)/((par.t+273)*par.d4))";
            //Act
            double actual = calculator.Calculate(expr, "par", 4, inputParams);
            //Assert
            //2.0316976958092337
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaA4()
        {
            //Arrange
            double p = 15;
            double d5 = 10;
            
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("p", p);
            inputParams.Add("d5", d5);
            double expected = 1.2060453783110544;

            string expr = @"Math.Sqrt((par.p+1)/(par.d5+1))";
            //Act
            double actual = calculator.Calculate(expr, "par", 2, inputParams);
            //Assert
            //1.2060453783110545
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaA7()
        {
            //Arrange
            double p = 15;
            double t = 40;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double df = 0;
            double d = 50;
            double al = 0.001163;
            df = GetValueFormulaC(t, d, al);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("t", t);
            inputParams.Add("p", p);
            inputParams.Add("d4", d4);
            inputParams.Add("d5", d5);
            inputParams.Add("d6", d6);
            inputParams.Add("d", d);
            double expected = 0.38742954912211515;
            
            string expr = @"Math.Sqrt(((par.p+1)*(par.d6+273)*par.d4)/((par.d5+1)*(par.t+273)*par.d))";
            //Act
            double actual = calculator.Calculate(expr, "par", 6, inputParams);
            //Assert
            //0.38742954912211519
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaA10()
        {
            //Arrange
            double pl = 20;
            double d2 = 15;
            
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("pl", pl);
            inputParams.Add("d2", d2);
            double expected = 3;

            string expr = @"((par.d2*par.pl)/100)";
            //Act
            double actual = calculator.Calculate(expr, "par", 2, inputParams);
            //Assert
            //3.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaA11()
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double d2 = 15;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("pl", pl);
            inputParams.Add("pl1", pl1);
            inputParams.Add("d2", d2);
            double expected = 150;

            string expr = @"(par.d2*(par.pl-par.pl1))";
            //Act
            double actual = calculator.Calculate(expr, "par", 3, inputParams);
            //Assert
            //150.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaA15()
        {
            //Arrange
            double t = 40;
            double d4 = 5;
            double d6 = 50;
            double df = 20;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("t", t);
            inputParams.Add("d4", d4);
            inputParams.Add("d6", d6);
            inputParams.Add("df", df);
            double expected = 0.5079244239523084;

            string expr = @"Math.Sqrt(((par.d6+273)*par.d4)/((par.t+273)*par.df))";
            //Act
            double actual = calculator.Calculate(expr, "par", 4, inputParams);
            //Assert
            //0.50792442395230841
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }
                                
        [TestMethod]
        public void PartFormulaK15()
        {
            //Arrange
            double p = 10;
            double t = 50;
            double pw = 0;
            double pw1 = 0;
            t = t + 273.15;
            pw = Math.Pow((0.001 * t), 2);
            pw1 = Math.Pow(((0.001 * t) - 0.21), 3);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("p", p);
            inputParams.Add("t", t);
            inputParams.Add("pw", pw);
            inputParams.Add("pw1", pw1);
            double expected = -61.360061149207226;

            string expr = @"((0.758195 - (7.97826 / par.pw) - ((3.078455 * 0.001 * par.t - 0.21549) / par.pw1)) * 0.01 * par.p)";
            t = t - 273.15;            
            //Act
            double actual = calculator.Calculate(expr, "par", 4, inputParams);
            //Assert
            //-61.360061149207226
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaK2()
        {
            //Arrange
            double p = 10;
            double t = 50;
            double pt = 0;
            double pr1 = 0;
            double pr2 = 0;
            t = t + 273.15;
            pt = Math.Pow((0.001 * t), 8);
            pr1 = Math.Pow((0.001 * t), 14);
            pr2 = Math.Pow((0.01 * p), 2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("p", p);
            inputParams.Add("t", t);
            inputParams.Add("pt", pt);
            inputParams.Add("pr1", pr1);
            inputParams.Add("pr2", pr2);
            double expected = -182.59399108211345;

            string expr = @"(0.0644126 - (0.268671 / par.pt) - (0.216661 / par.pr1 / 100)) * par.pr2";
            //Act
            double actual = calculator.Calculate(expr, "par", 5, inputParams);
            //Assert
            //-182.59399108211346
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaK3()
        {
            //Arrange
            double t = 50;
            double j = 0;
            double j1 = 0;
            double w3 = 0;
            t = t + 273.15;
            j = t / 647.6;
            j1 = Math.Log(j);
            w3 = Math.Pow(j, 2);            

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("t", t);
            inputParams.Add("j", j);
            inputParams.Add("j1", j1);
            inputParams.Add("w3", w3);
            double expected = 619.60660460297342;

            string expr = @"(503.43 + (11.02849 * par.j1) + (229.2569 * par.j) + (37.93129 * par.w3))";
            //Act
            double actual = calculator.Calculate(expr, "par", 4, inputParams);
            //Assert
            //619.60660460297345
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartFormulaEN()
        {
            //Arrange
            double t = 50;
            double p = 10;
            double k15 = 0;
            double k2 = 0;
            double k3 = 0;
            k15 = GetValueFormulaK15(t, p);
            k2 = GetValueFormulaK2(t, p);
            k3 = GetValueFormulaK3(t);
            
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("k15", k15);
            inputParams.Add("k2", k2);
            inputParams.Add("k3", k3);
            double expected = 0.37565255237165275;

            string expr = @"(par.k15 + par.k2 + par.k3) * 0.001";
            //Act
            double actual = calculator.Calculate(expr, "par", 3, inputParams);
            //Assert
            //0.37565255237165274
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void PartEntalpiaFormulaTest()
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
        public void PartMyFormulaTest()
        {
            //Arrange
            double t = 455;
            double P = 20.4;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("t", t);
            inputParams.Add("P", P);
            double expected = 9.13583231980112;

            string expr = @"Math.Log(par.t*par.P)";
            //Act
            double actual = calculator.Calculate(expr, "par", 2, inputParams);
            //Assert
            //.8044167846552613
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));            
        }

        /**********************************************************/
        /***                                                    ***/
        /***    F O R M U L A S                                 ***/
        /***                                                    ***/
        /**********************************************************/

        [TestMethod]
        public void FormulaP1()             // 1) P1 ;ПАРА-КОНСУМИРАНА [ТОНОВЕ] :: X A1,F Q
        {
            //Arrange
            double p = 20;
            double t = 20;
            double d2 = 15;
            double pl = 20;            
            double d5 = 2;
            double d6 = 2;
            double a1 = GetValueFormulaA1(p, t, d5, d6);
            double f = GetValueFormulaF(d2, pl, a1);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 7.689583397867826;
            
            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //7.6895833978678265
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }
                
        [TestMethod]
        public void FormulaPP1()            // 2) PP1 ;ПАРА-ПРОИЗВЕДЕНА [ТОНОВЕ] :: X A1,F Q
        {
            //Arrange
            double p = 20;
            double t = 20;
            double d2 = 15;
            double pl = 20;
            double d5 = 2;
            double d6 = 2;
            double a1 = GetValueFormulaA1(p, t, d5, d6);
            double f = GetValueFormulaF(d2, pl, a1);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 7.689583397867826;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //7.6895833978678265
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaZ1()             // 3) Z1 ;ПАРА-КОНСУМИРАНА [MWH] :: X A1,F,K15,K2,K3,EN S Q=Q*ENT/0.860 Q
        {
            //Arrange
            double p = 10;
            double t = 50;
            double pl = 20;
            double d2 = 15;
            double d5 = 2;
            double d6 = 3;
            double a1 = GetValueFormulaA1(p, t, d5, d6);
            double f = GetValueFormulaF(d2, pl, a1);
            double ent = GetValueFormulaEN(t, p);            

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            inputParams.Add("ent", ent);
            double expected = 2.3195201121528819;

            string expr = @"(par.f*par.ent)/0.860";
            //Act
            double actual = calculator.Calculate(expr, "par", 2, inputParams);
            //Assert
            //2.3195201121528819
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaZZ1()             // 4) ZZ1 ;ПАРА-ПРОИЗВЕДЕНА [MWH] :: X A1,F,K15,K2,K3,EN S Q=Q*ENT/0.860 Q
        {
            //Arrange
            double p = 10;
            double t = 50;
            double pl = 20;
            double d2 = 15;
            double d5 = 2;
            double d6 = 3;
            double a1 = GetValueFormulaA1(p, t, d5, d6);
            double f = GetValueFormulaF(d2, pl, a1);
            double ent = GetValueFormulaEN(t, p);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            inputParams.Add("ent", ent);
            double expected = 2.3195201121528819;

            string expr = @"(par.f*par.ent)/0.860";
            //Act
            double actual = calculator.Calculate(expr, "par", 2, inputParams);
            //Assert
            //2.3195201121528819
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaZ2()             // 5) Z2 ;ПАРА-КОНСУМИРАНА, АСУТП, [MWH] :: X K15,K2,K3,EN S Q=PL*ENT/0.860 Q
        {
            //Arrange
            double p = 10;
            double t = 50;
            double pl = 20;
            double ent = GetValueFormulaEN(t, p);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("pl", pl);
            inputParams.Add("ent", ent);
            double expected = 8.736105869108203;

            string expr = @"(par.pl*par.ent)/0.860";
            //Act
            double actual = calculator.Calculate(expr, "par", 2, inputParams);
            //Assert
            //8.7361058691082025
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaZZ2()             // 6) ZZ2 ;ПАРА-ПРОИЗВЕДЕНА, АСУТП, [MWH] :: X K15,K2,K3,EN S Q=PL*ENT/0.860 Q
        {
            //Arrange
            double p = 10;
            double t = 50;
            double pl = 20;
            double ent = GetValueFormulaEN(t, p);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("pl", pl);
            inputParams.Add("ent", ent);
            double expected = 8.736105869108203;

            string expr = @"(par.pl*par.ent)/0.860";
            //Act
            double actual = calculator.Calculate(expr, "par", 2, inputParams);
            //Assert
            //8.7361058691082025
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaN2()              // 7) N2 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ ;ИЗЧИСЛЯВАНЕ НА ПЛЪТНОСТ :: S:D<0.5 D=0.5 X C,A2,F Q 
        {
            //Arrange
            double c = 0;
            double a2 = 0;
            double f = 0;
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double al = 0.001163;
            if (d < 0.5)
            {
                d = 0.5;
            }
            c = GetValueFormulaC(t, d, al);
            a2 = GetValueFormulaA2(p, t, c, d4, d5, d6);
            f = GetValueFormulaF(d2, pl, a2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 11.620182675781407;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //11.620182675781408
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaG2()              // 8) G2 ;НЕФТОЗАВОДСКИ ГАЗ И ВОДОРОД :: S DF=D X A2,F Q
        {
            //Arrange
            double a2 = 0;
            double f = 0;
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            a2 = GetValueFormulaA2(p, t, d, d4, d5, d6);
            f = GetValueFormulaF(d2, pl, a2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 11.622886473663454;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //11.622886473663455
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaV2()              // 9) V2 ;КОНДЕНЗАТ И ХОВ :: X C1,A2,F Q
        {
            //Arrange
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double qpt3 = 0.9787;
            double a2 = GetValueFormulaA2(p, t, qpt3, d4, d5, d6);
            double f = GetValueFormulaF(d2, pl, a2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 1.6261244801250729;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //1.6261244801250729
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaN3()              // 10) N3 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: S:D<0.5 D=0.5 X C,A7,F S Q=Q*DF Q
        {
            //Arrange
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double al = 0.001163;
            if (d < 0.5)
            {
                d = 0.5;
            }
            double c = GetValueFormulaC(t, d, al);
            double a7 = GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = GetValueFormulaF(d2, pl, a7);
            f = f * c;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 58.087397534379528;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //58.087397534379541
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaG3()              // 11) G3 ;НЕФТОЗАВОДСКИ ГАЗ :: S DF=D X A7,F S Q=Q*DF Q
        {
            //Arrange
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double df = d;
            
            double a7 = GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = GetValueFormulaF(d2, pl, a7);
            f = f * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 58.11443236831727;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //58.114432368317267
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaV3()              // 12) G3 ;КОНДЕНЗАТ И ХОВ :: X C1,A7,F S Q=Q*DF Q
        {
            //Arrange
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double qpt3 = 0.9929;
            double df = qpt3;
            
            double c1 = GetValueFormulaC1(p, t, qpt3);
            double a7 = GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = GetValueFormulaF(d2, pl, a7);
            f = f * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 1.1540363979700443;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //58.114432368317267
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaPP3()              // 13) PP3 ;МРЕЖОВА ВОДА :: X C1,A7,F S Q=Q*DF Q
        {
            //Arrange
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double qpt3 = 0.9929;
            double df = qpt3;

            double c1 = GetValueFormulaC1(p, t, qpt3);
            double a7 = GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = GetValueFormulaF(d2, pl, a7);
            f = f * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 1.1540363979700443;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assertal =
            //58.114432368317267
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaN4()              // 14) N4 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: S:D<0.5 D=0.5 X C,A3,F Q
        {
            //Arrange
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d6 = 50;
            double d = 50;
            double al = 0.001163;            
            if (d < 0.5)
            {
                d = 0.5;
            }

            double c = GetValueFormulaC(t, d, al);
            double a3 = GetValueFormulaA3(t, c, d4, d6);
            double f = GetValueFormulaF(d2, pl, a3);
            
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 9.634946482738739;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //9.634946482738739
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaN5()              // 15) N5 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: S:D<0.5 D=0.5 X C,A15,F S Q=Q*DF Q
        {
            //Arrange
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d6 = 50;
            double d = 50;
            double al = 0.001163;
            if (d < 0.5)
            {
                d = 0.5;
            }

            double df = GetValueFormulaC(t, d, al);
            double a15 = GetValueFormulaA15(t, df, d4, d6);
            double f = GetValueFormulaF(d2, pl, a15);
            f = f * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 48.174732413693696;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //48.174732413693704
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaG6()              // 16) G6 ;НЕФТОЗАВОДСКИ ГАЗ И ВОДОРОД :: X A4,F S Q=Q/1000 Q
        {
            //Arrange
            double p = 15;
            double pl = 20;
            double d2 = 15;
            double d5 = 10;

            double a4 = GetValueFormulaA4(p, d5);
            double f = GetValueFormulaF(d2, pl, a4);
            f = f / 1000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 0.0036181361349331632;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //0.0036181361349331641
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaG7()              // 17) G7 ;НЕФТОЗАВОДСКИ ГАЗ И ВОДОРОД :: S DF=D X A7,F S Q=Q*DF/1000 Q
        {
            //Arrange
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double df = d;

            double a7 = GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = GetValueFormulaF(d2, pl, a7);
            f = (f * df) / 1000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 0.05811443236831727;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //0.05811443236831728
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaR10()              // 20) R10 ;ПРИРОДЕН ГАЗ :: X A7,F S Q=Q/1000 Q
        {
            //Arrange
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            
            double a7 = GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = GetValueFormulaF(d2, pl, a7);
            f = f / 1000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 0.0011622886473663454;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //0.0011622886473663457
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaO13()              // 23) O13 ;ОБОРОТНИ, ПИТЕЙНИ И ОТПАДНИ ВОДИ :: X A10 Q
        {
            //Arrange
            double pl = 20;
            double d2 = 15;
            
            double q = GetValueFormulaA10(pl, d2);
            
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 3;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //3.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaN14()              // 25) N14 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: X A10 Q
        {
            //Arrange
            double pl = 20;
            double d2 = 15;

            double q = GetValueFormulaA10(pl, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 3;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //3.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaV14()              // 26) V14 ;КОНДЕНЗАТ, ХОВ :: X A10 Q
        {
            //Arrange
            double pl = 20;
            double d2 = 15;

            double q = GetValueFormulaA10(pl, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 3;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //3.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaI16()              // 30) I16 ;ВЪЗДУХ, АЗОТ, КИСЛОРОД :: X A1,F Q
        {
            //Arrange
            double p = 20;
            double t = 20;
            double d2 = 15;
            double pl = 20;
            double d5 = 2;
            double d6 = 2;
            double a1 = GetValueFormulaA1(p, t, d5, d6);
            double f = GetValueFormulaF(d2, pl, a1);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 7.689583397867826;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //7.6895833978678265
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaI17()              // 31) I17 ;ВЪЗДУХ, АЗОТ, КИСЛОРОД :: X A4,F Q
        {
            //Arrange
            double p = 15;
            double pl = 20;
            double d2 = 15;
            double d5 = 10;

            double a4 = GetValueFormulaA4(p, d5);
            double f = GetValueFormulaF(d2, pl, a4);
            
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double expected = 3.6181361349331632;

            string expr = @"par.f";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //3.618136134933164
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaR18()              // 32) R18 ;БРОЯЧИ ЗА ПРИРОДЕН ГАЗ :: X A11 Q
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double d2 = 15;

            double q = GetValueFormulaA11(pl, pl1, d2);
            
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 150;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //150.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaN18()              // 34) N18 ;;БРОЯЧИ ЗА НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: X A11 Q
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double d2 = 15;

            double q = GetValueFormulaA11(pl, pl1, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 150;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //150.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaO18()              // 36) O18 ;;БРОЯЧИ ЗА ВОДИ :: X A11 Q
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double d2 = 15;

            double q = GetValueFormulaA11(pl, pl1, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 150;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //150.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaP18()              // 39) P18 ;;БРОЯЧИ ЗА ПАРА :: X A11 Q
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double d2 = 15;

            double q = GetValueFormulaA11(pl, pl1, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 150;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //150.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaZZ52()              // 41) ZZ52 ;БРОЯЧИ ЗА ПАРА - ДОБАВЕНО 03/05/2007 - ЗА ТЕЦА /ТЦ104/  :: X A11 Q
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double d2 = 15;

            double q = GetValueFormulaA11(pl, pl1, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 150;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //150.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaZ18()              // 42) Z18 ;БРОЯЧИ ЗА ПАРА [MWH] :: X A11,K15,K2,K3,EN S Q=Q*ENT/0.860 Q
        {
            //Arrange
            double p = 15;
            double t = 40;
            double pl = 20;
            double pl1 = 10;
            double d2 = 15;
            double a11 = GetValueFormulaA11(pl, pl1, d2);
            double ent = GetValueFormulaEN(t, p);
            double q = (a11 * ent) / 0.860;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = -21.565392831532395;

            string expr = @"(par.q)";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //-21.565392831532428
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaN19()              // 47) N19 ;БРОЯЧИ ЗА НЕФТОПРОДУКТИ :: S:D<0.5 D=0.5 X C,A11 S Q=Q*DF Q
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double t = 40;            
            double d = 50;
            double d2 = 15;
            double al = 0.001163;
            if (d < 0.5)
            {
                d = 0.5;
            }
            double c = GetValueFormulaC(t, d, al);
            double a11 = GetValueFormulaA11(pl, pl1, d2);
            double q = a11 * c;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 7496.511;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //7496.5109999999995
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaG19()              // 48) G19 ;БРОЯЧИ ЗА ГАЗОВЕ :: S DF=D X A11 S Q=Q*DF Q
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double d = 50;
            double d2 = 15;
            double df = d;

            double q = GetValueFormulaA11(pl, pl1, d2);
            q = q * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 7500;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //7500.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaI19()              // 52) I19 ;БРОЯЧИ ЗА ВЪЗДУХ, АЗОТ, КИСЛОРОД :: S DF=D X A11 S Q=Q*DF Q
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double d = 50;
            double d2 = 15;
            double df = d;

            double q = GetValueFormulaA11(pl, pl1, d2);
            q = q * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 7500;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //7500.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaG26()              // 62) G26 ;ПРЕВРЪЩАНЕ ДИМЕНСИЯТА ОТ "Н.M.КУБ." В "ТОНОВЕ" :: S Q=PL*D Q
        {
            //Arrange
            double pl = 20;
            double d = 50;            
            double q = pl * d;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 1000;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //1000.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaP26()              // 63) P26 ;ПРЕВРЪЩАНЕ ДИМЕНСИЯТА ОТ "M.КУБ." В "ТОНОВЕ" :: S Q=PL*D Q
        {
            //Arrange
            double pl = 20;
            double d = 50;
            double q = pl * d;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 1000;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //1000.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaPP36()              // 79) PP36 ;МРЕЖ.ВОДА КАТО ПАРА /ТОН/ :: S Q=PL*T*D(2)/73500 Q
        {
            //Arrange
            double pl = 20;
            double t = 40;
            double d2 = 15;
            double q = pl * t * d2 / 73500;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = .16326530612244897;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //0.16326530612244897
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaZZ36()              // 80) ZZ36 ;МРЕЖ.ВOДА КАТО ПАРА [MWH] :: S Q=PL*T*D(2)/86000 Q
        {
            //Arrange
            double pl = 20;
            double t = 40;
            double d2 = 15;
            double q = pl * t * d2 / 86000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = .13953488372093023;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //0.13953488372093023
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaZZ38()              // 82) ZZ38 ;СТОКОВ КОНДЕНЗАТ-ТОПЛОЕНЕРГИЯ /Х.КВТЧ./ :: S Q=PL/0.860 Q
        {
            //Arrange
            double pl = 1720;
            double q = pl / 860;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 2;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //2.0
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaZZ39()              // 84) ZZ39 ;КОНДЕНЗАТ-ТОПЛОЕНЕРГИЯ /Х.КВТЧ/ :: S DF=D X A11 S Q=Q*DF*T/860 Q
        {
            //Arrange
            double pl = 20;
            double pl1 = 10;
            double t = 40;
            double d = 50;
            double d2 = 15;
            double df = d;

            double q = GetValueFormulaA11(pl, pl1, d2);
            q = q * df * (t/860);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 348.83720930232558;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //348.83720930232556
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }

        [TestMethod]
        public void FormulaN42()              // 87) N42 ;НЕФТОПРОДУКТИ /ХО-1/ :: S:D<0.5 D=0.5 X C S Q=PL*DF Q
        {
            //Arrange
            double pl = 20;
            double t = 40;
            double d = 50;
            double al = 0.001163;
            if (d < 0.5)
            {
                d = 0.5;
            }
            double c = GetValueFormulaC(t, d, al);
            double q = pl * c;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            double expected = 999.5348;

            string expr = @"par.q";
            //Act
            double actual = calculator.Calculate(expr, "par", 1, inputParams);
            //Assert
            //999.5348
            Assert.AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual));
        }





        /**********************************************************/
        /***                                                    ***/
        /***    F U N C T I O N S                               ***/
        /***                                                    ***/
        /**********************************************************/

        public double GetValueFormulaF(double d2, double pl, double res)
        {

            double actual = ((d2 * pl * res) / 100);
            return actual;
        }

        public double GetValueFormulaC(double t, double d, double al)
        {

            double actual = (d - (al * (t - 20)));
            return actual;
        }

        public double GetValueFormulaC1(double p, double t, double qpt3)
        {

            t = t + 273;
            p = p + 1;
            if ((p > 0) & (p < 4))
            {
                p = 1;
            }
            if ((p > 3) & (p < 8))
            {
                p = 5;
            }
            if ((p > 7) & (p < 13))
            {
                p = 10;
            }
            if ((p > 12) & (p < 18))
            {
                p = 15;
            }
            if (p > 17)
            {
                p = 20;
            }
            if (t > 343)
            {
                t = 343;
            }
            double actual = qpt3;
            return actual;
        }

        public double GetValueFormulaA1(double p, double t, double d5, double d6)
        {

            double actual = Math.Sqrt(((p + 1) * (d6 + 273)) / ((d5 + 1) * (t + 273)));
            return actual;
        }

        public double GetValueFormulaA2(double p, double t, double df, double d4, double d5, double d6)
        {

            double actual = Math.Sqrt(((p + 1) * (d6 + 273) * df) / ((d5 + 1) * (t + 273) * d4));
            return actual;
        }

        public double GetValueFormulaA3(double t, double df, double d4, double d6)
        {
            
            double actual = Math.Sqrt(((d6 + 273) * df) / ((t + 273) * d4));
            return actual;
        }

        public double GetValueFormulaA4(double p, double d5)
        {

            double actual = Math.Sqrt((p + 1) / (d5 + 1));
            return actual;
        }

        public double GetValueFormulaA7(double p, double t, double d, double d4, double d5, double d6)
        {

            double actual = Math.Sqrt(((p + 1) * (d6 + 273) * d4) / ((d5 + 1) * (t + 273) * d));
            return actual;
        }

        public double GetValueFormulaA10(double pl, double d2)
        {

            double actual = ((d2 * pl) / 100);
            return actual;
        }

        public double GetValueFormulaA11(double pl, double pl1, double d2)
        {

            double actual = (d2 * (pl - pl1));
            return actual;
        }

        public double GetValueFormulaA15(double t, double df, double d4, double d6)
        {

            double actual = Math.Sqrt(((d6 + 273) * d4) / ((t + 273) * df));
            return actual;
        }
               
        public double GetValueFormulaENT(double t, double p)
        {

            double actual = (((0.758195 - (7.97826 / Math.Pow((0.001 * (t + 273.15)), 2)) - ((3.078455 * 0.001 * (t + 273.15) - 0.21549) / Math.Pow(0.001 * (t + 273.15) - 0.21, 3))) * 0.01 * p) +
                            (0.0644126 - (0.268671 / Math.Pow(0.001 * (t + 273.15), 8)) - (0.216661 / Math.Pow(0.001 * (t + 273.15), 14) / 100)) * Math.Pow(0.01 * p, 2) +
                            (503.43 + (11.02849 * Math.Log(((t + 273.15) / 647.6))) + (229.2569 * ((t + 273.15) / 647.6)) + (37.93129 * Math.Pow(((t + 273.15) / 647.6), 2)))) * 0.001;
            return actual;
        }

        public double GetValueFormulaK15(double t, double p)
        {
            double pw = 0;
            double pw1 = 0;
            t = t + 273.15;
            pw = Math.Pow((0.001 * t), 2);
            pw1 = Math.Pow(((0.001 * t) - 0.21), 3);
            double actual = ((0.758195 - (7.97826 / pw) - ((3.078455 * 0.001 * t - 0.21549) / pw1)) * 0.01 * p);
            return actual;
        }

        public double GetValueFormulaK2(double t, double p)
        {
            double pt = 0;
            double pr1 = 0;
            double pr2 = 0;
            t = t + 273.15;
            pt = Math.Pow((0.001 * t), 8);
            pr1 = Math.Pow((0.001 * t), 14);
            pr2 = Math.Pow((0.01 * p), 2);
            double actual = (0.0644126 - (0.268671 / pt) - (0.216661 / pr1 / 100)) * pr2;
            return actual;
        }

        public double GetValueFormulaK3(double t)
        {
            double j = 0;
            double j1 = 0;
            double w3 = 0;
            t = t + 273.15;
            j = t / 647.6;
            j1 = Math.Log(j);
            w3 = Math.Pow(j, 2);
            double actual = (503.43 + (11.02849 * j1) + (229.2569 * j) + (37.93129 * w3));
            return actual;
        }

        public double GetValueFormulaEN(double t, double p)
        {
            double k15 = 0;
            double k2 = 0;
            double k3 = 0;
            k15 = GetValueFormulaK15(t, p);
            k2 = GetValueFormulaK2(t, p);
            k3 = GetValueFormulaK3(t);
            double actual = ((k15 + k2 +k3) * 0.001);
            return actual;
        }

    }


}
