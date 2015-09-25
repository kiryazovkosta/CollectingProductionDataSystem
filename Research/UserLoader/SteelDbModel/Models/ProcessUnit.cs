using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class ProcessUnit
    {
        public ProcessUnit()
        {
            this.ProductionPlanConfigs = new List<ProductionPlanConfig>();
            this.UnitsDailyConfigs = new List<UnitsDailyConfig>();
            this.UnitsConfigs = new List<UnitsConfig>();
            this.AspNetUsers = new List<AspNetUser>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int FactoryId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual Factory Factory { get; set; }
        public virtual ICollection<ProductionPlanConfig> ProductionPlanConfigs { get; set; }
        public virtual ICollection<UnitsDailyConfig> UnitsDailyConfigs { get; set; }
        public virtual ICollection<UnitsConfig> UnitsConfigs { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
