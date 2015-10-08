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

        [Display(Name = "AggregateParameter", ResourceType = typeof(Resources.Layout))]
        public string AggregateParameter { get; set; }

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
        public decimal? MaximumCost { get; set; }

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
        
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitConfig, UnitConfigViewModel>()
                .ForMember(p => p.ShiftProductTypeId, opt => opt.MapFrom(p => p.ShiftProductTypeId == null ? 0 : (int)p.ShiftProductTypeId));

            configuration.CreateMap<UnitConfigViewModel, UnitConfig>()
                .ForMember(p => p.ShiftProductTypeId, opt => opt.MapFrom(p => p.ShiftProductTypeId == 0 ? null : (Nullable<int>)p.ShiftProductTypeId));
        }
    }
}