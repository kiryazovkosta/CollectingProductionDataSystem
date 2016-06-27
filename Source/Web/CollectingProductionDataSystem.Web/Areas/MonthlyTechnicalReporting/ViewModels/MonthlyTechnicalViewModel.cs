namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.ViewModels
{
    using AutoMapper;
    using CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Resources = App_GlobalResources.Resources;

    public class MonthlyTechnicalViewModel : IMapFrom<MonthlyTechnicalReportDataDto>, IHaveCustomMappings
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Factory", ResourceType = typeof(Resources.Layout))]
        public string Factory { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public string ProcessUnit { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MessageType", ResourceType = typeof(Resources.Layout))]
        public string MaterialType { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MeasureUnit", ResourceType = typeof(Resources.Layout))]
        public string MeasurementUnit { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MessageType", ResourceType = typeof(Resources.Layout))]
        public string DetailedMaterialType { get; set; }



        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MonthPlanValue", ResourceType = typeof(Resources.Layout))]
        public decimal MonthPlanValue { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MonthPlanPercentage", ResourceType = typeof(Resources.Layout))]
        public decimal MonthPlanPercentage { get; set; }



        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MonthFactValue", ResourceType = typeof(Resources.Layout))]
        public decimal MonthFactValue { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MonthFactPercentage", ResourceType = typeof(Resources.Layout))]
        public decimal MonthFactPercentage { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MonthFactValueDifference", ResourceType = typeof(Resources.Layout))]
        public decimal MonthFactValueDifference { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "MonthFactPercentageDifference", ResourceType = typeof(Resources.Layout))]
        public decimal MonthFactPercentageDifference { get; set; }



        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "YearFactValue", ResourceType = typeof(Resources.Layout))]
        public decimal YearFactValue { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "YearFactPercentage", ResourceType = typeof(Resources.Layout))]
        public decimal YearFactPercentage { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "YearFactValueDifference", ResourceType = typeof(Resources.Layout))]
        public decimal YearFactValueDifference { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "YearFactPercentageDifference", ResourceType = typeof(Resources.Layout))]
        public decimal YearFactPercentageDifference { get; set; }



        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<MonthlyTechnicalReportDataDto, MonthlyTechnicalViewModel>()
                .ForMember(p => p.MonthPlanValue, opt => opt.MapFrom(p => p.PlanValue))
                .ForMember(p => p.MonthPlanPercentage, opt => opt.MapFrom(p => p.PlanPercentage))

                .ForMember(p => p.MonthFactValue, opt => opt.MapFrom(p => p.FactValue))
                .ForMember(p => p.MonthFactPercentage, opt => opt.MapFrom(p => p.FactPercentage))
                .ForMember(p => p.MonthFactValueDifference, opt => opt.MapFrom(p => p.FactValueDifference))
                .ForMember(p => p.MonthFactPercentageDifference, opt => opt.MapFrom(p => p.FactPercentageDifference))

                .ForMember(p => p.YearFactValue, opt => opt.MapFrom(p => p.YearValue))
                .ForMember(p => p.YearFactPercentage, opt => opt.MapFrom(p => p.YearPercentage))
                .ForMember(p => p.YearFactValueDifference, opt => opt.MapFrom(p => p.YearValueDifference))
                .ForMember(p => p.YearFactPercentageDifference, opt => opt.MapFrom(p => p.YearPercentageDifference));
        }
    }
}