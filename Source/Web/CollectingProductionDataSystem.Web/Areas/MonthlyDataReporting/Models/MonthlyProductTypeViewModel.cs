namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models
{
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using Resources = App_GlobalResources.Resources;

    public class MonthlyProductTypeViewModel : IMapFrom<DailyProductType>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductType", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        public string SortableName { get; set; }
    }
}