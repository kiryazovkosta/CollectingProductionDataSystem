namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using Resources = App_GlobalResources.Resources;

    public class MonthlyReportTypeViewModel : IMapFrom<MonthlyReportType>, IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Position", ResourceType = typeof(Resources.Layout))]
        public int Position { get; set; }

        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }
    }
}