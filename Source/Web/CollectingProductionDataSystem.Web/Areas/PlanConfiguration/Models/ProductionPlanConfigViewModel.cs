using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Models
{
    public class ProductionPlanConfigViewModel : IMapFrom<ProductionPlanConfig>, IHaveCustomMappings
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductionPlanConfig, ProductionPlanConfigViewModel>()
                .ForMember(p => p.DisplayName, opt => opt.MapFrom(p => string.Format("{0} - {1}", p.Code, p.Name)));
        }
    }
}