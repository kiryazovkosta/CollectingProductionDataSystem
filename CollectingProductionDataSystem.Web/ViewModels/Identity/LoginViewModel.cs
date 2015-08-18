namespace WeightScale.WorkstationsChecker.Web.Models.Identity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Required]
        [Display(Name="Потребителско име")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Парола")]
        public string Password { get; set; }
    }
}