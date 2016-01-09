namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;
    using Resources = App_GlobalResources.Resources;

    public class HighwayPipelinesDataViewModel : IMapFrom<HighwayPipelineData>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public int HighwayPipelineConfigId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public int PipeNumber { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductId", ResourceType = typeof(Resources.Layout))]
        public string HighwayPipelineConfigName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductName", ResourceType = typeof(Resources.Layout))]
        public string ProductName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductCode", ResourceType = typeof(Resources.Layout))]
        public int ProductCode { get; set; }

        [Display(Name = "Volume", ResourceType = typeof(Resources.Layout))]
        public decimal Volume { get; set; }

        [Display(Name = "Mass", ResourceType = typeof(Resources.Layout))]
        public decimal Mass { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<HighwayPipelineData, HighwayPipelinesDataViewModel>()
                .ForMember(p => p.HighwayPipelineConfigName, opt => opt.MapFrom(p => p.HighwayPipelineConfig.Name))
                .ForMember(p => p.PipeNumber, opt => opt.MapFrom(p => p.HighwayPipelineConfig.PipeNumber));
        }
    }
}