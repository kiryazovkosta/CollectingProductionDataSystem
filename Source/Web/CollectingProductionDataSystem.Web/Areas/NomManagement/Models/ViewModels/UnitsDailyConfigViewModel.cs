namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    public class UnitsDailyConfigViewModel : IMapFrom<UnitDailyConfig>, IHaveCustomMappings, IEntity
    {
        public UnitsDailyConfigViewModel()
        {
            this.UnitConfigUnitDailyConfigs = new HashSet<UnitConfigUnitDailyConfigViewModel>();
            this.RelatedUnitDailyConfigs = new HashSet<RelatedUnitDailyConfigsViewModel>();
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

        [Display(Name = "ProductType", ResourceType = typeof(Resources.Layout))]
        public int DailyProductTypeId { get; set; }

        [Required]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public int MeasureUnitId { get; set; }

        [Display(Name = "AggregationFormula", ResourceType = typeof(Resources.Layout))]
        public string AggregationFormula { get; set; }

        [Display(Name = "AggregationCurrentLevel", ResourceType = typeof(Resources.Layout))]
        public bool AggregationCurrentLevel { get; set; }

        [Display(Name = "AggregationMembers", ResourceType = typeof(Resources.Layout))]
        public string AggregationMembers { get; set; }

        [Display(Name = "IsEditable", ResourceType = typeof(Resources.Layout))]
        public bool IsEditable { get; set; }

        [UIHint("UnitConfigUnitDailyConfigEditor")]
        [Display(Name = "UnitConfigUnitDailyConfig", ResourceType = typeof(Resources.Layout))]
        public ICollection<UnitConfigUnitDailyConfigViewModel> UnitConfigUnitDailyConfigs { get; set; }

        [UIHint("RelatedUnitDailyConfigsEditor")]
        [Display(Name = "RelatedUnitDailyConfigs", ResourceType = typeof(Resources.Layout))]
        public ICollection<RelatedUnitDailyConfigsViewModel> RelatedUnitDailyConfigs { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitDailyConfig, UnitsDailyConfigViewModel>()
                    .ForMember(p => p.DailyProductTypeId, opt => opt.MapFrom(p => p.DailyProductTypeId == null ? 0 : (int)p.DailyProductTypeId))
                    .ForMember(p => p.UnitConfigUnitDailyConfigs, opt => opt.MapFrom(p => p.UnitConfigUnitDailyConfigs.OrderBy(x => x.Position)))
                    .ForMember(p => p.RelatedUnitDailyConfigs, opt => opt.MapFrom(p => p.RelatedUnitDailyConfigs.OrderBy(x => x.Position)));
                   

            configuration.CreateMap<UnitsDailyConfigViewModel, UnitDailyConfig>()
                 .ForMember(p => p.DailyProductTypeId, opt => opt.MapFrom(p => p.DailyProductTypeId == 0 ? null : (Nullable<int>)p.DailyProductTypeId))
                 .ForMember(p => p.UnitConfigUnitDailyConfigs, opt => opt.MapFrom(p => p.UnitConfigUnitDailyConfigs != null ?
                    p.UnitConfigUnitDailyConfigs.Select((x, ixc) => new UnitConfigUnitDailyConfig() { UnitConfigId = x.UnitConfigId, UnitDailyConfigId = p.Id, Position = ixc + 1 }) :
                    new List<UnitConfigUnitDailyConfig>()))
                 .ForMember(p => p.RelatedUnitDailyConfigs, opt => opt.MapFrom(p => p.RelatedUnitDailyConfigs != null ?
                    p.RelatedUnitDailyConfigs.Select((x, ixc) => new RelatedUnitDailyConfigs() { UnitsDailyConfigId = p.Id, RelatedUnitsDailyConfigId = x.Id, Position = ixc + 1 }) :
                    new List<RelatedUnitDailyConfigs>()))
                  .ForMember(p => p.IsConverted, opt => opt.Ignore());
        }
    }
}