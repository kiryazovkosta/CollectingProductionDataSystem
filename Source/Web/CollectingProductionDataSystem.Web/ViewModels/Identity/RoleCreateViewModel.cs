namespace CollectingProductionDataSystem.Web.ViewModels.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Identity;
    using Resources = App_GlobalResources.Resources;

    public class RoleCreateViewModel : IMapFrom<ApplicationRole>,IHaveCustomMappings
    {
        [Required]
        [Display(Name="RoleName",ResourceType=typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name="Description",ResourceType=typeof(Resources.Layout))]
        public string Description { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<RoleCreateViewModel, ApplicationRole>()
                .ForMember(p => p.IsAvailableForAdministrators, opt => opt.Ignore())
                .ForMember(p => p.Users, opt => opt.Ignore())
                .ForMember(p => p.IsDeleted, opt => opt.Ignore())
                .ForMember(p => p.DeletedOn, opt => opt.Ignore())
                .ForMember(p => p.DeletedFrom, opt => opt.Ignore())
                .ForMember(p => p.CreatedOn, opt => opt.Ignore())
                .ForMember(p => p.PreserveCreatedOn, opt => opt.Ignore())
                .ForMember(p => p.ModifiedOn, opt => opt.Ignore())
                .ForMember(p => p.CreatedFrom, opt => opt.Ignore())
                .ForMember(p => p.ModifiedFrom, opt => opt.Ignore())
                .ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}