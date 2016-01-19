using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    public class ProductionPlanConfigViewModel : IMapFrom<ProductionPlanConfig>, IEntity, IHaveCustomMappings
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Percentages", ResourceType = typeof(Resources.Layout))]
        public decimal Percentages { get; set; }

        [Required]
        [Display(Name = "QuantityPlanFormula", ResourceType = typeof(Resources.Layout))]
        public string QuantityPlanFormula { get; set; }

        [Required]
        [Display(Name = "QuantityPlanMembers", ResourceType = typeof(Resources.Layout))]
        public string QuantityPlanMembers { get; set; }

        [Required]
        [Display(Name = "QuantityFactFormula", ResourceType = typeof(Resources.Layout))]
        public string QuantityFactFormula { get; set; }

        [Required]
        [Display(Name = "QuantityFactMembers", ResourceType = typeof(Resources.Layout))]
        public string QuantityFactMembers { get; set; }

        [Required]
        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public int ProcessUnitId { get; set; }

        [Required]
        [Display(Name = "MaterialType", ResourceType = typeof(Resources.Layout))]
        public int MaterialTypeId { get; set; }

        [Required]
        [Display(Name = "MaterialDetailType", ResourceType = typeof(Resources.Layout))]
        public int MaterialDetailTypeId { get; set; }

        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductionPlanConfig, ProductionPlanConfigViewModel>()
                    .ForMember(p => p.MaterialDetailTypeId, opt => opt.MapFrom(p => p.MaterialDetailTypeId == null ? 0 : p.MaterialDetailTypeId));

            configuration.CreateMap<ProductionPlanConfigViewModel, ProductionPlanConfig>()
                    .ForMember(p => p.MaterialDetailTypeId, opt => opt.MapFrom(p => p.MaterialDetailTypeId == 0 ? null : (int?)p.MaterialDetailTypeId));
        }
    }
}