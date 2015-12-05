namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.ViewModels
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;

    public class DailyConfirmationViewModel
    {
        [JsonProperty(ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        public DateTime Day { get; set; }

        public bool IsConfirmed { get; set; }
    }
}