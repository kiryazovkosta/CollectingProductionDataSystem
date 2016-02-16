namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using Resources = App_GlobalResources.Resources;

    public class TankStatusViewModel : IMapFrom<TankStatus>, IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "FlagValue", ResourceType = typeof(Resources.Layout))]
        public int FlagValue { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }
    }
}