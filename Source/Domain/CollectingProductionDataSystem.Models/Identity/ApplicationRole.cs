namespace CollectingProductionDataSystem.Models.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationRole : IdentityRole<int, UserRoleIntPk>,IAuditInfo,IDeletableEntity, IEntity
    {
        private ICollection<ApplicationUser> roleUsers;
        public ApplicationRole()
        {
            this.roleUsers = new HashSet<ApplicationUser>();
        }

        public ApplicationRole(string name)
            :this()
        {
            Name = name;
        }

        public ApplicationRole(string name, bool isAvailableForAdministrators) 
            :this(name)
        {
            this.IsAvailableForAdministrators = isAvailableForAdministrators;
        }

         public bool IsAvailableForAdministrators { get; set; }

         public virtual ICollection<ApplicationUser> RoleUsers 
         {
             get { return this.roleUsers; }
             set { this.roleUsers = value; }
         }

         #region IAuditInfo_IDeletableEntity
         [StringLength(250)]
         public string Description { get; set; }
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
    }
}