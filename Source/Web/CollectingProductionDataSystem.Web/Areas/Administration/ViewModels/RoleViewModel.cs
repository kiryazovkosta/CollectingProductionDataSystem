namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Identity;

    public class RoleViewModel:IMapFrom<ApplicationRole>,IHaveCustomMappings
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<RoleViewModel, ApplicationRole>()
                .ForMember(p => p.IsAvailableForAdministrators, opt => opt.Ignore())
                .ForMember(p => p.Users, opt => opt.Ignore())
                .ForMember(p => p.IsDeleted, opt => opt.Ignore())
                .ForMember(p => p.DeletedOn, opt => opt.Ignore())
                .ForMember(p => p.DeletedFrom, opt => opt.Ignore())
                .ForMember(p => p.CreatedOn, opt => opt.Ignore())
                .ForMember(p => p.PreserveCreatedOn, opt => opt.Ignore())
                .ForMember(p => p.ModifiedOn, opt => opt.Ignore())
                .ForMember(p => p.CreatedFrom, opt => opt.Ignore())
                .ForMember(p => p.ModifiedFrom, opt => opt.Ignore());
        }
    }
}