using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.ViewModels.Identity
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(100,ErrorMessageResourceName="UserName",ErrorMessageResourceType=typeof(App_GlobalResources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "UserName", ResourceType = typeof(App_GlobalResources.Layout))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "Password", ResourceType = typeof(App_GlobalResources.Layout))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(App_GlobalResources.Layout))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceName = "UserName", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "UserName", ResourceType = typeof(App_GlobalResources.Layout))]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage=null,ErrorMessageResourceName="Email",ErrorMessageResourceType=typeof(App_GlobalResources.ErrorMessages))]
        [Display(Name = "Email", ResourceType = typeof(App_GlobalResources.Layout))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "Password", ResourceType = typeof(App_GlobalResources.Layout))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(App_GlobalResources.Layout))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "ConfirmPassword", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceName = "UserName", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "UserName", ResourceType = typeof(App_GlobalResources.Layout))]
        [HiddenInput(DisplayValue=false)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "Password", ResourceType = typeof(App_GlobalResources.Layout))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(App_GlobalResources.Layout))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "ConfirmPassword", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "Password", ResourceType = typeof(App_GlobalResources.Layout))]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "NewPassword", ResourceType = typeof(App_GlobalResources.Layout))]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(App_GlobalResources.Layout))]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "ConfirmPassword", ErrorMessageResourceType = typeof(App_GlobalResources.ErrorMessages))]
        public string ConfirmPassword { get; set; }
    }
}
