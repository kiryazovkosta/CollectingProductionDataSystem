namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.ViewModels
{
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Resources = App_GlobalResources.Resources;

    public class TankWeighInVacuumViewModel: IMapFrom<WeightInVacuumDto>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductId", ResourceType = typeof(Resources.Layout))]
        public string ProductId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductName", ResourceType = typeof(Resources.Layout))]
        public string ProductName { get; set; }

        [Display(Name = "WeightInVacuum", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "WeightInVacuum", ErrorMessageResourceType = typeof(Resources.Layout))]
        public decimal? WeightInVacuum { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<WeightInVacuumDto, TankWeighInVacuumViewModel>()
                .ForMember(p => p.RecordTimestamp, opt => opt.MapFrom(p => p.RecordTimestamp))
                .ForMember(p => p.ProductId, opt => opt.MapFrom(p => p.Product.Code))
                .ForMember(p => p.ProductName, opt => opt.MapFrom(p => p.Product.Name))
                .ForMember(p => p.WeightInVacuum, opt => opt.MapFrom(p => p.WeightInVaccum));

        }

    }
}