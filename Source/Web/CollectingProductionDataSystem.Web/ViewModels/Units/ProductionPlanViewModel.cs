using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.ViewModels.Units
{
    public class ProductionPlanViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Percentages { get; set; }
        public decimal QuantityPlan { get; set; }
        public decimal QuantityFact { get; set; }
    }
}