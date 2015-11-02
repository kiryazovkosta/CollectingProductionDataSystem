using System;
using System.Collections.Generic;
namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Transactions;
    using Resources = App_GlobalResources.Resources;

    public class IkunkViewModel:IMapFrom<Ikunk>, IEntity
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