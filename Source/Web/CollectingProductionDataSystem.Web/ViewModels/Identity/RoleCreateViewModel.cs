namespace CollectingProductionDataSystem.Web.ViewModels.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Resources = App_GlobalResources.Resources;
    public class RoleCreateViewModel
    {
        [Required]
        [Display(Name="RoleName",ResourceType=typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name="Description",ResourceType=typeof(Resources.Layout))]
        public string Description { get; set; }

    }
}