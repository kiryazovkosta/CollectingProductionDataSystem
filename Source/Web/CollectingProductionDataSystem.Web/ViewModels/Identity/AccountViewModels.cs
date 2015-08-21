using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.ViewModels.Identity
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(100,ErrorMessageResourceName="UserName",ErrorMessageResourceType=typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Layout))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Layout))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Resources.Layout))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceName = "UserName", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Layout))]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage=null,ErrorMessageResourceName="Email",ErrorMessageResourceType=typeof(Resources.ErrorMessages))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Layout))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Layout))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Layout))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "ConfirmPassword", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceName = "UserName", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Layout))]
        [HiddenInput(DisplayValue=false)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Layout))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Layout))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "ConfirmPassword", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Layout))]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 6)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Layout))]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Layout))]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "ConfirmPassword", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public string ConfirmPassword { get; set; }
    }
}
