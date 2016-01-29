using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Productions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.ViewModels.Units
{
    public class ProductionPlanEnergyViewModel : IMapFrom<ProductionPlanData>, IHaveCustomMappings 
    {
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Display(Name = "UsageRateName", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public string Name { get; set; }

        [Display(Name = "UsageRatePlan", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal PercentagesPlan { get; set; }

        [Display(Name = "UsageRateQuantityPlan", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal QuantityPlan { get; set; }

        [Display(Name = "UsageRateQuantityFact", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal QuantityFact { get; set; }

        [Display(Name = "UsageRateFact", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal PercentagesFact { get; set; }

         [Display(Name = "UsageRateTotalMonthQuantityFact", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal TotalMonthQuantityFact { get; set; }

        [Display(Name = "TotalMonthPercentagesFact", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal TotalMonthPercentagesFact { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductionPlanData, ProductionPlanEnergyViewModel>()
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.Name))
                .ForMember(p => p.PercentagesPlan, opt => opt.MapFrom(p => p.PercentagesPlan))
                .ForMember(p => p.QuantityPlan, opt => opt.MapFrom(p => p.QuanityPlan))
                .ForMember(p => p.QuantityFact, opt => opt.MapFrom(p => p.QuantityFact))
                .ForMember(p => p.PercentagesFact, opt => opt.MapFrom(p => p.PercentagesFact))
                .ForMember(p => p.TotalMonthQuantityFact, opt => opt.MapFrom(p => p.QuanityFactCurrentMonth + p.QuantityFact))
                .ForMember(p => p.TotalMonthPercentagesFact, opt => opt.MapFrom(p => p.PercentagesFactCurrentMonth));
        }
    }
}