using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Productions.Mounthly;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class ProductionPlanConfigUnitMonthlyConfigFactFractionMembersViewModel
        : IMapFrom<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers>, IMapFrom<UnitMonthlyConfig>, IHaveCustomMappings
    {
        public int UnitMonthlyConfigId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }


        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers, ProductionPlanConfigUnitMonthlyConfigFactFractionMembersViewModel>()
                         .ForMember(p => p.Code, opt => opt.MapFrom(p => p.UnitMonthlyConfig.Code))
                         .ForMember(p => p.Name, opt => opt.MapFrom(p => p.UnitMonthlyConfig.Name));

            configuration.CreateMap<UnitMonthlyConfig, ProductionPlanConfigUnitMonthlyConfigFactFractionMembersViewModel>()
                         .ForMember(p => p.UnitMonthlyConfigId, opt => opt.MapFrom(p => p.Id))
                         .ForMember(p => p.Position, opt => opt.Ignore());
        }
    }
}