namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using Resources = App_GlobalResources.Resources;

    public class UnitMonthlyConfigViewModel : IMapFrom<UnitMonthlyConfig>, IHaveCustomMappings, IEntity
    {
        public UnitMonthlyConfigViewModel()
        {
            this.UnitDailyConfigUnitMonthlyConfigs = new HashSet<UnitDailyConfigUnitMonthlyConfigViewModel>();
            this.RelatedUnitMonthlyConfigs = new HashSet<RelatedUnitMonthlyConfigsViewModel>();
        }

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
        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public int ProcessUnitId { get; set; }

        [Required]
        [Display(Name = "Product", ResourceType = typeof(Resources.Layout))]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "MonthlyReportType", ResourceType = typeof(Resources.Layout))]
        public int MonthlyReportTypeId { get; set; }

        [Required]
        [Display(Name = "ProductType", ResourceType = typeof(Resources.Layout))]
        public int ProductTypeId { get; set; }

        [Required]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public int MeasureUnitId { get; set; }

        [Required]
        [Display(Name = "MaterialType", ResourceType = typeof(Resources.Layout))]
        public int MaterialTypeId { get; set; }

        [Display(Name = "MaterialDetailType", ResourceType = typeof(Resources.Layout))]
        public int MaterialDetailTypeId { get; set; }

        [Display(Name = "AggregationFormula", ResourceType = typeof(Resources.Layout))]
        public string AggregationFormula { get; set; }

        [Display(Name = "AggregationCurrentLevel", ResourceType = typeof(Resources.Layout))]
        public bool AggregationCurrentLevel { get; set; }

        [Display(Name = "IsEditable", ResourceType = typeof(Resources.Layout))]
        public bool IsEditable { get; set; }

        [UIHint("UnitDailyConfigUnitMonthlyConfigEditor")]
        [Display(Name = "UnitDailyConfigUnitMonthlyConfig", ResourceType = typeof(Resources.Layout))]
        public ICollection<UnitDailyConfigUnitMonthlyConfigViewModel> UnitDailyConfigUnitMonthlyConfigs { get; set; }

        [UIHint("RelatedUnitMonthlyConfigsEditor")]
        [Display(Name = "RelatedUnitMonthlyConfigs", ResourceType = typeof(Resources.Layout))]
        public ICollection<RelatedUnitMonthlyConfigsViewModel> RelatedUnitMonthlyConfigs { get; set; }

        [Display(Name = "IsManualEntry", ResourceType = typeof(Resources.Layout))]
        public bool IsManualEntry { get; set; }

        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }

        [Required]
        [Display(Name = "IsTotalPosition", ResourceType = typeof(Resources.Layout))]
        public bool IsTotalPosition { get; set; }

        [Required]
        [Display(Name = "IsTotalInputPosition", ResourceType = typeof(Resources.Layout))]
        public bool IsTotalInputPosition { get; set; }

        [Required]
        [Display(Name = "IsExternalOutputPosition", ResourceType = typeof(Resources.Layout))]
        public bool IsExternalOutputPosition { get; set; }

        [Required]
        [Display(Name = "IsTotalExternalOutputPosition", ResourceType = typeof(Resources.Layout))]
        public bool IsTotalExternalOutputPosition { get; set; }

        [Required]
        [Display(Name = "IsTotalInternalPosition", ResourceType = typeof(Resources.Layout))]
        public bool IsTotalInternalPosition { get; set; }

        [Required]
        [Display(Name = "ProductionPlanConfig", ResourceType = typeof(Resources.Layout))]
        public int? ProductionPlanConfigId { get; set; }



        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitMonthlyConfig, UnitMonthlyConfigViewModel>()
                    //.ForMember(p => p.MonthlyProductTypeId, opt => opt.MapFrom(p => p.MonthlyProductTypeId == null ? 0 : (int)p.MonthlyProductTypeId))
                //.ForMember(p => p.MaterialTypeId, opt => opt.MapFrom(p => p.MaterialTypeId == null ? 0 : p.MaterialTypeId))
                    //.ForMember(p => p.MaterialDetailTypeId, opt => opt.MapFrom(p => p.MaterialDetailTypeId == null ? 0 : p.MaterialDetailTypeId))
                    .ForMember(p => p.UnitDailyConfigUnitMonthlyConfigs, opt => opt.MapFrom(p => p.UnitDailyConfigUnitMonthlyConfigs.OrderBy(x => x.Position)))
                    .ForMember(p => p.RelatedUnitMonthlyConfigs, opt => opt.MapFrom(p => p.RelatedUnitMonthlyConfigs.OrderBy(x => x.Position)));


            configuration.CreateMap<UnitMonthlyConfigViewModel, UnitMonthlyConfig>()
                 //.ForMember(p => p.MonthlyProductTypeId, opt => opt.MapFrom(p => p.MonthlyProductTypeId == 0 ? null : (Nullable<int>)p.MonthlyProductTypeId))
                 .ForMember(p => p.UnitDailyConfigUnitMonthlyConfigs, opt => opt.MapFrom(p => p.UnitDailyConfigUnitMonthlyConfigs != null ?
                    p.UnitDailyConfigUnitMonthlyConfigs.Select((x, ixc) => new UnitDailyConfigUnitMonthlyConfig() { UnitDailyConfigId = x.UnitDailyConfigId, UnitMonthlyConfigId = p.Id, Position = ixc + 1, }) :
                    new List<UnitDailyConfigUnitMonthlyConfig>()))
                 .ForMember(p => p.RelatedUnitMonthlyConfigs, opt => opt.MapFrom(p => p.RelatedUnitMonthlyConfigs != null ?
                    p.RelatedUnitMonthlyConfigs.Select((x, ixc) => new RelatedUnitMonthlyConfigs() 
                    { UnitMonthlyConfigId = p.Id, RelatedUnitMonthlyConfigId = x.Id, Position = ixc + 1 }) :
                    new List<RelatedUnitMonthlyConfigs>()))
                    //.ForMember(p => p.MaterialDetailTypeId, opt => opt.MapFrom(p => p.MaterialDetailTypeId == 0 ? null : (int?)p.MaterialDetailTypeId))
                  .ForMember(p => p.IsConverted, opt => opt.Ignore());
        }
    }
}