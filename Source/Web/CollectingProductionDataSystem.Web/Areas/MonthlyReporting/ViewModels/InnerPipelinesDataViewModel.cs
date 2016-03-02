namespace CollectingProductionDataSystem.Web.Areas.MonthlyReporting.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.MonthlyServices;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using Resources = App_GlobalResources.Resources;

    public class InnerPipelinesDataViewModel : IMapFrom<InnerPipelineData>, IMapFrom<InnerPipelineDto>, IHaveCustomMappings
    {
        [Required]
        [Display(Name = "ProductId", ResourceType = typeof(Resources.Layout))]
        public int Id { get; set; }

        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        [Required]
        [Display(Name = "Product", ResourceType = typeof(Resources.Layout))]
        public int ProductId { get; set; }

        public PipeProductViewModel Product { get; set; }

        [Display(Name = "Volume", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "Volume", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public decimal Volume { get; set; }

        [Display(Name = "Mass", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "Mass", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public decimal Mass { get; set; }
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<InnerPipelineDto, InnerPipelinesDataViewModel>()
                .ForMember(p=>p.Id,opt=>opt.MapFrom(p=>p.Product.Code))
                .ForMember(p=>p.Product, opt=>opt.MapFrom(p=>p.Product))
                .ForMember(p => p.Mass, opt => opt.MapFrom(p => p.Quantity.Mass))
                .ForMember(p => p.Volume, opt => opt.MapFrom(p => p.Quantity.Volume));
            configuration.CreateMap<InnerPipelinesDataViewModel, InnerPipelineData>()
            .ForMember(p => p.Product, opt => opt.Ignore())
            .ForMember(p => p.Id, opt => opt.Ignore());

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PipeProductViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public PipeProductTypeViewModel ProductType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PipeProductTypeViewModel : IMapFrom<ProductType>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SortableName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(this.Id.ToString("d3"));
                sb.Append(" ");
                sb.Append(this.Name);
                return sb.ToString();
            }
        }
    }
}