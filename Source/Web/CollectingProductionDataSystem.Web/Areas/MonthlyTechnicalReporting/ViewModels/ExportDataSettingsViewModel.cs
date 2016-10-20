using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.ViewModels
{
    [Serializable]
    public class ExportDataSettingsViewModel
    {
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public DateTime Month { get; set; }
        public string MonthAsString { get; set; }
        public string CreatorName { get; set; }
        public string Ocupation { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}