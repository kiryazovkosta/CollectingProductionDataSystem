using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class UnitsDailyConfig
    {
        public UnitsDailyConfig()
        {
            this.UnitsDailyDatas = new List<UnitsDailyData>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProcessUnitId { get; set; }
        public int ProductTypeId { get; set; }
        public int MeasureUnitId { get; set; }
        public string AggregationFormula { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public string AggregationMembers { get; set; }
        public bool AggregationCurrentLevel { get; set; }
        public virtual MeasureUnit MeasureUnit { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<UnitsDailyData> UnitsDailyDatas { get; set; }
    }
}
