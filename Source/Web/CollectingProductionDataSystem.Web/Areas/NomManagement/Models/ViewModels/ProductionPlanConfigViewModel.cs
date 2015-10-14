using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using Resources = App_GlobalResources.Resources;
namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class ProductionPlanConfigViewModel : IMapFrom<ProductionPlanConfig>, IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Percentages", ResourceType = typeof(Resources.Layout))]
        public decimal Percentages { get; set; }

        [Required]
        [Display(Name = "QuantityPlanFormula", ResourceType = typeof(Resources.Layout))]
        public string QuantityPlanFormula { get; set; }

        [Required]
        [Display(Name = "QuantityPlanMembers", ResourceType = typeof(Resources.Layout))]
        public string QuantityPlanMembers { get; set; }

        [Required]
        [Display(Name = "QuantityFactFormula", ResourceType = typeof(Resources.Layout))]
        public string QuantityFactFormula { get; set; }

        [Required]
        [Display(Name = "QuantityFactMembers", ResourceType = typeof(Resources.Layout))]
        public string QuantityFactMembers { get; set; }

        [Required]
        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public int ProcessUnitId { get; set; }
    }
}