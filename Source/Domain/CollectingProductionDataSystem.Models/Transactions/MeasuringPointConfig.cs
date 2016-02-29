namespace CollectingProductionDataSystem.Models.Transactions
{
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public partial class MeasuringPointConfig : DeletableEntity, IEntity
    {
        private ICollection<RelatedMeasuringPointConfigs> relatedMeasuringPointConfigs;
        private ICollection<MeasuringPointProductsData> measuringPointProductsDatas;
        private ICollection<MeasuringPointProductsConfig> measuringPointProductsConfigs;

        public MeasuringPointConfig()
        {
            this.MeasuringPointsConfigsDatas = new HashSet<MeasuringPointsConfigsData>();
            this.ActiveTransactionsDatas = new HashSet<ActiveTransactionsData>();
            this.relatedMeasuringPointConfigs = new HashSet<RelatedMeasuringPointConfigs>();
            this.measuringPointProductsDatas = new HashSet<MeasuringPointProductsData>();
            this.measuringPointProductsConfigs = new HashSet<MeasuringPointProductsConfig>();
        }
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string ControlPoint { get; set; }
        public string MeasuringPointName { get; set; }
        public string EngineeringUnitMass { get; set; }
        public string EngineeringUnitVolume { get; set; }
        public string EngineeringUnitDensity { get; set; }
        public string EngineeringUnitTemperature { get; set; }
        public decimal FactorMass { get; set; }
        public decimal FactorVolume { get; set; }
        public decimal FactorDensity { get; set; }
        public decimal FactorTemperature { get; set; }
        public decimal TotalizerBeginGrossObservableVolumeLowExtreme { get; set; }
        public decimal TotalizerBeginGrossObservableVolumeHighExtreme { get; set; }
        public decimal TotalizerEndGrossObservableVolumeLowExtreme { get; set; }
        public decimal TotalizerEndGrossObservableVolumeHighExtreme { get; set; }
        public decimal TotalizerBeginGrossStandardVolumeLowExtreme { get; set; }
        public decimal TotalizerBeginGrossStandardVolumeHighExtreme { get; set; }
        public decimal TotalizerEndGrossStandardVolumeLowExtreme { get; set; }
        public decimal TotalizerEndGrossStandardVolumeHighExtreme { get; set; }
        public decimal TotalizerBeginMassLowExtreme { get; set; }
        public decimal TotalizerBeginMassHighExtreme { get; set; }
        public decimal TotalizerEndMassLowExtreme { get; set; }
        public decimal TotalizerEndMassHighExtreme { get; set; }
        public decimal TotalizerBeginGrossObservableCommonVolumeLowExtreme { get; set; }
        public decimal TotalizerBeginGrossObservableCommonVolumeHighExtreme { get; set; }
        public decimal TotalizerEndGrossObservableCommonVolumeLowExtreme { get; set; }
        public decimal TotalizerEndGrossObservableCommonVolumeHighExtreme { get; set; }
        public decimal TotalizerBeginGrossStandardCommonVolumeLowExtreme { get; set; }
        public decimal TotalizerBeginGrossStandardCommonVolumeHighExtreme { get; set; }
        public decimal TotalizerEndGrossStandardCommonVolumeLowExtreme { get; set; }
        public decimal TotalizerEndGrossStandardCommonVolumeHighExtreme { get; set; }
        public decimal TotalizerBeginCommonMassLowExtreme { get; set; }
        public decimal TotalizerBeginCommonMassHighExtreme { get; set; }
        public decimal TotalizerEndCommonMassLowExtreme { get; set; }
        public decimal TotalizerEndCommonMassHighExtreme { get; set; }
        public decimal GrossObservableVolumeLowExtreme { get; set; }
        public decimal GrossObservableVolumeHighExtreme { get; set; }
        public decimal GrossStandardVolumeLowExtreme { get; set; }
        public decimal GrossStandardVolumeHighExtreme { get; set; }
        public decimal MassLowExtreme { get; set; }
        public decimal MassHighExtreme { get; set; }
        public decimal AverageObservableDensityLowExtreme { get; set; }
        public decimal AverageObservableDensityHighExtreme { get; set; }
        public decimal AverageReferenceDensityLowExtreme { get; set; }
        public decimal AverageReferenceDensityHighExtreme { get; set; }
        public decimal AverageTemperatureLowExtreme { get; set; }
        public decimal AverageTemperatureHighExtreme { get; set; }
        public decimal GrossObservableVolumeReverseLowExtreme { get; set; }
        public decimal GrossObservableVolumeReverseHighExtreme { get; set; }
        public decimal GrossStandardVolumeReverseLowExtreme { get; set; }
        public decimal GrossStandardVolumeReverseHighExtreme { get; set; }
        public decimal MassReverseLowExtreme { get; set; }
        public decimal MassReverseHighExtreme { get; set; }
        public decimal AverageObservableDensityReverseLowExtreme { get; set; }
        public decimal AverageObservableDensityReverseHighExtreme { get; set; }
        public decimal AverageReferenceDensityReverseLowExtreme { get; set; }
        public decimal AverageReferenceDensityReverseHighExtreme { get; set; }
        public decimal AverageTemperatureReverseLowExtreme { get; set; }
        public decimal AverageTemperatureReverseHighExtreme { get; set; }
        public decimal TotalizerBeginGrossObservableVolumeReverseLowExtreme { get; set; }
        public decimal TotalizerBeginGrossObservableVolumeReverseHighExtreme { get; set; }
        public decimal TotalizerEndGrossObservableVolumeReverseLowExtreme { get; set; }
        public decimal TotalizerEndGrossObservableVolumeReverseHighExtreme { get; set; }
        public decimal TotalizerBeginGrossStandardVolumeReverseLowExtreme { get; set; }
        public decimal TotalizerBeginGrossStandardVolumeReverseHighExtreme { get; set; }
        public decimal TotalizerEndGrossStandardVolumeReverseLowExtreme { get; set; }
        public decimal TotalizerEndGrossStandardVolumeReverseHighExtreme { get; set; }
        public decimal TotalizerBeginMassReverseLowExtreme { get; set; }
        public decimal TotalizerBeginMassReverseHighExtreme { get; set; }
        public decimal TotalizerEndMassReverseLowExtreme { get; set; }
        public decimal TotalizerEndMassReverseHighExtreme { get; set; }
        public decimal TotalizerBeginGrossObservableCommonVolumeReverseLowExtreme { get; set; }
        public decimal TotalizerBeginGrossObservableCommonVolumeReverseHighExtreme { get; set; }
        public decimal TotalizerEndGrossObservableCommonVolumeReverseLowExtreme { get; set; }
        public decimal TotalizerEndGrossObservableCommonVolumeReverseHighExtreme { get; set; }
        public decimal TotalizerBeginGrossStandardCommonVolumeReverseLowExtreme { get; set; }
        public decimal TotalizerBeginGrossStandardCommonVolumeReverseHighExtreme { get; set; }
        public decimal TotalizerEndGrossStandardCommonVolumeReverseLowExtreme { get; set; }
        public decimal TotalizerEndGrossStandardCommonVolumeReverseHighExtreme { get; set; }
        public decimal TotalizerBeginCommonMassReverseLowExtreme { get; set; }
        public decimal TotalizerBeginCommonMassReverseHighExtreme { get; set; }
        public decimal TotalizerEndCommonMassReverseLowExtreme { get; set; }
        public decimal TotalizerEndCommonMassReverseHighExtreme { get; set; }
        public int? FlowDirection { get; set; }
        public int DirectionId { get; set; }
        public virtual Direction Direction { get; set; }
        public string WeightScaleNumber{ get; set; }
        public bool IsInternalPoint { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<MeasuringPointsConfigsData> MeasuringPointsConfigsDatas { get; set; }
        public int TransportTypeId { get; set; }
        public virtual TransportType TransportType { get; set; }
        public string ActiveTransactionStatusTag { get; set; }
        public string ActiveTransactionProductTag { get; set; }
        public string ActiveTransactionMassTag { get; set; }
        public string TotalizerCurrentValueTag { get; set; }
        public string ActiveTransactionMassReverseTag { get; set; }
        public string MassCorrectionFactor { get; set; }
        public bool IsUsedPhdTotalizers { get; set; }
        public bool IsUsedInTradeReport { get; set; }
        public bool IsUsedInProductionReport { get; set; }

        public virtual ICollection<ActiveTransactionsData> ActiveTransactionsDatas { get; set; }
        public virtual ICollection<RelatedMeasuringPointConfigs> RelatedMeasuringPointConfigs
        {
            get { return this.relatedMeasuringPointConfigs; }
            set { this.relatedMeasuringPointConfigs = value; }
        }
        public virtual ICollection<MeasuringPointProductsData> MeasuringPointProductsDatas
        {
            get { return this.measuringPointProductsDatas; }
            set { this.measuringPointProductsDatas = value; }
        }
        public virtual ICollection<MeasuringPointProductsConfig> MeasuringPointProductsConfigs 
        {
            get { return this.measuringPointProductsConfigs; }
            set { this.measuringPointProductsConfigs = value; }
        }
    }
}
