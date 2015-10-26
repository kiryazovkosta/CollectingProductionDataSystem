using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Nomenclatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.ViewModels.Nomenclatures
{
    public class ShiftViewModel : IMapFrom<Shift>, IHaveCustomMappings
    {
        public  int Id { get; set;}
        public string Name { get; set; }
        public long BeginTicks { get; set; }
        public long ReadOffsetTicks { get; set; }
        public long ReadPollTimeSlotTicks { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            //configuration.CreateMap<Shift, ShiftViewModel>()
            //    .ForMember(p => p.Minutes, opt => opt.MapFrom(p => p.BeginMinutes));
        }
    }
}