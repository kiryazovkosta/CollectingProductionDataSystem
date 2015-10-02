namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;


    /// <summary>
    /// 
    /// </summary>
    public class ProcessUnitViewModel : IMapFrom<ProcessUnit>
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "ShortName", ResourceType = typeof(Resources.Layout))]
        public string ShortName { get; set; }

        [Required]
        [Display(Name = "FullName", ResourceType = typeof(Resources.Layout))]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Factory", ResourceType = typeof(Resources.Layout))]
        public FactoryViewModel Factory { get; set; }
    }
}