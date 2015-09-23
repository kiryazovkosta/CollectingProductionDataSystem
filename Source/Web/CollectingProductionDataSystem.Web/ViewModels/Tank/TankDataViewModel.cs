namespace CollectingProductionDataSystem.Web.ViewModels.Tank
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Inventories;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Web.InputModels;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public class TankDataViewModel:IMapFrom<TankData>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        public int ParkId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductName", ResourceType = typeof(Resources.Layout))]
        public string ProductName { get; set; }

        [Display(Name = "LiquidLevel", ResourceType = typeof(Resources.Layout))]
        public decimal? LiquidLevel { get; set; }

        [Display(Name = "ProductLevel", ResourceType = typeof(Resources.Layout))]
        public decimal? ProductLevel { get; set; }

        [Display(Name = "NetStandardVolume", ResourceType = typeof(Resources.Layout))]
        public decimal? NetStandardVolume { get; set; }

        [Display(Name = "ReferenceDensity", ResourceType = typeof(Resources.Layout))]
        public decimal? ReferenceDensity { get; set; }

        [Display(Name = "WeightInAir", ResourceType = typeof(Resources.Layout))]
        public decimal? WeightInAir { get; set; }

        [Display(Name = "WeightInVacuum", ResourceType = typeof(Resources.Layout))]
        public decimal? WeightInVacuum { get; set; }

        [Display(Name = "FreeWaterLevel", ResourceType = typeof(Resources.Layout))]
        public decimal? FreeWaterLevel { get; set; }

        public int ShiftId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "TankName", ResourceType = typeof(Resources.Layout))]
        public string TankName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ParkName", ResourceType = typeof(Resources.Layout))]
        public string ParkName { get; set; }

        public TanksManualDataViewModel TanksManualData { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<TankData, TankDataViewModel>()
                .ForMember(p => p.TankName, opt => opt.MapFrom(p => p.TankConfig.TankName))
                .ForMember(p => p.ParkName, opt => opt.MapFrom(p => p.TankConfig.Park.Name))
                .ForMember(p => p.LiquidLevel, opt => opt.MapFrom(p => p.TanksManualData != null ? p.TanksManualData.LiquidLevel : p.LiquidLevel))
                .ForMember(p => p.ProductLevel, opt => opt.MapFrom(p => p.TanksManualData != null ? p.TanksManualData.ProductLevel : p.ProductLevel))
                .ForMember(p => p.NetStandardVolume, opt => opt.MapFrom(p => p.TanksManualData != null ? p.TanksManualData.NetStandardVolume : p.NetStandardVolume))
                .ForMember(p => p.ReferenceDensity, opt => opt.MapFrom(p => p.TanksManualData != null ? p.TanksManualData.ReferenceDensity : p.ReferenceDensity))
                .ForMember(p => p.WeightInAir, opt => opt.MapFrom(p => p.TanksManualData != null ? p.TanksManualData.WeightInAir : p.WeightInAir))
                .ForMember(p => p.WeightInVacuum, opt => opt.MapFrom(p => p.TanksManualData != null ? p.TanksManualData.WeightInVacuum : p.WeightInVacuum))
                .ForMember(p => p.FreeWaterLevel, opt => opt.MapFrom(p => p.TanksManualData != null ? p.TanksManualData.FreeWaterLevel : p.FreeWaterLevel))
                .ForMember(p => p.TanksManualData, opt => opt.MapFrom(p => p.TanksManualData ?? new TanksManualData() { }));
        }
    }

    public class TanksManualDataViewModel : IMapFrom<TanksManualData>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "EditReason", ResourceType = typeof(Resources.Layout))]
        public EditReasonInputModel EditReason { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<TanksManualData, TanksManualDataViewModel>()
                .ForMember(p => p.EditReason, opt => opt.MapFrom(p => p.EditReason ?? new EditReason() { Id = 0, Name = Resources.Layout.AutomaticData }));
        }
    }


}