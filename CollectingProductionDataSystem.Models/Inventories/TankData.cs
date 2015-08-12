using System;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Nomenclatures;

namespace CollectingProductionDataSystem.Models.Inventories
{
    public partial class TankData: DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public System.DateTime RecordTimestamp { get; set; }
        public string ExciseStoreId { get; set; }
        public int ParkId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> LiquidLevel { get; set; }
        public Nullable<decimal> TotalObservableVolume { get; set; }
        public Nullable<decimal> ProductLevel { get; set; }
        public Nullable<decimal> GrossObservableVolume { get; set; }
        public Nullable<decimal> ObservableDensity { get; set; }
        public Nullable<decimal> GrossStandardVolume { get; set; }
        public Nullable<decimal> AverageTemperature { get; set; }
        public Nullable<decimal> NetStandardVolume { get; set; }
        public Nullable<decimal> ReferenceDensity { get; set; }
        public Nullable<decimal> WeightInAir { get; set; }
        public Nullable<decimal> WeightInVacuum { get; set; }
        public Nullable<decimal> FreeWaterLevel { get; set; }
        public Nullable<decimal> FreeWaterVolume { get; set; }
        public Nullable<decimal> MaxVolume { get; set; }
        public Nullable<decimal> AvailableRoom { get; set; }
        public Nullable<System.DateTime> LastUpdateTimestamp { get; set; }
        public virtual TankConfig InventoryTank { get; set; }
        public virtual Product Product { get; set; }
    }
}
