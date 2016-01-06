using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Nomenclatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class MaterialDetailTypeViewModel : IEntity, IMapFrom<MaterialDetailType>, IHaveCustomMappings
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Display(Name = "Position", ResourceType = typeof(Resources.Layout))]
        public int Position { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Display(Name = "MaterialType", ResourceType = typeof(Resources.Layout))]
        public int MaterialTypeId { get; set; }


        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        ///<param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<MaterialDetailType, MaterialDetailTypeViewModel>()
                .ForMember(p => p.Position, opt => opt.MapFrom(p => p.Position ?? p.Id));
        }


    }
}