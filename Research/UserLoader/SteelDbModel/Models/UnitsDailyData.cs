using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class UnitsDailyData
    {
        public UnitsDailyData()
        {
            this.UnitsDatas = new List<UnitsData>();
        }

        public int Id { get; set; }
        public System.DateTime RecordTimestamp { get; set; }
        public int UnitsDailyConfigId { get; set; }
        public decimal Value { get; set; }
        public bool IsApproved { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public bool HasManualData { get; set; }
        public virtual UnitsDailyConfig UnitsDailyConfig { get; set; }
        public virtual UnitsManualDailyData UnitsManualDailyData { get; set; }
        public virtual ICollection<UnitsData> UnitsDatas { get; set; }
    }
}
