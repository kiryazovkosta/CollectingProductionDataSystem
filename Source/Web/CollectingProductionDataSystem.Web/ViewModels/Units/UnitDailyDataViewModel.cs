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
        private static Random rnd = new Random(); 
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Display(Name = "AutomaticValue", ResourceType = typeof(Resources.Layout))]
        public decimal? Value { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        public int UnitsDailyConfigId { get; set; }

        public UnitsDailyConfigDataViewModel UnitsDailyConfig { get; set; }

        public UnitManualDailyDataViewModel UnitsManualDailyData { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitsDailyData, UnitDailyDataViewModel>()
                .ForMember(p => p.UnitsManualDailyData, opt => opt.MapFrom(p => p.UnitsManualDailyData ?? new UnitsManualDailyData() { Value = p.Value }));
        }

        public bool HasManualData { get { return rnd.Next(10) > 6; } set { } }
    }

    public class UnitsDailyConfigDataViewModel : IMapFrom<UnitsDailyConfig>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "UnitName", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [UIHint("Hidden")]
        public int ProductTypeId { get; set; }

        public ProductTypeUnitsDailyDataViewModel ProductType { get; set; }

        public int ProcessUnitId { get; set; }

        public ProcessUnitUnitsDailyDataViewModel ProcessUnit { get; set; }

        public int MeasureUnitId { get; set; }
        public MeasureUnitUnitsDailyDataViewModel MeasureUnit { get; set; }
    }

    public class ProductTypeUnitsDailyDataViewModel : IMapFrom<ProductType>
    {
        [Required]
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

    public class ProcessUnitUnitsDailyDataViewModel : IMapFrom<ProcessUnit>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProcessUnitName", ResourceType = typeof(Resources.Layout))]
        public string ShortName { get; set; } 

    }

    public class MeasureUnitUnitsDailyDataViewModel : IMapFrom<MeasureUnit>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }
    }

    public class UnitManualDailyDataViewModel : IMapFrom<UnitsManualDailyData>, IHaveCustomMappings
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
            configuration.CreateMap<UnitsManualDailyData, UnitManualDailyDataViewModel>()
                .ForMember(p => p.EditReason, opt => opt.MapFrom(p => p.EditReason ?? new EditReason() { Id = 0, Name = Resources.Layout.AutomaticData }));
        }
    }
}