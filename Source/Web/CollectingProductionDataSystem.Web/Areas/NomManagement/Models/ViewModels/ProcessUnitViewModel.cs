namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessUnitViewModel : IMapFrom<ProcessUnit>,IHaveCustomMappings
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "ShortName", ResourceType = typeof(Resources.Layout))]
        public string ShortName { get; set; }

        [Required]
        [Display(Name = "FullName", ResourceType = typeof(Resources.Layout))]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Factory", ResourceType = typeof(Resources.Layout))]
        public FactoryViewModel Factory { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProcessUnit, ProcessUnitViewModel>()
                .ForMember(p => p.Factory, opt => opt.MapFrom(p => p.Factory ?? new Factory() { Id = 0, FullName = string.Empty }));

            configuration.CreateMap<ProcessUnitViewModel, ProcessUnit>()
                .ForMember(p => p.FactoryId, opt => opt.MapFrom(p => p.Factory.Id))
                .ForMember(p => p.Factory, opt => opt.Ignore());
        }
    }
}