using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class Park
    {
        public Park()
        {
            this.TankConfigs = new List<TankConfig>();
            this.AspNetUsers = new List<AspNetUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int AreaId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual Area Area { get; set; }
        public virtual ICollection<TankConfig> TankConfigs { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
