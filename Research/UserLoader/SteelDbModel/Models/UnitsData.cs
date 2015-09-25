using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class UnitsData
    {
        public UnitsData()
        {
            this.UnitsDailyDatas = new List<UnitsDailyData>();
        }

        public int Id { get; set; }
        public System.DateTime RecordTimestamp { get; set; }
        public int UnitConfigId { get; set; }
        public Nullable<decimal> Value { get; set; }
        public bool IsApproved { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public int ShiftId { get; set; }
        public virtual UnitsConfig UnitsConfig { get; set; }
        public virtual UnitsManualData UnitsManualData { get; set; }
        public virtual ICollection<UnitsDailyData> UnitsDailyDatas { get; set; }
    }
}