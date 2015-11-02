using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class UnitsInspectionData
    {
        public int Id { get; set; }
        public System.DateTime RecordTimestamp { get; set; }
        public int UnitConfigId { get; set; }
        public decimal Value { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual UnitsConfig UnitsConfig { get; set; }
    }
}
