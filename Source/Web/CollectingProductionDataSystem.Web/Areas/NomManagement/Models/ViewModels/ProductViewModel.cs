namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using Resources = App_GlobalResources.Resources;

    public class ProductViewModel:IMapFrom<Product>, IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public int Code { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "ProductType", ResourceType = typeof(Resources.Layout))]
        public int ProductTypeId { get; set; }

        [Required]
        [Display(Name = "IsAvailableForInnerPipeLine", ResourceType = typeof(Resources.Layout))]
        public bool IsAvailableForInnerPipeLine { get; set; }


        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }
   
        //[Display(Name = "ShiftProductType", ResourceType = typeof(Resources.Layout))]
        //public int ShiftProductTypeId { get; set; }

        //[Display(Name = "DailyProductType", ResourceType = typeof(Resources.Layout))]
        //public int DailyProductTypeId { get; set; }
        
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        //public void CreateMappings(IConfiguration configuration)
        //{
        //    //configuration.CreateMap<Product, ProductViewModel>()
        //    //    .ForMember(p => p.DailyProductTypeId, opt => opt.MapFrom(p => p.DailyProductTypeId == null ? 0 : (int)p.DailyProductTypeId))
        //    //    .ForMember(p => p.ShiftProductTypeId, opt => opt.MapFrom(p => p.ShiftProductTypeId == null ? 0 : (int)p.ShiftProductTypeId));

        //    //configuration.CreateMap<ProductViewModel, Product>()
        //    //    .ForMember(p => p.DailyProductTypeId, opt => opt.MapFrom(p => p.DailyProductTypeId == 0 ? null : (Nullable<int>)p.DailyProductTypeId))
        //    //    .ForMember(p => p.ShiftProductTypeId, opt => opt.MapFrom(p => p.ShiftProductTypeId == 0 ? null : (Nullable<int>)p.ShiftProductTypeId));
        //}
    }
}