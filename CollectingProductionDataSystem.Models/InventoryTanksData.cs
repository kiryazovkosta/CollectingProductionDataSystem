namespace CollectingProductionDataSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using CollectingProductionDataSystem.Models;

    [Table("InventoryTanksData")]
    public class InventoryTanksData
    {
        [Key, Column(Order = 0)]
        public int TankId { get; set; }

        [Key, Column(Order = 1)]
        public DateTime RecordTimestamp { get; set; }

        public string ExciseStoreId { get; set; }

        public int ParkId { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string ProductName { get; set; }

        public decimal? LiquidLevel { get; set; }

        public decimal? TotalObservableVolume { get; set; }

        public decimal? ProductLevel { get; set; }

        public decimal? GrossObservableVolume { get; set; }

        public decimal? ObservableDensity { get; set; }

        public decimal? GrossStandardVolume { get; set; }

        public decimal? AverageTemperature { get; set; }

        public decimal? NetStandardVolume { get; set; }

        public decimal? ReferenceDensity { get; set; }

        public decimal? WeightInAir { get; set; }

        public decimal? WeightInVacuum { get; set; }

        public decimal? FreeWaterLevel { get; set; }

        public decimal? FreeWaterVolume { get; set; }

        public decimal? MaxVolume { get; set; }

        public decimal? AvailableRoom { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastUpdateTimestamp { get; set; }

        public virtual InventoryTank InventoryTank { get; set; }
    }
}
