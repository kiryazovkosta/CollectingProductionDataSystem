using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models
{
    public class MonthlyEnergyReportsViewModel
    {
        public MonthlyEnergyReportsViewModel(string reportName, string controllerName, int monthlyReportTypeId) 
        {
            this.ReportName = reportName;
            this.ControllerName = controllerName;
            this.MonthlyReportTypeId = monthlyReportTypeId;
        }
        public string ReportName { get; private set; }
        public string ControllerName { get; private set; }
        public int MonthlyReportTypeId { get; private set; }
    }
}