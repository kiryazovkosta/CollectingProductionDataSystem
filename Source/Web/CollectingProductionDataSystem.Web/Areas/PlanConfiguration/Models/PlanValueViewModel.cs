using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Models
{
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models;
    using Resources = App_GlobalResources.Resources;

    public class PlanValueViewModel : IMapFrom<PlanValue>
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        //[Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public ProcessUnitUnitsMonthlyDataViewModel ProcessUnit { get; set; }

        [Required]
        [Display(Name = "Month", ResourceType = typeof(Resources.Layout))]
        public DateTime Month { get; set; }

        [Required]
        [Display(Name = "ProcessingPlan", ResourceType = typeof(Resources.Layout))]
        public decimal Value { get; set; }

        [Display(Name = "ProcessingPlanLiquid", ResourceType = typeof(Resources.Layout))]
        public decimal? ValueLiquid { get; set; }
    }
}