using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class TanksManualData
    {
        public int Id { get; set; }
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
        public int EditReasonId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual EditReason EditReason { get; set; }
        public virtual TankData TankData { get; set; }
    }
}
