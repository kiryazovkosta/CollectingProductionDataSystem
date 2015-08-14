using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    internal class Utility
    {
        public static void SetRegionalSettings()
        {
	        if (Properties.Settings.Default.FORCE_REGIONAL_SETTINGS) {
		        CultureInfo newCi = new CultureInfo(Properties.Settings.Default.CULTURE_INFO, false);
		        newCi.NumberFormat.CurrencyDecimalSeparator = ".";
		        newCi.NumberFormat.CurrencyGroupSeparator = string.Empty;
		        newCi.NumberFormat.NumberDecimalSeparator = ".";
		        newCi.NumberFormat.NumberGroupSeparator = string.Empty;
		        newCi.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
		        newCi.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
		        newCi.DateTimeFormat.LongTimePattern = "HH:mm:ss";
		        newCi.DateTimeFormat.LongDatePattern = "dd.MM.yyyy";
		        System.Threading.Thread.CurrentThread.CurrentCulture = newCi;
	        }
        }

    }
}
