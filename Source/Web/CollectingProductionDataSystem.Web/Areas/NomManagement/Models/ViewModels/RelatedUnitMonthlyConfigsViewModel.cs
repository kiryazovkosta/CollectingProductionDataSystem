using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Models.Productions.Mounthly;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class RelatedUnitMonthlyConfigsViewModel : IMapFrom<RelatedUnitMonthlyConfigs>, IMapFrom<UnitMonthlyConfig>, IHaveCustomMappings, IEntity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<RelatedUnitMonthlyConfigs, RelatedUnitMonthlyConfigsViewModel>()
                         .ForMember(p => p.Id, opt => opt.MapFrom(p => p.RelatedUnitMonthlyConfigId))
                         .ForMember(p => p.Code, opt => opt.MapFrom(p => p.RelatedUnitMonthlyConfig.Code))
                         .ForMember(p => p.Name, opt => opt.MapFrom(p => p.RelatedUnitMonthlyConfig.Name));

            configuration.CreateMap<UnitMonthlyConfig, RelatedUnitMonthlyConfigsViewModel>()
                         .ForMember(p => p.Id, opt => opt.MapFrom(p => p.Id))
                         .ForMember(p => p.Position, opt => opt.Ignore());
        }
    }
}