namespace CollectingProductionDataSystem.Web.ViewModels.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    public class RoleCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsAvailableForAdministrators { get; set; }
    }
}