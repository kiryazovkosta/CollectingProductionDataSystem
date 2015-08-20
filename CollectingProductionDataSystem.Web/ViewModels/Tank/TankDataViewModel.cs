using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Inventories;

namespace CollectingProductionDataSystem.Web.ViewModels.Tank
{
    public class TankDataViewModel:IMapFrom<TankData>, IHaveCustomMappings
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int ParkId { get; set; }   
        public decimal? LiquidLevel { get; set; }
        public decimal? ProductLevel { get; set; }
        public decimal? ReferenceDensity { get; set; }
        public decimal? WeightInVacuum { get; set; }
        public virtual string TankName { get; set; }
        
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<TankData, TankDataViewModel>()
                .ForMember(p => p.TankName, opt => opt.MapFrom(p => p.TankConfig.TankName));
        }
    }
}