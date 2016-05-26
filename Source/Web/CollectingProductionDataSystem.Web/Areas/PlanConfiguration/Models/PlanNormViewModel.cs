namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    public class PlanNormViewModel:IMapFrom<PlanNorm>
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public int ProductionPlanConfigId { get; set; }

        [Required]
        [Display(Name = "Month", ResourceType = typeof(Resources.Layout))]
        public DateTime Month { get; set; }

        [Required]
        [Display(Name = "NormValue", ResourceType = typeof(Resources.Layout))]
        public decimal Value { get; set; }
    }
}