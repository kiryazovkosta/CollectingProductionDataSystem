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
    public class ProductionPlanViewModel : IMapFrom<ProductionPlanData>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PercentagesPlan { get; set; }
        public decimal QuantityPlan { get; set; }
        public decimal QuantityFact { get; set; }
        public decimal PercentagesFact { get; set; }

        public decimal TotalMonthQuantityFact { get; set; }
        public decimal TotalMonthPercentagesFact { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductionPlanData, ProductionPlanViewModel>()
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