namespace CollectingProductionDataSystem.Models.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Productions;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>, IAuditInfo, IDeletableEntity, IEntity
    {
        private ICollection<ApplicationUserProcessUnit> applicationUserProcessUnits;
        //private ICollection<Park> parks;
        private IEnumerable<ApplicationRole> userRoles;
        private ICollection<ApplicationUserPark> applicationUserParks;

        #region IAuditInfo_IDeletableEntity
        /// <summary>
        /// Gets or sets the is deleted.
        /// </summary>
        /// <value>The is deleted.</value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the deleted on.
        /// </summary>
        /// <value>The deleted on.</value>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Gets or sets the deleted from.
        /// </summary>
        /// <value>The deleted from.</value>
        public string DeletedFrom { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the preserve created on.
        /// </summary>
        /// <value>The preserve created on.</value>
        public bool PreserveCreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the modified on.
        /// </summary>
        /// <value>The modified on.</value>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the created from.
        /// </summary>
        /// <value>The created from.</value>
        public string CreatedFrom { get; set; }

        /// <summary>
        /// Gets or sets the modified from.
        /// </summary>
        /// <value>The modified from.</value>
        public string ModifiedFrom { get; set; }
        #endregion

        public ApplicationUser()
        {
            this.applicationUserProcessUnits = new HashSet<ApplicationUserProcessUnit>();
            this.userRoles = new List<ApplicationRole>();
            this.applicationUserParks = new HashSet<ApplicationUserPark>();
            this.CreatedOn = DateTime.Now;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Occupation { get; set; }

        public bool IsChangePasswordRequired { get; set; }

        public virtual ICollection<ApplicationUserProcessUnit> ApplicationUserProcessUnits
        {
            get { return this.applicationUserProcessUnits; }
            set { this.applicationUserProcessUnits = value; }
        }

        public virtual ICollection<ApplicationUserPark> ApplicationUserParks
        {
            get { return this.ApplicationUserParks; }
            set { this.ApplicationUserParks = value; }
        }

        [NotMapped]
        public IEnumerable<ApplicationRole> UserRoles
        {
            get { return this.userRoles; }
            set { this.userRoles = value; }
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                StringBuilder userName = new StringBuilder(150);
                if (!string.IsNullOrEmpty((this.FirstName ?? string.Empty).Trim()))
                {
                    userName.Append(this.FirstName);
                    userName.Append(" ");
                }
                if (!string.IsNullOrEmpty((this.MiddleName ?? string.Empty).Trim()))
                {
                    userName.Append(this.MiddleName);
                    userName.Append(" ");
                }
                if (!string.IsNullOrEmpty((this.LastName ?? string.Empty).Trim()))
                {
                    userName.Append(this.LastName);
                }
                var resultName = userName.ToString().TrimEnd();

                if (string.IsNullOrEmpty(resultName))
                {
                    return this.UserName;
                }

                return resultName;
            }
        }
    }
}
