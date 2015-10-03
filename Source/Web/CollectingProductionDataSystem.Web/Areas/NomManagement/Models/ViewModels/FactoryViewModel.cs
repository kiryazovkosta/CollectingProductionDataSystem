namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    public class FactoryViewModel : IMapFrom<Factory>, IHaveCustomMappings
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
        [Display(Name = "factory", ResourceType = typeof(Resources.Layout))]
        public PlantViewModel Plant { get; set; }
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Factory, FactoryViewModel>()
                .ForMember(p => p.Plant, opt => opt.MapFrom(p => p.Plant ?? new Plant() { Id = 0, FullName = string.Empty }));
            configuration.CreateMap<FactoryViewModel, Factory>()
                .ForMember(p => p.PlantId, opt => opt.MapFrom(p => p.Plant.Id))
                .ForMember(p => p.Plant, opt => opt.Ignore());
        }
    }
}