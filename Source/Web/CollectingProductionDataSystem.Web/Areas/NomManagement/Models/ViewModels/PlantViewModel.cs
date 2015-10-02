namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    public class PlantViewModel : IMapFrom<Plant>
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

    }
}