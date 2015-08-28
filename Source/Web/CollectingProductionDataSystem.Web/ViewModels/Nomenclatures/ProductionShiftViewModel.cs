using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Nomenclatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.ViewModels.Nomenclatures
{
    public class ProductionShiftViewModel : IMapFrom<ProductionShift>, IHaveCustomMappings
    {
        public  int Id { get; set;}
        public string Name { get; set; }
        public int Minutes { get; set; }
        public int OffsetMinutes { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductionShift, ProductionShiftViewModel>()
                .ForMember(p => p.Minutes, opt => opt.MapFrom(p => p.BeginMinutes));
        }
    }
}