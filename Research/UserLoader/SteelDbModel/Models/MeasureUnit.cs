using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class MeasureUnit
    {
        public MeasureUnit()
        {
            this.UnitsDailyConfigs = new List<UnitsDailyConfig>();
            this.UnitsConfigs = new List<UnitsConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual ICollection<UnitsDailyConfig> UnitsDailyConfigs { get; set; }
        public virtual ICollection<UnitsConfig> UnitsConfigs { get; set; }
    }
}
