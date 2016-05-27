using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Models
{
    public class ProductionPlanConfigViewModel : IMapFrom<ProductionPlanConfig>
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        public int Position { get; set; }

        public ProcessUnitUnitsMonthlyDataViewModel ProcessUnit { get; set; }

        public MaterialTypeViewModel MaterialType { get; set; }
        
    }
}