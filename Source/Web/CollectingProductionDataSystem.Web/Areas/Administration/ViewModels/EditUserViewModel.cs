namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Web.ViewModels.Identity;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Resources = App_GlobalResources.Resources;

    public class EditUserViewModel:IMapFrom<ApplicationUser>
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "UserName",ResourceType=typeof(Resources.Layout))]
        public string UserName { get; set; }

        [Display(Name = "Email",ResourceType=typeof(Resources.Layout))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50,ErrorMessageResourceName="LengthError",ErrorMessageResourceType=typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Layout))]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50,ErrorMessageResourceName="LengthError",ErrorMessageResourceType=typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "MiddleName", ResourceType = typeof(Resources.Layout))]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50,ErrorMessageResourceName="LengthError",ErrorMessageResourceType=typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "LastName", ResourceType = typeof(Resources.Layout))]
        public string LastName { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Layout))]
        public string FullName { get; private set; }

        [Required]
        [StringLength(250, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "Occupation", ResourceType = typeof(Resources.Layout))]
        public string Occupation { get; set; }

        [Display(Name = "Roles", ResourceType = typeof(Resources.Layout))]
        public IEnumerable<AsignRoleViewModel> UserRoles { get; set; }

        public IEnumerable<ProcessUnitViewModel> ProcessUnits { get; set; }

        public IEnumerable<ParkViewModel> Parks { get; set; }

        [Display(Name = "NewPassword",ResourceType=typeof(Resources.Layout))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

    }
}