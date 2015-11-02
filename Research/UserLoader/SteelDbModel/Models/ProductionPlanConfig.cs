using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class ProductionPlanConfig
    {
        public int Id { get; set; }
        public decimal Percentages { get; set; }
        public int ProcessUnitId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public string Name { get; set; }
        public string QuantityPlanFormula { get; set; }
        public string QuantityPlanMembers { get; set; }
        public string QuantityFactFormula { get; set; }
        public string QuantityFactMembers { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
    }
}
