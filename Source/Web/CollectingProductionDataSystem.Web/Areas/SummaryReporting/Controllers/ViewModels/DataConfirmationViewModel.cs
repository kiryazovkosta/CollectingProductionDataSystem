using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.ViewModels
{
    public class DataConfirmationViewModel
    {
        public DataConfirmationViewModel()
        {
            this.Shift1Confirmed = false;
            this.Shift2Confirmed = false;
            this.Shift3Confirmed = false;
            this.DayMaterialConfirmed = false;

        }

        [UIHint("Hidden")]
        public int FactoryId { get; set; }

        [Display(Name = "Factory", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public string FactoryName { get; set; }

        public string FactorySortableName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(this.FactoryId.ToString("d2"));
                sb.Append(" ");
                sb.Append(this.FactoryName);
                return  sb.ToString() ;
            }
        }

        [UIHint("Hidden")]
        public int ProcessUnitId { get; set; }

        [Display(Name = "ProcessUnit", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public string ProcessUnitName { get; set; }

        [Display(Name = "Shift1Confirmed", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public bool Shift1Confirmed { get; set; }

        [Display(Name = "Shift2Confirmed", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public bool Shift2Confirmed { get; set; }

        [Display(Name = "Shift3Confirmed", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public bool Shift3Confirmed { get; set; }

        [Display(Name = "DayMaterialConfirmed", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public bool DayMaterialConfirmed { get; set; }

        [Display(Name = "DayEnergyConfirmed", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public bool DayEnergyConfirmed { get; set; }


        [Display(Name = "DayConfirmed", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public bool DayConfirmed { get; set; }

        [UIHint("DailyConfirmationViewModel")]
        public string ConfirmedDaysUntilTheDay { get; set; }
    }
}