namespace CollectingProductionDataSystem.Models.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Contracts;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser() 
        {
            this.UserRoles = new HashSet<ApplicationRole>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<ApplicationRole> UserRoles { get; set; }

        public string NickName { get; set; }
    }
}