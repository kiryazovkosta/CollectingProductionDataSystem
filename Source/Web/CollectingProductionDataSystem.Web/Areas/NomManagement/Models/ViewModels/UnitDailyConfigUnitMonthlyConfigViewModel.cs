namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class UnitDailyConfigUnitMonthlyConfigViewModel : IMapFrom<UnitDailyConfigUnitMonthlyConfig>, IMapFrom<UnitDailyConfig>, IHaveCustomMappings
    {
        public int UnitDailyConfigId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitDailyConfigUnitMonthlyConfig, UnitDailyConfigUnitMonthlyConfigViewModel>()
                         .ForMember(p => p.Code, opt => opt.MapFrom(p => p.UnitDailyConfig.Code))
                         .ForMember(p => p.Name, opt => opt.MapFrom(p => p.UnitDailyConfig.Name));

            configuration.CreateMap<UnitDailyConfig, UnitDailyConfigUnitMonthlyConfigViewModel>()
                         .ForMember(p => p.UnitDailyConfigId, opt => opt.MapFrom(p => p.Id))
                         .ForMember(p => p.Position, opt => opt.Ignore());
        }
    }
}