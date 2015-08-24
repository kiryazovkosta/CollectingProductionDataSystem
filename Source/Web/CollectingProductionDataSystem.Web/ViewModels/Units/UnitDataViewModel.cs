namespace CollectingProductionDataSystem.Web.ViewModels.Units
{
    using System;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitDataViewModel :IMapFrom<UnitsData>, IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProcessUnitName { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string MeasureUnit { get; set; }
        public string CollectingDataMechanism { get; set; }
        public decimal? Value { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitsData, UnitDataViewModel>()
                .ForMember(p => p.Code, opt => opt.MapFrom(p => p.Unit.Code));
            configuration.CreateMap<UnitsData, UnitDataViewModel>()
                .ForMember(p => p.ProcessUnitName, opt => opt.MapFrom(p => p.Unit.ProcessUnit.ShortName));
            configuration.CreateMap<UnitsData, UnitDataViewModel>()
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.Unit.Name));
            configuration.CreateMap<UnitsData, UnitDataViewModel>()
                .ForMember(p => p.Position, opt => opt.MapFrom(p => p.Unit.Position));
            configuration.CreateMap<UnitsData, UnitDataViewModel>()
                .ForMember(p => p.MeasureUnit, opt => opt.MapFrom(p => p.Unit.MeasureUnit.Code));
            configuration.CreateMap<UnitsData, UnitDataViewModel>()
                .ForMember(p => p.CollectingDataMechanism, opt => opt.MapFrom(p => p.Unit.CollectingDataMechanism));
        }
    }
}