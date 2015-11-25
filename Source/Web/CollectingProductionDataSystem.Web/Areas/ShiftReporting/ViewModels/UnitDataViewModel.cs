namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.ViewModels
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

    public class UnitDataViewModel : IMapFrom<UnitsData>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Display(Name = "AutomaticValue", ResourceType = typeof(Resources.Layout))]
        public decimal? Value { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        public int UnitConfigId { get; set; }

        public UnitConfigDataViewModel UnitConfig { get; set; }

        public UnitsManualDataViewModel UnitsManualData { get; set; }

        [Display(Name = "Shift", ResourceType = typeof(Resources.Layout))]
        public ShiftType Shift { get; set; }

        public bool IsEditable { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitsData, UnitDataViewModel>()
                .ForMember(p => p.UnitsManualData, opt => opt.MapFrom(p => p.UnitsManualData ?? new UnitsManualData() { Value = p.Value ?? 0M }))
                .ForMember(p => p.Shift, opt => opt.MapFrom(p => p.ShiftId))
                .ForMember(p => p.IsEditable, opt => opt.MapFrom(p => p.UnitConfig.IsEditable));
        }
    }

    public class UnitConfigDataViewModel : IMapFrom<UnitConfig>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "UnitName", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Position", ResourceType = typeof(Resources.Layout))]
        public string Position { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "CollectingDataMechanism", ResourceType = typeof(Resources.Layout))]
        public string CollectingDataMechanism { get; set; }

        public bool IsEditable { get; set; }

        [UIHint("Hidden")]
        public bool IsCalculated { get; set; }
        public string CalculatedFormula { get; set; }
        public decimal? MaximumFlow { get; set; }
        public decimal? EstimatedDensity { get; set; }
        public decimal? EstimatedPressure { get; set; }
        public decimal? EstimatedTemperature { get; set; }
        public decimal? EstimatedCompressibilityFactor { get; set; }

        public int ProcessUnitId { get; set; }
        public ProcessUnitUnitDataViewModel ProcessUnit { get; set; }

        public int MeasureUnitId { get; set; }
        public MeasureUnitUnitDataViewModel MeasureUnit { get; set; }

        public int? EnteredMeasureUnitId { get; set; }
        public MeasureUnitUnitDataViewModel EnteredMeasureUnit { get; set; }

        [UIHint("Hidden")]
        public int ShiftProductTypeId { get; set; }
        public ShiftProductTypeViewModel ShiftProductType { get; set; }

    }

    public class ShiftProductTypeViewModel : IMapFrom<ShiftProductType>
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductType", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }


        public string Sort
        {
            get
            {
                return this.Id.ToString().PadLeft(2,'0')+ " " + this.Name;
            }
        }
    }

    //public class ShiftProductViewModel : IMapFrom<Product>
    //{
    //    public int Id { get; set; }
    //    public int Code { get; set; }
    //    public string Name { get; set; }
    //}

    public class ProcessUnitUnitDataViewModel : IMapFrom<ProcessUnit>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public string ShortName { get; set; } 

    }

    public class MeasureUnitUnitDataViewModel : IMapFrom<MeasureUnit>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }
    }

    public class UnitsManualDataViewModel : IMapFrom<UnitsManualData>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ManualValue", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "ManualValue", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public decimal Value { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "EditReason", ResourceType = typeof(Resources.Layout))]
        public EditReasonInputModel EditReason { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitsManualData, UnitsManualDataViewModel>()
                .ForMember(p => p.EditReason, opt => opt.MapFrom(p => p.EditReason ?? new EditReason() { Id = 0, Name = Resources.Layout.AutomaticData }));
        }
    }
}