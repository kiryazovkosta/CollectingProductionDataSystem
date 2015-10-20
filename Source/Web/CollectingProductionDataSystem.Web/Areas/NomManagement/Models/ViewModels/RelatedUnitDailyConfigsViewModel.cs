using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class RelatedUnitDailyConfigsViewModel : IMapFrom<RelatedUnitDailyConfigs>, IMapFrom<UnitDailyConfig>, IHaveCustomMappings, IEntity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }



        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<RelatedUnitDailyConfigs, RelatedUnitDailyConfigsViewModel>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => p.RelatedUnitsDailyConfigId))
                .ForMember(p => p.Code, opt => opt.MapFrom(p => p.RelatedUnitsDailyConfig.Code))
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.RelatedUnitsDailyConfig.Name));

            configuration.CreateMap<UnitDailyConfig, RelatedUnitDailyConfigsViewModel>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(p => p.Position, opt => opt.Ignore());
        }
    }
}