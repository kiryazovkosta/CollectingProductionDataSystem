using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Infrastructure.Mapping;

namespace CollectingProductionDataSystem.Web.ViewModels.Tank
{
    public class ParkViewModel : IMapFrom<Park>,IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ParkViewModel, Park>()
                .ForMember(p => p.Area, opt => opt.Ignore())
                .ForMember(p => p.AreaId, opt => opt.Ignore());
        }
    }
}