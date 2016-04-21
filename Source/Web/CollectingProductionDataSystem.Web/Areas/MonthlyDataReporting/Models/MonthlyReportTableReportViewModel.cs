namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using Resources = App_GlobalResources.Resources;

    public class MonthlyReportTableReportViewModel: IMapFrom<UnitMonthlyData>, IHaveCustomMappings, IValuable
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        //[Display(Name = "MeasuredValue", ResourceType = typeof(Resources.Layout))]
        public decimal? Value { get; set; }

        //[Display(Name = "MeasuredValue", ResourceType = typeof(Resources.Layout))]
        public double RealValue { get; set; }

        public double? RecalculationPercentage { get; set; }

        public double? TotalValue { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        public int UnitMonthlyConfigId { get; set; }

        public UnitsMonthlyConfigDataViewModel UnitMonthlyConfig { get; set; }

        public UnitManualMonthlyDataViewModel UnitManualMonthlyData { get; set; }

        [UIHint("Hidden")]
        public bool IsEditable { get; set; }

        [Display(Name = "RelativeDiff", ResourceType = typeof(Resources.Layout))]
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

        [UIHint("Hidden")]
        public bool IsTotalPosition { get; set; }

        [UIHint("Hidden")]
        public bool IsTotalInputPosition { get; set; }

        [UIHint("Hidden")]
        public bool IsExternalOutputPosition { get; set; }

        [UIHint("Hidden")]
        public bool IsTotalExternalOutputPosition { get; set; }

        [UIHint("Hidden")]
        public bool IsTotalInternalPosition { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitMonthlyData, MonthlyReportTableReportViewModel>()
                         .ForMember(p => p.UnitManualMonthlyData, opt => opt.MapFrom(p => p.UnitManualMonthlyData ?? new UnitManualMonthlyData() { Value = p.Value }))
                         .ForMember(p => p.IsEditable, opt => opt.MapFrom(p => p.UnitMonthlyConfig.IsEditable))
                         .ForMember(p => p.IsTotalPosition, opt => opt.MapFrom(p => p.UnitMonthlyConfig.IsTotalPosition))
                         .ForMember(p => p.IsTotalInputPosition, opt => opt.MapFrom(p => p.UnitMonthlyConfig.IsTotalInputPosition))
                         .ForMember(p => p.IsTotalInternalPosition, opt => opt.MapFrom(p => p.UnitMonthlyConfig.IsTotalInternalPosition))
                         .ForMember(p => p.IsTotalExternalOutputPosition, opt => opt.MapFrom(p => p.UnitMonthlyConfig.IsTotalExternalOutputPosition))
                         .ForMember(p => p.IsExternalOutputPosition, opt => opt.MapFrom(p => p.UnitMonthlyConfig.IsExternalOutputPosition));
        }

        public bool HasManualData { get; set; }
    }
}