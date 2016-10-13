using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Transactions;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class MeasuringPointConfigViewModel : IMapFrom<MeasuringPointConfig>, IHaveCustomMappings, IEntity
    {

        [Required]
        [Display(Name = "Id", ResourceType = typeof(Resources.Layout))]
        public int Id { get; set; }

        [Required]
        [Display(Name = "ControlPoint", ResourceType = typeof(Resources.Layout))]
        public string ControlPoint { get; set; }

        [Required]
        [Display(Name = "MeasuringPointName", ResourceType = typeof(Resources.Layout))]
        public string MeasuringPointName { get; set; }

        [Required]
        [Display(Name = "ZoneId", ResourceType = typeof(Resources.Layout))]
        public int ZoneId { get; set; }

        [Required]
        [Display(Name = "EngineeringUnitMass", ResourceType = typeof(Resources.Layout))]
        public string EngineeringUnitMass { get; set; }

        [Required]
        [Display(Name = "EngineeringUnitVolume", ResourceType = typeof(Resources.Layout))]
        public string EngineeringUnitVolume { get; set; }

        [Required]
        [Display(Name = "EngineeringUnitDensity", ResourceType = typeof(Resources.Layout))]
        public string EngineeringUnitDensity { get; set; }

        [Required]
        [Display(Name = "EngineeringUnitTemperature", ResourceType = typeof(Resources.Layout))]
        public string EngineeringUnitTemperature { get; set; }

        [Required]
        [Display(Name = "FactorMass", ResourceType = typeof(Resources.Layout))]
        public decimal FactorMass { get; set; }

        [Required]
        [Display(Name = "FactorVolume", ResourceType = typeof(Resources.Layout))]
        public decimal FactorVolume { get; set; }

        [Required]
        [Display(Name = "FactorDensity", ResourceType = typeof(Resources.Layout))]
        public decimal FactorDensity { get; set; }

        [Required]
        [Display(Name = "FactorTemperature", ResourceType = typeof(Resources.Layout))]
        public decimal FactorTemperature { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossObservableVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossObservableVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossObservableVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossObservableVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossObservableVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossObservableVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossObservableVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossObservableVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossStandardVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossStandardVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossStandardVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossStandardVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossStandardVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossStandardVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossStandardVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossStandardVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginMassLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginMassLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginMassHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginMassHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndMassLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndMassLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndMassHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndMassHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossObservableCommonVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossObservableCommonVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossObservableCommonVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossObservableCommonVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossObservableCommonVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossObservableCommonVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossObservableCommonVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossObservableCommonVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossStandardCommonVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossStandardCommonVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossStandardCommonVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossStandardCommonVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossStandardCommonVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossStandardCommonVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossStandardCommonVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossStandardCommonVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginCommonMassLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginCommonMassLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginCommonMassHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginCommonMassHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndCommonMassLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndCommonMassLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndCommonMassHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndCommonMassHighExtreme { get; set; }

        [Required]
        [Display(Name = "GrossObservableVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossObservableVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "GrossObservableVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossObservableVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "GrossStandardVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossStandardVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "GrossStandardVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossStandardVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "MassLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal MassLowExtreme { get; set; }

        [Required]
        [Display(Name = "MassHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal MassHighExtreme { get; set; }

        [Required]
        [Display(Name = "AverageObservableDensityLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageObservableDensityLowExtreme { get; set; }

        [Required]
        [Display(Name = "AverageObservableDensityHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageObservableDensityHighExtreme { get; set; }

        [Required]
        [Display(Name = "AverageReferenceDensityLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageReferenceDensityLowExtreme { get; set; }

        [Required]
        [Display(Name = "AverageReferenceDensityHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageReferenceDensityHighExtreme { get; set; }

        [Required]
        [Display(Name = "AverageTemperatureLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageTemperatureLowExtreme { get; set; }

        [Required]
        [Display(Name = "AverageTemperatureHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageTemperatureHighExtreme { get; set; }

        [Required]
        [Display(Name = "GrossObservableVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossObservableVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "GrossObservableVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossObservableVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "GrossStandardVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossStandardVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "GrossStandardVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossStandardVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "MassReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal MassReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "MassReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal MassReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "AverageObservableDensityReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageObservableDensityReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "AverageObservableDensityReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageObservableDensityReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "AverageReferenceDensityReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageReferenceDensityReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "AverageReferenceDensityReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageReferenceDensityReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "AverageTemperatureReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageTemperatureReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "AverageTemperatureReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageTemperatureReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossObservableVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossObservableVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossObservableVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossObservableVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossObservableVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossObservableVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossObservableVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossObservableVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossStandardVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossStandardVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossStandardVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossStandardVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossStandardVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossStandardVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossStandardVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossStandardVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginMassReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginMassReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginMassReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginMassReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndMassReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndMassReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndMassReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndMassReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossObservableCommonVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossObservableCommonVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossObservableCommonVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossObservableCommonVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossObservableCommonVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossObservableCommonVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossObservableCommonVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossObservableCommonVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossStandardCommonVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossStandardCommonVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginGrossStandardCommonVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginGrossStandardCommonVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossStandardCommonVolumeReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossStandardCommonVolumeReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndGrossStandardCommonVolumeReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndGrossStandardCommonVolumeReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginCommonMassReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginCommonMassReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerBeginCommonMassReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerBeginCommonMassReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndCommonMassReverseLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndCommonMassReverseLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalizerEndCommonMassReverseHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalizerEndCommonMassReverseHighExtreme { get; set; }

        [Required]
        [Display(Name = "Direction", ResourceType = typeof(Resources.Layout))]
        public int DirectionId { get; set; }

        [Display(Name = "FlowDirection", ResourceType = typeof(Resources.Layout))]
        public int FlowDirection { get; set; }

        [Display(Name = "WeightScaleNumber", ResourceType = typeof(Resources.Layout))]
        public string WeightScaleNumber { get; set; }

        [Required]
        [Display(Name = "ControlPoint", ResourceType = typeof(Resources.Layout))]
        public bool IsInternalPoint { get; set; }

        [Required]
        [Display(Name = "TransportTypeId", ResourceType = typeof(Resources.Layout))]
        public int TransportTypeId { get; set; }

        //[Required]
        [Display(Name = "ActiveTransactionStatusTag", ResourceType = typeof(Resources.Layout))]
        public string ActiveTransactionStatusTag { get; set; }

        //[Required]
        [Display(Name = "ActiveTransactionProductTag", ResourceType = typeof(Resources.Layout))]
        public string ActiveTransactionProductTag { get; set; }

        //[Required]
        [Display(Name = "ActiveTransactionMassTag", ResourceType = typeof(Resources.Layout))]
        public string ActiveTransactionMassTag { get; set; }

        //[Required]
        [Display(Name = "TotalizerCurrentValueTag", ResourceType = typeof(Resources.Layout))]
        public string TotalizerCurrentValueTag { get; set; }

        //[Required]
        [Display(Name = "ActiveTransactionMassReverseTag", ResourceType = typeof(Resources.Layout))]
        public string ActiveTransactionMassReverseTag { get; set; }

        [Display(Name = "MassCorrectionFactor", ResourceType = typeof(Resources.Layout))]
        public string MassCorrectionFactor { get; set; }

        //[UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }

        [UIHint("RelatedMeasuringPointConfigEditor")]
        [Display(Name = "RelatedMeasuringPointConfigs", ResourceType = typeof(Resources.Layout))]
        public virtual ICollection<RelatedMeasuringPointConfigsViewModel> RelatedMeasuringPointConfigs { get; set; } = new HashSet<RelatedMeasuringPointConfigsViewModel>();

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<MeasuringPointConfig, MeasuringPointConfigViewModel>()
                .ForMember(p => p.FlowDirection, opt => opt.NullSubstitute(0));
            //.ForMember(p => p.DailyProductTypeId, opt => opt.MapFrom(p => p.DailyProductTypeId == null ? 0 : (int)p.DailyProductTypeId))
            //.ForMember(p => p.UnitConfigUnitDailyConfigs, opt => opt.MapFrom(p => p.UnitConfigUnitDailyConfigs.OrderBy(x => x.Position)))
            //.ForMember(p => p.RelatedMeasuringPointConfigs, opt => opt.MapFrom(p => p.RelatedMeasuringPointConfigs.OrderBy(x => x.Position)));


            configuration.CreateMap<MeasuringPointConfigViewModel, MeasuringPointConfig>()
                 .ForMember(p => p.RelatedMeasuringPointConfigs, opt => opt.MapFrom(p => p.RelatedMeasuringPointConfigs != null ?
                    p.RelatedMeasuringPointConfigs.Select((x, ixc) => new RelatedMeasuringPointConfigs()
                    { MeasuringPointConfigId = p.Id, RelatedMeasuringPointConfigId = x.Id, Position = ixc + 1 }) :
                    new List<RelatedMeasuringPointConfigs>()))
                    .ForMember(p => p.FlowDirection, opt => opt.MapFrom(p => p.FlowDirection == 0 ? null : (int?) p.FlowDirection));
        }

    }
}