namespace CollectingProductionDataSystem.Models.Identity
{
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationRole : IdentityRole<int, UserRoleIntPk>, IEntity
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name)
        {
            Name = name;
        }

        public ApplicationRole(string name, bool isAvailableForAdministrators) 
            :this(name)
        {
            this.IsAvailableForAdministrators = isAvailableForAdministrators;
        }

         public bool IsAvailableForAdministrators { get; set; }

         [StringLength(250)]
         public string Description { get; set; }
    }
}