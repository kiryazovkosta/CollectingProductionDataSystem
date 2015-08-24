using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Inventories;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.ViewModels.Tank
{
    public class TankDataViewModel:IMapFrom<TankData>, IHaveCustomMappings
    {
        [Display(Name="Id", ResourceType=typeof(Resources.Layout))]
        public int Id { get; set; }

        //[Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        //public DateTime RecordTimestamp { get; set; }
        //public int ParkId { get; set; }
        public string ProductName { get; set; }
        public decimal? LiquidLevel { get; set; }
        public decimal? ProductLevel { get; set; }
        public decimal? NetStandardVolume { get; set; }
        public decimal? ReferenceDensity { get; set; }
        public decimal? WeightInAir { get; set; }
        public decimal? WeightInVacuum { get; set; }
        public decimal? FreeWaterLevel { get; set; }
        public virtual string TankName { get; set; }
        public virtual string ParkName { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<TankData, TankDataViewModel>()
                .ForMember(p => p.TankName, opt => opt.MapFrom(p => p.TankConfig.TankName));
            configuration.CreateMap<TankData, TankDataViewModel>()
                .ForMember(p => p.ParkName, opt => opt.MapFrom(p => p.TankConfig.Park.Name));
        }
    }
}