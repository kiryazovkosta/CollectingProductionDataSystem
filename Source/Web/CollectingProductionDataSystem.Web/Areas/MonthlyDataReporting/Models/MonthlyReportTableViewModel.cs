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

    public class MonthlyReportTableViewModel: IMapFrom<UnitMonthlyData>, IHaveCustomMappings, IValuable
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        //[Display(Name = "MeasuredValue", ResourceType = typeof(Resources.Layout))]
        public decimal? Value { get; set; }

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

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UnitMonthlyData, MonthlyReportTableViewModel>()
                         .ForMember(p => p.UnitManualMonthlyData, opt => opt.MapFrom(p => p.UnitManualMonthlyData ?? new UnitManualMonthlyData() { Value = p.Value }))
                         .ForMember(p => p.IsEditable, opt => opt.MapFrom(p => p.UnitMonthlyConfig.IsEditable));
        }

        public bool HasManualData { get; set; }
    }
}