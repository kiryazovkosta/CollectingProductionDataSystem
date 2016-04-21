using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models
{
    public class RecalculatedMonthlyDataViewModel
    {
        public DateTime? Date { get; set; }

        public string Action { get; set; }
    }
}