using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.ViewModels
{
    public class ExportSettingsViewModel
    {
        public int Id { get; set; }

        public string Factory { get; set; }

        public string ProcessUnit { get; set; }

        public DateTime RecordTimestamp { get; set; }
    }
}