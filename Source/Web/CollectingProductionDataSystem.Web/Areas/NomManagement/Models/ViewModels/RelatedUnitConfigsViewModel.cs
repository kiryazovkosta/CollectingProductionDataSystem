using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class RelatedUnitConfigsViewModel:IMapFrom<RelatedUnitConfigs>,IMapFrom<UnitConfig>, IHaveCustomMappings,IEntity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }



        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<RelatedUnitConfigs, RelatedUnitConfigsViewModel>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => p.RelatedUnitConfigId))
                .ForMember(p => p.Code, opt => opt.MapFrom(p => p.RelatedUnitConfig.Code))
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.RelatedUnitConfig.Name));

            configuration.CreateMap<UnitConfig, RelatedUnitConfigsViewModel>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(p => p.Position, opt => opt.Ignore())
                .ForSourceMember(p => p.IsMemberOfShiftsReport, opt => opt.Ignore());
        }
    }
}