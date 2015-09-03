using System.ComponentModel.DataAnnotations;
using Resources = App_GlobalResources.Resources;
namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceName = "UserName", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Layout))]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
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
}