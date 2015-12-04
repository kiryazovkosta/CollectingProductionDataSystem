using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            this.DayConfirmed = false;

        }

        [UIHint("Hidden")]
        public int FactoryId { get; set; }

        [Display(Name = "Factory", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public string FactoryName { get; set; }

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

        [Display(Name = "DayConfirmed", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public bool DayConfirmed { get; set; }

        public IEnumerable<DailyConfirmationViewModel> ConfirmedDaysUntilTheDay { get; set; }
    }
}