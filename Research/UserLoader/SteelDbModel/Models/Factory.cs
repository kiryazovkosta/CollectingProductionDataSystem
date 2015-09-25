using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class Factory
    {
        public Factory()
        {
            this.ProcessUnits = new List<ProcessUnit>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public Nullable<int> PlantId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual ICollection<ProcessUnit> ProcessUnits { get; set; }
    }
}
