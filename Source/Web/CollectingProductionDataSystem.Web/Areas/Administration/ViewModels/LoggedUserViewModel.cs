namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Identity;
    using Resources = App_GlobalResources.Resources;
    public class LoggedUserViewModel:IMapFrom<ApplicationUser>,IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Layout))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Layout))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Layout))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "MiddleName", ResourceType = typeof(Resources.Layout))]
        public string MiddleName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "LastName", ResourceType = typeof(Resources.Layout))]
        public string LastName { get; set; }

        [Display(Name = "FullUserName", ResourceType = typeof(Resources.Layout))]
        public string FullName
        {
            get
            {
                StringBuilder userName = new StringBuilder(150);
                if (!string.IsNullOrEmpty((this.FirstName ?? string.Empty).Trim()))
                {
                    userName.Append(this.FirstName);
                    userName.Append(" ");
                }
                if (!string.IsNullOrEmpty((this.MiddleName ?? string.Empty).Trim()))
                {
                    userName.Append(this.MiddleName);
                    userName.Append(" ");
                }
                if (!string.IsNullOrEmpty((this.LastName ?? string.Empty).Trim()))
                {
                    userName.Append(this.LastName);
                }
                var resultName = userName.ToString().TrimEnd();

                if (string.IsNullOrEmpty(resultName))
                {
                    return this.UserName;
                }

                return resultName;
            }
            private set { }
        }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "LastLogedIn", ResourceType = typeof(Resources.Layout))]
        public DateTime? LastLogedIn { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [StringLength(250, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "Occupation", ResourceType = typeof(Resources.Layout))]
        public string Occupation { get; set; }

        [Display(Name = "IsUserLoggedIn", ResourceType = typeof(Resources.Layout))]
        public int IsUserLoggedIn { get; set; }

        [Display(Name = "UserChangedPassword", ResourceType = typeof(Resources.Layout))]
        [DataType(DataType.Password)]
        public bool UserChangedPassword { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, LoggedUserViewModel>()
                .ForMember(p => p.UserChangedPassword, opt => opt.MapFrom(p => !p.IsChangePasswordRequired));
        }
    }
}