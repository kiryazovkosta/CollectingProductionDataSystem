namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.ViewModels
{
    using System;

    public class DailyConfirmationViewModel
    {
        public DateTime Day { get; set; }

        public bool IsConfirmed { get; set; }
    }
}