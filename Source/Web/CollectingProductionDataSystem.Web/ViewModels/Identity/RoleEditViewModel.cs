namespace CollectingProductionDataSystem.Web.ViewModels.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Identity;
    using Resources = App_GlobalResources.Resources;

    public class RoleEditViewModel
    {
       
        public ApplicationRole Role { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name="RoleName",ResourceType=typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name="Description",ResourceType=typeof(Resources.Layout))]
        public string Description { get; set; }

        public IEnumerable<ApplicationUserModel> Members { get; set; }
        public IEnumerable<ApplicationUserModel> NonMembers { get; set; }
    }
 
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationUserModel:IMapFrom<ApplicationUser>
    {
        [Required]
        public int Id { get; set; }
        [Display(Name="UserName",ResourceType=typeof(Resources.Layout))]
        public string UserName { get; set; }
        [Display(Name="FullName",ResourceType=typeof(Resources.Layout))]
        public string FullName { get; set; }
    }
}