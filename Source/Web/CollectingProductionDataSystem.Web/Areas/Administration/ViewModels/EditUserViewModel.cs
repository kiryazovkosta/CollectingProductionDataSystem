namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Web.ViewModels.Identity;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Resources = App_GlobalResources.Resources;

    public class EditUserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public EditUserViewModel()
        {
            this.UserRoles = new List<RoleViewModel>();
            this.ProcessUnits = new List<ProcessUnitViewModel>();
            this.Parks = new List<ParkViewModel>();
        }
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

        [Display(Name = "FullName", ResourceType = typeof(Resources.Layout))]
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
        [StringLength(250, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(Resources.ErrorMessages), MinimumLength = 4)]
        [Display(Name = "Occupation", ResourceType = typeof(Resources.Layout))]
        public string Occupation { get; set; }

        [Display(Name = "Roles", ResourceType = typeof(Resources.Layout))]
        public IEnumerable<RoleViewModel> UserRoles { get; set; }

        [Display(Name = "ProcessUnits", ResourceType = typeof(Resources.Layout))]
        public IEnumerable<ProcessUnitViewModel> ProcessUnits { get; set; }

        [Display(Name = "Parks", ResourceType = typeof(Resources.Layout))]
        public IEnumerable<ParkViewModel> Parks { get; set; }

        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Layout))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

         /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<EditUserViewModel, ApplicationUser>()
                .ForMember(p => p.UserRoles, opt => opt.Ignore())
                .ForMember(p => p.Parks, opt => opt.Ignore())
                .ForMember(p => p.ProcessUnits, opt => opt.Ignore());
        }

    }
}