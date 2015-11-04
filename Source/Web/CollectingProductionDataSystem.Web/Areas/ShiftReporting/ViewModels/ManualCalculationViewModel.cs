namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Resources = App_GlobalResources.Resources;

    public class ManualCalculationViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "ManualValue", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "OldValue", ResourceType = typeof(Resources.Layout))]
        public decimal OldValue { get; set; }

        public bool IsOldValueAvailableForEditing { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "ManualValue", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "NewValue", ResourceType = typeof(Resources.Layout))]
        public decimal NewValue { get; set; }

        [Required]
        [UIHint("Hidden")]
        public int UnitDataId { get; set; }

        public string EditorScreenHeading { get; set; }

        [Required]
        [UIHint("Hidden")]
        public string MeasurementCode{ get; set; }
    }
}