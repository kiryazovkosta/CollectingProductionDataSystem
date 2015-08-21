using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Identity;

namespace CollectingProductionDataSystem.Models
{
    public class UserProfile
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<ApplicationRole> Roles { get; set; }
    }
}
