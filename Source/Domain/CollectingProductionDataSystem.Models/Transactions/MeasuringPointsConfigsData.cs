using CollectingProductionDataSystem.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Nomenclatures;

namespace CollectingProductionDataSystem.Models.Transactions
{
    public partial class MeasuringPointsConfigsData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public string ExciseStoreId { get; set; }
        public int MeasuringPointId { get; set; }
        public int ZoneId { get; set; }
        public long TransactionNumber { get; set; }
        public int RowId { get; set; }
        public int? BaseProductNumber { get; set; }
        public int? BaseProductType { get; set; }
        public string BaseProductName { get; set; }
        public int? ProductNumber { get; set; }
        public int? ProductType { get; set; }
        public string ProductName { get; set; }
        public int? FlowDirection { get; set; }
        public DateTime TransactionBeginTime { get; set; }
        public DateTime TransactionEndTime { get; set; }
        public string EngineeringUnitMass { get; set; }
        public string EngineeringUnitVolume { get; set; }
        public string EngineeringUnitDensity { get; set; }
        public string EngineeringUnitTemperature { get; set; }
        public decimal? FactorMass { get; set; }
        public decimal? FactorVolume { get; set; }
        public decimal? FactorDensity { get; set; }
        public decimal? FactorTemperature { get; set; }
        public decimal? TotalizerBeginGrossObservableVolume { get; set; }
        public decimal? TotalizerEndGrossObservableVolume { get; set; }
        public decimal? TotalizerBeginGrossStandardVolume { get; set; }
        public decimal? TotalizerEndGrossStandardVolume { get; set; }
        public decimal? TotalizerBeginMass { get; set; }
        public decimal? TotalizerEndMass { get; set; }
        public decimal? TotalizerBeginGrossObservableCommonVolume { get; set; }
        public decimal? TotalizerEndGrossObservableCommonVolume { get; set; }
        public decimal? TotalizerBeginGrossStandardCommonVolume { get; set; }
        public decimal? TotalizerEndGrossStandardCommonVolume { get; set; }
        public decimal? TotalizerBeginCommonMass { get; set; }
        public decimal? TotalizerEndCommonMass { get; set; }
        public decimal? GrossObservableVolume { get; set; }
        public decimal? GrossStandardVolume { get; set; }
        public decimal? Mass { get; set; }
        public decimal? AverageObservableDensity { get; set; }
        public decimal? AverageReferenceDensity { get; set; }
        public decimal? AverageTemperature { get; set; }
        public decimal? TotalizerBeginGrossObservableVolumeReverse { get; set; }
        public decimal? TotalizerEndGrossObservableVolumeReverse { get; set; }
        public decimal? TotalizerBeginGrossStandardVolumeReverse { get; set; }
        public decimal? TotalizerEndGrossStandardVolumeReverse { get; set; }
        public decimal? TotalizerBeginMassReverse { get; set; }
        public decimal? TotalizerEndMassReverse { get; set; }
        public decimal? TotalizerBeginGrossObservableCommonVolumeReverse { get; set; }
        public decimal? TotalizerEndGrossObservableCommonVolumeReverse { get; set; }
        public decimal? TotalizerBeginGrossStandardCommonVolumeReverse { get; set; }
        public decimal? TotalizerEndGrossStandardCommonVolumeReverse { get; set; }
        public decimal? TotalizerBeginCommonMassReverse { get; set; }
        public decimal? TotalizerEndCommonMassReverse { get; set; }
        public decimal? GrossObservableVolumeReverse { get; set; }
        public decimal? GrossStandardVolumeReverse { get; set; }
        public decimal? MassReverse { get; set; }
        public decimal? AverageObservableDensityReverse { get; set; }
        public decimal? AverageReferenceDensityReverse { get; set; }
        public decimal? AverageTemperatureReverse { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public long? ResultId { get; set; }
        public long? AdditiveResultId { get; set; }
        public DateTime? TamasCreateTimestamp { get; set; }
        public string TamasRecipeTransId { get; set; }
        public long? EpksSequenceNumber { get; set; }
        public long? MartaRowNum { get; set; }
        public decimal? AlcoholContent { get; set; }
        public decimal? AlcoholContentReverse { get; set; }
        public long? BatchId { get; set; }
        public int MeasuringPointConfigId { get; set; }
        public virtual MeasuringPointConfig MeasuringPointConfig { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
