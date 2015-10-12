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

    public class ShiftViewModel:IMapFrom<Shift>,IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "BeginTime", ResourceType = typeof(Resources.Layout))]
        public TimeSpan BeginTime { get; set; }

        [Required]
        [Display(Name = "ReadOffset", ResourceType = typeof(Resources.Layout))]
        public TimeSpan ReadOffset { get; set; }

        [Required]
        [Display(Name = "ReadPollTimeSlot", ResourceType = typeof(Resources.Layout))]
        public TimeSpan ReadPollTimeSlot { get; set; }
    }
}