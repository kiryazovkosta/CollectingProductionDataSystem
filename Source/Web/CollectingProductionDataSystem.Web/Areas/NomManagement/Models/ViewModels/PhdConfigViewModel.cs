using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Nomenclatures;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class PhdConfigViewModel : IMapFrom<PhdConfig>, IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "HostName", ResourceType = typeof(Resources.Layout))]
        public string HostName { get; set; }

        [Required]
        [Display(Name = "HostIpAddress", ResourceType = typeof(Resources.Layout))]
        public string HostIpAddress { get; set; }

        [Required]
        [Display(Name = "Position", ResourceType = typeof(Resources.Layout))]
        public int Position { get; set; }

        [Required]
        [Display(Name = "IsActive", ResourceType = typeof(Resources.Layout))]
        public bool IsActive { get; set; }
    }
}