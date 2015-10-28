namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Inventories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Web.ViewModels.Utility;
    using Resources = App_GlobalResources.Resources;

    public class AreaViewModel:IMapFrom<Area>,IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }
    }
}