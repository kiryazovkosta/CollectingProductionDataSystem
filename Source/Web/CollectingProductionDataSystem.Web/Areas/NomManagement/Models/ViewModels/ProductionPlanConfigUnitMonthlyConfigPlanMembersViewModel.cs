namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class ProductionPlanConfigUnitMonthlyConfigPlanMembersViewModel
        : IMapFrom<ProductionPlanConfigUnitMonthlyConfigPlanMembers>, IMapFrom<UnitMonthlyConfig>, IHaveCustomMappings
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
            configuration.CreateMap<ProductionPlanConfigUnitMonthlyConfigPlanMembers, ProductionPlanConfigUnitMonthlyConfigPlanMembersViewModel>()
                         .ForMember(p => p.Code, opt => opt.MapFrom(p => p.UnitMonthlyConfig.Code))
                         .ForMember(p => p.Name, opt => opt.MapFrom(p => p.UnitMonthlyConfig.Name));

            configuration.CreateMap<UnitMonthlyConfig, ProductionPlanConfigUnitMonthlyConfigPlanMembersViewModel>()
                         .ForMember(p => p.UnitMonthlyConfigId, opt => opt.MapFrom(p => p.Id))
                         .ForMember(p => p.Position, opt => opt.Ignore());
        }
    }
}