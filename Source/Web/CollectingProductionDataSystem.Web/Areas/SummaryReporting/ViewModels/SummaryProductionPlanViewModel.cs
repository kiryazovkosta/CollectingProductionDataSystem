﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Productions;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.ViewModels
{
    public class SummaryProductionPlanViewModel : IMapFrom<ProductionPlanData>, IHaveCustomMappings
    {
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Display(Name = "Code", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public string Code { get; set; }

        [Display(Name = "Name1", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public string Name { get; set; }

        [Display(Name = "PercentagesPlan", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal PercentagesPlan { get; set; }

        [Display(Name = "QuantityPlan", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal QuantityPlan { get; set; }

        [Display(Name = "QuantityFact", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal QuantityFact { get; set; }

        [Display(Name = "PercentagesFact", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal PercentagesFact { get; set; }

        [Display(Name = "TotalMonthPercentagesFact", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal TotalMonthPercentagesFact { get; set; }

        [Display(Name = "TotalMonthQuantityFact", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal TotalMonthQuantityFact { get; set; }

        [Display(Name = "TotalMonthQuantityPlan", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal TotalMonthQuantityPlan { get; set; }

        [Display(Name = "RelativeDifference", ResourceType = typeof(App_GlobalResources.Resources.Layout))]
        public decimal RelativeDifference
        {
            get
            {
                return
                    this.TotalMonthQuantityFact == 0 ? 0
                        : ((this.TotalMonthQuantityFact - this.TotalMonthQuantityPlan)/this.TotalMonthQuantityFact)*100;
            }
        }

        public string FactoryName { get; set; }

        public string ProcessUnitName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductionPlanData, SummaryProductionPlanViewModel>()
                .ForMember(p => p.Code, opt => opt.MapFrom(p => p.ProductionPlanConfig.Code))
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.Name))
                .ForMember(p => p.PercentagesPlan, opt => opt.MapFrom(p => p.PercentagesPlan))
                .ForMember(p => p.QuantityPlan, opt => opt.MapFrom(p => p.QuanityPlan))
                .ForMember(p => p.QuantityFact, opt => opt.MapFrom(p => p.QuantityFact))
                .ForMember(p => p.PercentagesFact, opt => opt.MapFrom(p => p.PercentagesFact))
                .ForMember(p => p.TotalMonthQuantityFact, opt => opt.MapFrom(p => p.QuanityFactCurrentMonth + p.QuantityFact))
                .ForMember(p => p.TotalMonthPercentagesFact, opt => opt.MapFrom(p => p.PercentagesFactCurrentMonth))
                .ForMember(p => p.TotalMonthQuantityPlan, opt => opt.MapFrom(p => p.RecordTimestamp.Day * p.QuanityPlan))
                .ForMember(p => p.RelativeDifference, opt => opt.Ignore())
                .ForMember(p => p.FactoryName, opt => opt.MapFrom(p => string.Format("{0:d2} {1}", p.FactoryId, p.ProductionPlanConfig.ProcessUnit.Factory.ShortName)))
                .ForMember(p => p.ProcessUnitName, opt => opt.MapFrom(p => string.Format("{0:d2} {1}", p.ProcessUnitId, p.ProductionPlanConfig.ProcessUnit.ShortName)));
        }
    }
}