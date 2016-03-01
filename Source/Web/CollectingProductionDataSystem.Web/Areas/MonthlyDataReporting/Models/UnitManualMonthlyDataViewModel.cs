namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models
{
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using Resources = App_GlobalResources.Resources;

    public class UnitManualMonthlyDataViewModel : IMapFrom<UnitManualMonthlyData> //, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ManualValue", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "ManualValue", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public decimal Value { get; set; }
    }
}