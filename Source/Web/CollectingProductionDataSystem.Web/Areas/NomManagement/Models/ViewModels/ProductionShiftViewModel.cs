namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using Resources = App_GlobalResources.Resources;

    public class ProductionShiftViewModel:IMapFrom<ProductionShift>,IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "BeginTime", ResourceType = typeof(Resources.Layout))]
        public string BeginTime { get; set; }

        [Required]
        [Display(Name = "EndTime", ResourceType = typeof(Resources.Layout))]
        public string EndTime { get; set; }

        [Required]
        [Display(Name = "BeginMinutes", ResourceType = typeof(Resources.Layout))]
        public int BeginMinutes { get; set; }

        [Required]
        [Display(Name = "OffsetMinutes", ResourceType = typeof(Resources.Layout))]
        public int OffsetMinutes { get; set; }
    }
}