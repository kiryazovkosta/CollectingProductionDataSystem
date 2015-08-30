namespace CollectingProductionDataSystem.Web.ViewModels.Units
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.InputModels;
    using Resources = App_GlobalResources.Resources;

    public class UnitDailyDataViewModel : IMapFrom<UnitsDailyData>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductType", ResourceType = typeof(Resources.Layout))]
        public string ProductType { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProcessUnitName", ResourceType = typeof(Resources.Layout))]
        public string ProcessUnitName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "UnitName", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public string MeasureUnit { get; set; }

        [Display(Name = "AutomaticValue", ResourceType = typeof(Resources.Layout))]
        public decimal AutomaticValue { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ManualValue", ResourceType = typeof(Resources.Layout))]
        [Range(0.01,double.MaxValue,ErrorMessageResourceName="ManualValue",ErrorMessageResourceType=typeof(Resources.ErrorMessages))]
        public decimal ManualValue { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "EditReason", ResourceType = typeof(Resources.Layout))]
        public EditReasonInputModel EditReason { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitsDailyData, UnitDailyDataViewModel>()
                .ForMember(p => p.Code, opt => opt.MapFrom(p => p.UnitsDailyConfig.Code))
                .ForMember(p => p.ProductType, opt => opt.MapFrom(p => p.UnitsDailyConfig.ProductType.Name))
                .ForMember(p => p.ProcessUnitName, opt => opt.MapFrom(p => p.UnitsDailyConfig.ProcessUnit.ShortName))
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.UnitsDailyConfig.Name))
                .ForMember(p => p.MeasureUnit, opt => opt.MapFrom(p => p.UnitsDailyConfig.MeasureUnit.Code))
                .ForMember(p => p.AutomaticValue, opt => opt.MapFrom(p => p.Value))
                .ForMember(p => p.ManualValue, opt => opt.MapFrom(p => p.UnitsManualDailyData == null ? p.Value : p.UnitsManualDailyData.Value))
                .ForMember(p => p.EditReason, opt => opt.MapFrom(p => p.UnitsManualDailyData == null ? new EditReason(){Id = 0, Name = Resources.Layout.AutomaticData} : p.UnitsManualDailyData.EditReason));

        }
    }
}