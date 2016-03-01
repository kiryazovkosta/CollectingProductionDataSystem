namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models
{
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Resources = App_GlobalResources.Resources;

    public class ProcessUnitUnitsMonthlyDataViewModel : IMapFrom<ProcessUnit>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProcessUnitName", ResourceType = typeof(Resources.Layout))]
        public string ShortName { get; set; }

        public string SortableName { get; set; }

        public FactoryViewModel Factory { get; set; }
    }
}