namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Resources = App_GlobalResources.Resources;

    public class ManualSelfCalculationViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "ManualValue", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "NewValue", ResourceType = typeof(Resources.Layout))]
        public decimal Value { get; set; }

        [Required]
        [UIHint("Hidden")]
        public int UnitDataId { get; set; }

        public string EditorScreenHeading { get; set; }

        [Required]
        [UIHint("Hidden")]
        public string EnteredMeasurementCode{ get; set; } 
    }
}