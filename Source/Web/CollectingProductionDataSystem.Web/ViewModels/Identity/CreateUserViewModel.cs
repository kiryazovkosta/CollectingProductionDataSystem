namespace CollectingProductionDataSystem.Web.ViewModels.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Identity;

    public class CreateUserViewModel : IMapFrom<ApplicationUser>,IHaveCustomMappings
    {
        [Required]
        [Display(Name = "Потребителско име")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Електронна поща")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Парола")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Повторно въвеждане на паролата")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Въведените пароли трябва да съвпадат !!!")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<AsignRoleViewModel> Roles {get; set;}
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<CreateUserViewModel, ApplicationUser>().ForMember(p=>p.Roles, opt => opt.MapFrom(p => new List<UserRoleIntPk>()));
        }
    }
}