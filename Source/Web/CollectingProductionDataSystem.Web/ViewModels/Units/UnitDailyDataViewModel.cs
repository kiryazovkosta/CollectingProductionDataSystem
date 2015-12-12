namespace CollectingProductionDataSystem.Web.ViewModels.Units
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
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

        [Display(Name = "AutomaticValue", ResourceType = typeof(Resources.Layout))]
        public decimal? Value { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        [Display(Name = "TotalMonthQuantity", ResourceType = typeof(Resources.Layout))]
        public decimal TotalMonthQuantity { get; set; }

        public int UnitsDailyConfigId { get; set; }

        public UnitsDailyConfigDataViewModel UnitsDailyConfig { get; set; }

        public UnitManualDailyDataViewModel UnitsManualDailyData { get; set; }

        [UIHint("Hidden")]
        public bool IsEditable { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitsDailyData, UnitDailyDataViewModel>()
                .ForMember(p => p.UnitsManualDailyData, opt => opt.MapFrom(p => p.UnitsManualDailyData ?? new UnitsManualDailyData() { Value = p.Value }))
                .ForMember(p=>p.IsEditable, opt=>opt.MapFrom(p=>p.UnitsDailyConfig.IsEditable))
                .ForMember(p=>p.TotalMonthQuantity, opt=>opt.MapFrom(p=>p.TotalMonthQuantity + (decimal)p.RealValue));
            configuration.CreateMap<UnitDailyDataViewModel, UnitsDailyData>()
                .ForMember(p => p.TotalMonthQuantity, opt => opt.Ignore());
        }

        public bool HasManualData { get; set; }
    }

    public class UnitsDailyConfigDataViewModel : IMapFrom<UnitDailyConfig>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "UnitName", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        //[UIHint("Hidden")]
        //public int ProductId { get; set; }
        //public DailyProductViewModel Product { get; set; }

        public int ProcessUnitId { get; set; }
        public ProcessUnitUnitsDailyDataViewModel ProcessUnit { get; set; }

        public int MeasureUnitId { get; set; }
        public MeasureUnitUnitsDailyDataViewModel MeasureUnit { get; set; }

        [UIHint("Hidden")]
        public int  DailyProductTypeId { get; set; }
        public DailyProductTypeViewModel DailyProductType { get; set; }

        //public void CreateMappings(IConfiguration configuration)
        //{
        //    configuration.CreateMap<UnitsDailyConfig, UnitsDailyConfigDataViewModel>()
        //        .ForMember(p => p.Product, opt => opt.MapFrom(p => p.Product ?? new Product() { Id = 0 }));
        //}
    }
 
    //public class DailyProductViewModel:IMapFrom<Product>,IHaveCustomMappings
    //{
    //    public int Id { get; set; }
    //    public int Code { get; set; }

    //    public string Name { get; set; }

    //    public DailyProductTypeViewModel ProductType { get; set; }

    //    public void CreateMappings(IConfiguration configuration)
    //    {
    //        configuration.CreateMap<Product, DailyProductViewModel>()
    //            .ForMember(p => p.ProductType, opt => opt.MapFrom(p => p.DailyProductType ?? new DailyProductType() { Id=0 }));
    //    }
    //}

    public class DailyProductTypeViewModel : IMapFrom<DailyProductType>
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

        public string SortableShortName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(this.Id.ToString("d2"));
                sb.Append(" ");
                sb.Append(this.ShortName);
                return sb.ToString();
            }
        }

        public FactoryViewModel Factory { get; set; }

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