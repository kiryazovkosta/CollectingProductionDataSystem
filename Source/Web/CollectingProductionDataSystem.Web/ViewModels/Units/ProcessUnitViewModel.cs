using CollectingProductionDataSystem.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Web.ViewModels.Units
{
    public class ProcessUnitViewModel : IMapFrom<ProcessUnit>, IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string  FullName { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ProcessUnit, ProcessUnitViewModel>()
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.ShortName));
            configuration.CreateMap<ProcessUnitViewModel, ProcessUnit>()
                .ForMember(p => p.ShortName, opt => opt.MapFrom(p => p.Name));
        }
    }
}