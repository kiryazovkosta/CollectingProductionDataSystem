namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Identity;

    public class RoleViewModel:IMapFrom<ApplicationRole>
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }


        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}