namespace CollectingProductionDataSystem.Web.ViewModels.Units
{
    using System;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.InputModels;
    using Resources = App_GlobalResources.Resources;

    public class UnitDataViewModel : IMapFrom<UnitsData>, IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProcessUnitName { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string MeasureUnit { get; set; }
        public string CollectingDataMechanism { get; set; }
        public decimal? AutomaticValue { get; set; }
        public decimal ManualValue { get; set; }
        public EditReasonInputModel EditReason { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitsData, UnitDataViewModel>()
                .ForMember(p => p.Code, opt => opt.MapFrom(p => p.Unit.Code))
                .ForMember(p => p.ProcessUnitName, opt => opt.MapFrom(p => p.Unit.ProcessUnit.ShortName))
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.Unit.Name))
                .ForMember(p => p.Position, opt => opt.MapFrom(p => p.Unit.Position))
                .ForMember(p => p.MeasureUnit, opt => opt.MapFrom(p => p.Unit.MeasureUnit.Code))
                .ForMember(p => p.CollectingDataMechanism, opt => opt.MapFrom(p => p.Unit.CollectingDataMechanism))
                .ForMember(p => p.AutomaticValue, opt => opt.MapFrom(p => p.Value))
                .ForMember(p => p.ManualValue, opt => opt.MapFrom(p => p.UnitsManualData== null ? p.Value : p.UnitsManualData.Value))
                .ForMember(p => p.EditReason, opt => opt.MapFrom(p => p.UnitsManualData == null ? new EditReason(){Id = 0, Name = Resources.Layout.AutomaticData} : p.UnitsManualData.EditReason));

        }
    }
}