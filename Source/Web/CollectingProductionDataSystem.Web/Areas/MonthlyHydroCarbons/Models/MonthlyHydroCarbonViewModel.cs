namespace CollectingProductionDataSystem.Web.Areas.MonthlyHydroCarbons.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Resources = App_GlobalResources.Resources;

    public class MonthlyHydroCarbonViewModel : IMapFrom<UnitMonthlyData> , IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Display(Name = "MeasuredValue", ResourceType = typeof(Resources.Layout))]
        public decimal? Value { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        public int UnitsDailyConfigId { get; set; }

        public UnitsMonthlyConfigDataViewModel UnitMonthlyConfig { get; set; }

        public UnitManualMonthlyDataViewModel UnitManualMonthlyData { get; set; }

        [UIHint("Hidden")]
        public bool IsEditable { get; set; }

        [Display(Name = "RelativeDifference", ResourceType = typeof(Resources.Layout))]
        public decimal RelativeDifference
        {
            get
            {
                var measured = this.Value ?? 0;
                decimal inventory = 0;

                if (this.UnitManualMonthlyData != null)
                {
                    inventory = this.UnitManualMonthlyData.Value;
                }

                decimal result = 0M;
                if (measured != 0)
                {
                    result = ((measured - inventory) / measured) * 100;
                }
                return result;
            }
        }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitMonthlyData, MonthlyHydroCarbonViewModel>()
                .ForMember(p => p.UnitManualMonthlyData, opt => opt.MapFrom(p => p.UnitManualMonthlyData ?? new UnitManualMonthlyData() { Value = p.Value }))
                .ForMember(p => p.IsEditable, opt => opt.MapFrom(p => p.UnitMonthlyConfig.IsEditable));
            //.ForMember(p => p.TotalMonthQuantity, opt => opt.MapFrom(p => p.UnitsDailyConfig.NotATotalizedPosition ? (decimal)p.RealValue : (p.TotalMonthQuantity + (decimal)p.RealValue)));
            //.ForMember(p=>p.TotalMonthQuantity, opt=>opt.MapFrom(p=>p.TotalMonthQuantity + (decimal)p.RealValue));
            //configuration.CreateMap<MonthlyHydroCarbonViewModel, UnitMonthlyData>()
            //    .ForMember(p => p.TotalMonthQuantity, opt => opt.Ignore());
        }

        public bool HasManualData { get; set; }
    }

    public partial class UnitsMonthlyConfigDataViewModel : IMapFrom<UnitMonthlyConfig>
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
        public ProcessUnitUnitsMonthlyDataViewModel ProcessUnit { get; set; }

        public int MeasureUnitId { get; set; }
        public MeasureUnitUnitMonthlyDataViewModel MeasureUnit { get; set; }

        [UIHint("Hidden")]
        public int ProductTypeId { get; set; }
        public MonthlyProductTypeViewModel ProductType { get; set; }


    }

    public partial class MonthlyProductTypeViewModel : IMapFrom<DailyProductType>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductType", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }


        public string SortableName
        {
            get
            {
                return this.Id.ToString().PadLeft(2, '0') + " " + this.Name;
            }
        }

    }

    public partial class ProcessUnitUnitsMonthlyDataViewModel : IMapFrom<ProcessUnit>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProcessUnitName", ResourceType = typeof(Resources.Layout))]
        public string ShortName { get; set; }

        public string SortableName
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

    public partial class MeasureUnitUnitMonthlyDataViewModel : IMapFrom<MeasureUnit>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }
    }

    public partial class UnitManualMonthlyDataViewModel : IMapFrom<UnitManualMonthlyData> //, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ManualValue", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "ManualValue", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public decimal Value { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        //public void CreateMappings(IConfiguration configuration)
        //{
        //    configuration.CreateMap<UnitManualMonthlyData, UnitManualMonthlyDataViewModel>()
        //        .ForMember(p => p.Value, opt => opt.MapFrom(p => p.Value)).AfterMap((p, v) => { if (v == null) { v = new UnitManualMonthlyDataViewModel() { Value = 0 }; } });
        //}
    }
}