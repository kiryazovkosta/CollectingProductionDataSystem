using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class Ikunk
    {
        public Ikunk()
        {
            this.Zones = new List<Zone>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual ICollection<Zone> Zones { get; set; }
    }
}
