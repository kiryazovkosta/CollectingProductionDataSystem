using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class TankData
    {
        public int Id { get; set; }
        public System.DateTime RecordTimestamp { get; set; }
        public int TankConfigId { get; set; }
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
        public Nullable<decimal> UnusableResidueLevel { get; set; }
        public bool IsApproved { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual Product Product { get; set; }
        public virtual TankConfig TankConfig { get; set; }
        public virtual TanksManualData TanksManualData { get; set; }
    }
}
