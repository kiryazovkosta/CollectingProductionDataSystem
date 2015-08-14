using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Contracts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CollectingProductionDataSystem.Models.Identity
{
    public class ApplicationRole: IdentityRole  
    {
        public ApplicationRole()
            :base()
        {
            this.RoleUsers = new HashSet<ApplicationUser>(); 
        }

        public ApplicationRole(string name) : base(name) 
        {
            this.RoleUsers = new HashSet<ApplicationUser>(); 
        }

        public string MyCustomField { get; set; }

        public virtual ICollection<ApplicationUser> RoleUsers { get; set; }
    }
}
