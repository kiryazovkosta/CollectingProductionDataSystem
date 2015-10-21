namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    public class UnitConfigViewModel : IMapFrom<UnitConfig>, IHaveCustomMappings, IEntity
    {
        public UnitConfigViewModel() 
        {
            this.RelatedUnitConfigs = new HashSet<RelatedUnitConfigsViewModel>();
        }

        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Position", ResourceType = typeof(Resources.Layout))]
        public string Position { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product", ResourceType = typeof(Resources.Layout))]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        // TODO: Check this out
        public int ProcessUnitId { get; set; }

        [Required]
        [Display(Name = "Direction", ResourceType = typeof(Resources.Layout))]
        public int DirectionId { get; set; }

        [Required]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public int MeasureUnitId { get; set; }

        [Required]
        [Display(Name = "MaterialType", ResourceType = typeof(Resources.Layout))]
        public int MaterialTypeId { get; set; }

        [Required]
        [Display(Name = "CollectingDataMechanism", ResourceType = typeof(Resources.Layout))]
        public string CollectingDataMechanism { get; set; }

        [Display(Name = "CalculatedFormula", ResourceType = typeof(Resources.Layout))]
        public string CalculatedFormula { get; set; }

        [Display(Name = "AggregateGroup", ResourceType = typeof(Resources.Layout))]
        public string AggregateGroup { get; set; }

        [Display(Name = "AggregationMembers", ResourceType = typeof(Resources.Layout))]
        public string AggregationMembers { get; set; }

        [Required]
        [Display(Name = "IsCalculated", ResourceType = typeof(Resources.Layout))]
        public bool IsCalculated { get; set; }

        [Display(Name = "PreviousShiftTag", ResourceType = typeof(Resources.Layout))]
        public string PreviousShiftTag { get; set; }

        //[Display(Name = "CurrentInspectionDataTag", ResourceType = typeof(Resources.Layout))]
        //public string CurrentInspectionDataTag { get; set; }

        [Display(Name = "Notes", ResourceType = typeof(Resources.Layout))]
        public string Notes { get; set; }

        [Display(Name = "MaximumCost", ResourceType = typeof(Resources.Layout))]
        public decimal? MaximumFlow { get; set; }

        [Display(Name = "EstimatedDensity", ResourceType = typeof(Resources.Layout))]
        public decimal? EstimatedDensity { get; set; }

        [Display(Name = "EstimatedPressure", ResourceType = typeof(Resources.Layout))]
        public decimal? EstimatedPressure { get; set; }

        [Display(Name = "EstimatedTemperature", ResourceType = typeof(Resources.Layout))]
        public decimal? EstimatedTemperature { get; set; }

        [Display(Name = "EstimatedCompressibilityFactor", ResourceType = typeof(Resources.Layout))]
        public decimal? EstimatedCompressibilityFactor { get; set; }

        [Display(Name = "ShiftProductType", ResourceType = typeof(Resources.Layout))]
        public int ShiftProductTypeId { get; set; }

        [Display(Name = "IsEditable", ResourceType = typeof(Resources.Layout))]
        public bool IsEditable { get; set; }

        [UIHint("RelatedUnitConfigsEditor")]
        [Display(Name = "RelatedUnitConfigs", ResourceType = typeof(Resources.Layout))]
        public ICollection<RelatedUnitConfigsViewModel> RelatedUnitConfigs { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitConfig, UnitConfigViewModel>()
                .ForMember(p => p.ShiftProductTypeId, opt => opt.MapFrom(p => p.ShiftProductTypeId == null ? 0 : (int)p.ShiftProductTypeId))
                .ForMember(p => p.RelatedUnitConfigs, opt => opt.MapFrom(p=>p.RelatedUnitConfigs.OrderBy(x=>x.Position)));
                //.ForMember(p => p.RelatedUnitConfigs, opt => opt.MapFrom(
                //    p => (p.RelatedUnitConfigs != null) && (p.RelatedUnitConfigs.Count != 0) ?
                //    p.RelatedUnitConfigs : new HashSet<RelatedUnitConfigs> { new RelatedUnitConfigs() }));


            configuration.CreateMap<UnitConfigViewModel, UnitConfig>()
                .ForMember(p => p.ShiftProductTypeId, opt => opt.MapFrom(p => p.ShiftProductTypeId == 0 ? null : (Nullable<int>)p.ShiftProductTypeId))
                .ForMember(p => p.RelatedUnitConfigs, opt => opt.MapFrom(p => p.RelatedUnitConfigs != null ?
                    p.RelatedUnitConfigs.Select((x,ixd) => new RelatedUnitConfigs() { UnitConfigId = p.Id, RelatedUnitConfigId = x.Id, Position=ixd+1 }) :
                    new List<RelatedUnitConfigs>()))
                .ForMember(p => p.Direction, opt => opt.Ignore())
                .ForMember(p => p.MaterialType, opt => opt.Ignore())
                .ForMember(p => p.MeasureUnit, opt => opt.Ignore())
                .ForMember(p => p.ProcessUnit, opt => opt.Ignore())
                .ForMember(p => p.Product, opt => opt.Ignore())
                .ForMember(p => p.ShiftProductType, opt => opt.Ignore())
                .ForMember(p => p.UnitsDatas, opt => opt.Ignore())
                .ForMember(p => p.IsDeleted, opt => opt.Ignore())
                .ForMember(p => p.DeletedOn, opt => opt.Ignore())
                .ForMember(p => p.DeletedFrom, opt => opt.Ignore())
                .ForMember(p => p.CreatedOn, opt => opt.Ignore())
                .ForMember(p => p.PreserveCreatedOn, opt => opt.Ignore())
                .ForMember(p => p.ModifiedOn, opt => opt.Ignore())
                .ForMember(p => p.CreatedFrom, opt => opt.Ignore())
                .ForMember(p => p.ModifiedFrom, opt => opt.Ignore())
                .ForMember(p => p.UnitConfigUnitDailyConfigs, opt => opt.Ignore());
        }
    }
}