using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models
{
    public class MonthlyReportParametersViewModel
    {
        public MonthlyReportParametersViewModel(string reportName, string controllerName, int monthlyReportTypeId, string defaultViewName) 
        {
            this.ReportName = reportName;
            this.ControllerName = controllerName;
            this.MonthlyReportTypeId = monthlyReportTypeId;
            this.DefaultView = defaultViewName;
        }
        public string ReportName { get; private set; }
        public string ControllerName { get; private set; }
        public int MonthlyReportTypeId { get; private set; }
        public string DefaultView { get; set; }
    }
}