namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using Resources = App_GlobalResources.Resources;

    public class TankMasterProductViewModel:IMapFrom<TankMasterProduct>, IEntity
    {
        [Required]
        [Display(Name = "Product", ResourceType = typeof(Resources.Layout))]
        public int Id { get; set; }

        [Required]
        [Display(Name = "TankMasterCode", ResourceType = typeof(Resources.Layout))]
        public int TankMasterProductCode { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }
    }
}