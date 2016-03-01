namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models
{
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using Resources = App_GlobalResources.Resources;

    public class MeasureUnitUnitMonthlyDataViewModel : IMapFrom<MeasureUnit>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }
    }
}