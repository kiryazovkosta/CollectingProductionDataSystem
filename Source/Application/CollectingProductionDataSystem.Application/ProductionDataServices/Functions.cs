using System;

namespace CollectingProductionDataSystem.Application.ProductionDataServices
{
    public class Functions
    {
        public static double GetValueFormulaF(double d2, double pl, double res)
        {

            double actual = ((d2 * pl * res) / 100);
            return actual;
        }

        public static double GetValueFormulaC(double t, double d, double al)
        {

            double actual = (d - (al * (t - 20)));
            return actual;
        }

        public static double GetValueFormulaC1(double p, double t, double qpt3)
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

        public static double GetValueFormulaA1(double p, double t, double d5, double d6)
        {

            double actual = Math.Sqrt(((p + 1) * (d6 + 273)) / ((d5 + 1) * (t + 273)));
            return actual;
        }

        public static double GetValueFormulaA2(double p, double t, double df, double d4, double d5, double d6)
        {

            double actual = Math.Sqrt(((p + 1) * (d6 + 273) * df) / ((d5 + 1) * (t + 273) * d4));
            return actual;
        }

        public static double GetValueFormulaA3(double t, double df, double d4, double d6)
        {
            
            double actual = Math.Sqrt(((d6 + 273) * df) / ((t + 273) * d4));
            return actual;
        }

        public static double GetValueFormulaA4(double p, double d5)
        {

            double actual = Math.Sqrt((p + 1) / (d5 + 1));
            return actual;
        }

        public static double GetValueFormulaA7(double p, double t, double d, double d4, double d5, double d6)
        {

            double actual = Math.Sqrt(((p + 1) * (d6 + 273) * d4) / ((d5 + 1) * (t + 273) * d));
            return actual;
        }

        public static double GetValueFormulaA10(double pl, double d2)
        {

            double actual = ((d2 * pl) / 100);
            return actual;
        }

        public static double GetValueFormulaA11(double pl, double pl1, double d2)
        {

            double actual = (d2 * (pl - pl1));
            return actual;
        }

        public static double GetValueFormulaA15(double t, double df, double d4, double d6)
        {

            double actual = Math.Sqrt(((d6 + 273) * d4) / ((t + 273) * df));
            return actual;
        }
               
        public static double GetValueFormulaEnt(double t, double p)
        {

            double actual = (((0.758195 - (7.97826 / Math.Pow((0.001 * (t + 273.15)), 2)) - ((3.078455 * 0.001 * (t + 273.15) - 0.21549) / Math.Pow(0.001 * (t + 273.15) - 0.21, 3))) * 0.01 * p) +
                            (0.0644126 - (0.268671 / Math.Pow(0.001 * (t + 273.15), 8)) - (0.216661 / Math.Pow(0.001 * (t + 273.15), 14) / 100)) * Math.Pow(0.01 * p, 2) +
                            (503.43 + (11.02849 * Math.Log(((t + 273.15) / 647.6))) + (229.2569 * ((t + 273.15) / 647.6)) + (37.93129 * Math.Pow(((t + 273.15) / 647.6), 2)))) * 0.001;
            return actual;
        }

        public static double GetValueFormulaK15(double t, double p)
        {
            double pw = 0;
            double pw1 = 0;
            t = t + 273.15;
            pw = Math.Pow((0.001 * t), 2);
            pw1 = Math.Pow(((0.001 * t) - 0.21), 3);
            double actual = ((0.758195 - (7.97826 / pw) - ((3.078455 * 0.001 * t - 0.21549) / pw1)) * 0.01 * p);
            return actual;
        }

        public static double GetValueFormulaK2(double t, double p)
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

        public static double GetValueFormulaK3(double t)
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

        public static double GetValueFormulaEN(double t, double p)
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
