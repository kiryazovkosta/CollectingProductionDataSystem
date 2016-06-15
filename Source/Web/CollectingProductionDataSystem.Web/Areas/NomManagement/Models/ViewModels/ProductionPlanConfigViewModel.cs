using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Models.Productions.Mounthly;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class ProductionPlanConfigViewModel : IMapFrom<ProductionPlanConfig>, IEntity, IHaveCustomMappings
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

        [Required]
        [Display(Name = "Position", ResourceType = typeof(Resources.Layout))]
        public int Position { get; set; }

        [Display(Name = "IsSummaryOfProcessing", ResourceType = typeof(Resources.Layout))]
        public bool IsSummaryOfProcessing { get; set; }

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

        [Display(Name = "UsageRateFormula", ResourceType = typeof(Resources.Layout))]
        public string UsageRateFormula { get; set; }

        [Display(Name = "UsageRateMembers", ResourceType = typeof(Resources.Layout))]
        public string UsageRateMembers { get; set; }

        [Required]
        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public int ProcessUnitId { get; set; }

        [Required]
        [Display(Name = "MaterialType", ResourceType = typeof(Resources.Layout))]
        public int MaterialTypeId { get; set; }

        [Display(Name = "MaterialDetailType", ResourceType = typeof(Resources.Layout))]
        public int MaterialDetailTypeId { get; set; }

        [Required]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public int MeasureUnitId { get; set; }

        [Required]
        [Display(Name = "IsPercentagesPlanVisibleInChast", ResourceType = typeof(Resources.Layout))]
        public bool IsPercentagesPlanVisibleInChast { get; set; }

        [Required]
        [Display(Name = "IsPercentagesFactVisibleInChast", ResourceType = typeof(Resources.Layout))]
        public bool IsPercentagesFactVisibleInChast { get; set; }

        [Required]
        [Display(Name = "IsQuanityPlanVisibleInChast", ResourceType = typeof(Resources.Layout))]
        public bool IsQuanityPlanVisibleInChast { get; set; }

        [Required]
        [Display(Name = "IsQuantityFactVisibleInChast", ResourceType = typeof(Resources.Layout))]
        public bool IsQuantityFactVisibleInChast { get; set; }

        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }

        [Required]
        [Display(Name = "IsPropductionPlan", ResourceType = typeof(Resources.Layout))]
        public bool IsPropductionPlan { get; set; }
        [Required]
        [Display(Name = "IsMonthlyPlan", ResourceType = typeof(Resources.Layout))]
        public bool IsMonthlyPlan { get; set; }

        [Display(Name = "MonthlyValuePlanFormula", ResourceType = typeof(Resources.Layout))]
        public string MonthlyValuePlanFormula { get; set; }

        [UIHint("ProductionPlanConfigUnitMonthlyConfigPlanMembersViewModelEditor")]
        [Display(Name = "ProductionPlanConfigUnitMonthlyConfigPlanMembers", ResourceType = typeof(Resources.Layout))]
        public ICollection<ProductionPlanConfigUnitMonthlyConfigPlanMembersViewModel> ProductionPlanConfigUnitMonthlyConfigPlanMembers { get; set; }

        [Display(Name = "MonthlyFactFractionFormula", ResourceType = typeof(Resources.Layout))]
        public string MonthlyFactFractionFormula { get; set; }

        [UIHint("ProductionPlanConfigUnitMonthlyConfigFactFractionMembersViewModelEditor")]
        [Display(Name = "ProductionPlanConfigUnitMonthlyConfigFactFractionMembers", ResourceType = typeof(Resources.Layout))]
        public ICollection<ProductionPlanConfigUnitMonthlyConfigFactFractionMembersViewModel> ProductionPlanConfigUnitMonthlyConfigFactFractionMembers { get; set; }
        


        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductionPlanConfig, ProductionPlanConfigViewModel>()
                    .ForMember(p => p.MaterialDetailTypeId, opt => opt.MapFrom(p => p.MaterialDetailTypeId == null ? 0 : p.MaterialDetailTypeId))
                    .ForMember(p => p.ProductionPlanConfigUnitMonthlyConfigPlanMembers, opt => opt.MapFrom(p => p.ProductionPlanConfigUnitMonthlyConfigPlanMembers.OrderBy(x => x.Position)))
                    .ForMember(p => p.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers, opt => opt.MapFrom(p => p.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers.OrderBy(x => x.Position))); 

            configuration.CreateMap<ProductionPlanConfigViewModel, ProductionPlanConfig>()
                    .ForMember(p => p.MaterialDetailTypeId, opt => opt.MapFrom(p => p.MaterialDetailTypeId == 0 ? null : (int?)p.MaterialDetailTypeId))
                    .ForMember(p => p.ProductionPlanConfigUnitMonthlyConfigPlanMembers, opt => opt.MapFrom(p => p.ProductionPlanConfigUnitMonthlyConfigPlanMembers != null ?
                    p.ProductionPlanConfigUnitMonthlyConfigPlanMembers.Select((x, ixc) => new ProductionPlanConfigUnitMonthlyConfigPlanMembers() { UnitMonthlyConfigId = x.UnitMonthlyConfigId, ProductionPlanConfigId = p.Id, Position = ixc + 1, }) :
                    new List<ProductionPlanConfigUnitMonthlyConfigPlanMembers>()))
                    .ForMember(p => p.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers, opt => opt.MapFrom(p => p.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers != null ?
                    p.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers.Select((x, ixc) => new ProductionPlanConfigUnitMonthlyConfigFactFractionMembers() { UnitMonthlyConfigId = x.UnitMonthlyConfigId, ProductionPlanConfigId = p.Id, Position = ixc + 1, }) :
                    new List<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers>()));
        }
    }
}