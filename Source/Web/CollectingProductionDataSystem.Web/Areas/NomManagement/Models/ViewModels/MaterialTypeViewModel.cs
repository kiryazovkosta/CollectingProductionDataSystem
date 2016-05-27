using System;
namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using Resources = App_GlobalResources.Resources;

    public class MaterialTypeViewModel : IMapFrom<MaterialType>,IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "IsDetailRequired", ResourceType = typeof(Resources.Layout))]
        public bool IsDetailRequired { get; set; }

        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }

        // TODO: Remove this when Production Plan rewriting is finished
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string SortableName { get; set; }
    }
}