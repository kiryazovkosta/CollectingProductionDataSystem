namespace CollectingProductionDataSystem.Models
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using CollectingProductionDataSystem.Common;
    using System.ComponentModel;
    
    public class User : IdentityUser, IActiveEntity
    {
        public User()
        {
            this.IsActive = true;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            return userIdentity;
        }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
