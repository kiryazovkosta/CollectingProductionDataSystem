namespace CollectingProductionDataSystem.Web.ViewModels.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Models.Identity;

    public class RoleEditViewModel
    {
        public ApplicationRole Role { get; set; }

        public bool IsAvailableForAdministrators { get { return Role.IsAvailableForAdministrators; } }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }
}