namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    public class UnitsDailyConfigViewModel : IMapFrom<UnitsDailyConfig>, IHaveCustomMappings,IEntity
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
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitsDailyConfig, UnitsDailyConfigViewModel>()
                    .ForMember(p => p.DailyProductTypeId, opt => opt.MapFrom(p => p.DailyProductTypeId == null ? 0 : (int)p.DailyProductTypeId));
            configuration.CreateMap<UnitsDailyConfigViewModel, UnitsDailyConfig>()
                 .ForMember(p => p.DailyProductTypeId, opt => opt.MapFrom(p => p.DailyProductTypeId == 0 ? null : (Nullable<int>)p.DailyProductTypeId));
        }
    }
}