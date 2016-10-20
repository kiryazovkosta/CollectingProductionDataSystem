namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels
{
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions;
    using System.ComponentModel.DataAnnotations;
    using Resources = App_GlobalResources.Resources;

    public class UnitsReportsDataViewModel : IMapFrom<MultiShift>, IHaveCustomMappings
    {
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        //[Display(Name = "№")]
        //public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime TimeStamp { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Factory", ResourceType = typeof(Resources.Layout))]
        public string Factory { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public string ProcessUnit { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Position", ResourceType = typeof(Resources.Layout))]
        public string Position { get; set; }

        [UIHint("Hidden")]
        public int UnitConfigId { get; set; }

        public bool NotATotalizedPosition { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "UnitName", ResourceType = typeof(Resources.Layout))]
        public string UnitName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public string MeasureUnit { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ShiftProductType", ResourceType = typeof(Resources.Layout))]
        public string ShiftProductType { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Shift1QuantityValue", ResourceType = typeof(Resources.Layout))]
        public decimal Shift1QuantityValue { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Shift2QuantityValue", ResourceType = typeof(Resources.Layout))]
        public decimal Shift2QuantityValue { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Shift3QuantityValue", ResourceType = typeof(Resources.Layout))]
        public decimal Shift3QuantityValue { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "TotalQuantityValue", ResourceType = typeof(Resources.Layout))]
        public decimal TotalQuantityValue
        {
            get
            {
                decimal totalQuantity = 0m;
                if (this.NotATotalizedPosition == true)
                {
                    totalQuantity = this.Shift3QuantityValue;
                }
                else
                {
                    totalQuantity = this.Shift1QuantityValue + this.Shift2QuantityValue + this.Shift3QuantityValue;
                }

                return totalQuantity;
            }
        }

        [Display(Name = "TotalMonthQuantity", ResourceType = typeof(Resources.Layout))]
        public double TotalMonthQuantity { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<MultiShift, UnitsReportsDataViewModel>()
                .ForMember(p => p.Shift1QuantityValue, opt => opt.MapFrom(p => (p.Shift1 != null) ? p.Shift1.RealValue : 0))
                .ForMember(p => p.Shift2QuantityValue, opt => opt.MapFrom(p => (p.Shift2 != null) ? p.Shift2.RealValue : 0))
                .ForMember(p => p.Shift3QuantityValue, opt => opt.MapFrom(p => (p.Shift3 != null) ? p.Shift3.RealValue : 0))
                .ForMember(p => p.TotalQuantityValue, opt => opt.Ignore())
                .ForMember(p => p.TotalMonthQuantity, opt => opt.Ignore());
        }
    }
}