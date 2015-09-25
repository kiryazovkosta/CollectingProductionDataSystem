using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class UnitsManualDailyData
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public int EditReasonId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual EditReason EditReason { get; set; }
        public virtual UnitsDailyData UnitsDailyData { get; set; }
    }
}
