using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Identity;
using Newtonsoft.Json;

namespace CollectingProductionDataSystem.Web.ViewModels.Identity
{
    public class AsignRoleViewModel:IMapFrom<ApplicationRole>
    {
        [Required]
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }


        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Display]
        public bool IsUserInRole { get; set; }
    }
}