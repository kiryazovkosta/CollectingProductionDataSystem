namespace CollectingProductionDataSystem.Web.Areas.MonthlyReporting.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using Resources = App_GlobalResources.Resources;

    public class InProcessUnitsViewModel : IMapFrom<InProcessUnitData>, IEntity
    {
        [Display(Name = "№")]
        public int Id { get; set; }

        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        [Required]
        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public int ProcessUnitId{ get; set; }

        [Required]
        [Display(Name = "Product", ResourceType = typeof(Resources.Layout))]
        public int ProductId { get; set; }

        [Display(Name = "Mass", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "Mass", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public decimal Mass { get; set; }
    }
}