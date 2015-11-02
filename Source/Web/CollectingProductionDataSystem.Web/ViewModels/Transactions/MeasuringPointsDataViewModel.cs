namespace CollectingProductionDataSystem.Web.ViewModels.Transactions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Transactions;
    using Resources = App_GlobalResources.Resources;

    public class MeasuringPointsDataViewModel : IMapFrom<MeasuringPointsConfigsData>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductId", ResourceType = typeof(Resources.Layout))]
        public int ProductId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductName", ResourceType = typeof(Resources.Layout))]
        public string ProductName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "AvtoQuantity", ResourceType = typeof(Resources.Layout))]
        public decimal AvtoQuantity { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "JpQuantity", ResourceType = typeof(Resources.Layout))]
        public decimal JpQuantity { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "SeaQuantity", ResourceType = typeof(Resources.Layout))]
        public decimal SeaQuantity { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "PipeQuantity", ResourceType = typeof(Resources.Layout))]
        public decimal PipeQuantity { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "TotalQuantity", ResourceType = typeof(Resources.Layout))]
        public decimal TotalQuantity { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ActiveQuantity", ResourceType = typeof(Resources.Layout))]
        public decimal ActiveQuantity { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<MeasuringPointsConfigsData, MeasuringPointsDataViewModel>()
                .ForMember(p => p.ProductName, opt => opt.MapFrom(p => p.ProductName));
        }
    }
}