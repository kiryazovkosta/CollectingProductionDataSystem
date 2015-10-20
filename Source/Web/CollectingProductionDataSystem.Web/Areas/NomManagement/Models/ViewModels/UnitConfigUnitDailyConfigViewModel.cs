using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class UnitConfigUnitDailyConfigViewModel : IMapFrom<UnitConfigUnitDailyConfig>,IMapFrom<UnitConfig>, IHaveCustomMappings
    {
        public int UnitConfigId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitConfigUnitDailyConfig, UnitConfigUnitDailyConfigViewModel>()
                .ForMember(p => p.Code, opt => opt.MapFrom(p => p.UnitConfig.Code))
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.UnitConfig.Name));

            configuration.CreateMap<UnitConfig, UnitConfigUnitDailyConfigViewModel>()
               .ForMember(p => p.UnitConfigId, opt => opt.MapFrom(p => p.Id));
        }
    }
}