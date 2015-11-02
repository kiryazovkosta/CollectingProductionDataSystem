using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Transactions;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class RelatedMeasuringPointConfigsViewModel : IMapFrom<RelatedMeasuringPointConfigs>, IMapFrom<MeasuringPointConfig>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ControlPoint { get; set; }

        public string MeasuringPointName { get; set; }

        public int Position { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<RelatedMeasuringPointConfigs, RelatedMeasuringPointConfigsViewModel>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => p.RelatedMeasuringPointConfigId))
                .ForMember(p => p.ControlPoint, opt => opt.MapFrom(p => p.RelatedMeasuringPointConfig.ControlPoint))
                .ForMember(p => p.MeasuringPointName, opt => opt.MapFrom(p => p.RelatedMeasuringPointConfig.MeasuringPointName));

            configuration.CreateMap<MeasuringPointConfig, RelatedMeasuringPointConfigsViewModel>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(p => p.Position, opt => opt.Ignore());
        }
    }
}