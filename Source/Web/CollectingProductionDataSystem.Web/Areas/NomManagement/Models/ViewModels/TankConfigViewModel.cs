namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using Resources = App_GlobalResources.Resources;

    public class TankConfigViewModel : IMapFrom<TankConfig>, IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Park", ResourceType = typeof(Resources.Layout))]
        public int ParkId { get; set; }

        [Required]
        [Display(Name = "ControlPoint", ResourceType = typeof(Resources.Layout))]
        public string ControlPoint { get; set; }

        [Required]
        [Display(Name = "TankName", ResourceType = typeof(Resources.Layout))]
        public string TankName { get; set; }

        [Required]
        [Display(Name = "PhdTagProductId", ResourceType = typeof(Resources.Layout))]
        public string PhdTagProductId { get; set; }

        [Required]
        [Display(Name = "PhdTagProductName", ResourceType = typeof(Resources.Layout))]
        public string PhdTagProductName { get; set; }

        [Required]
        [Display(Name = "PhdTagLiquidLevel", ResourceType = typeof(Resources.Layout))]
        public string PhdTagLiquidLevel { get; set; }

        [Required]
        [Display(Name = "LiquidLevelLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal LiquidLevelLowExtreme { get; set; }

        [Required]
        [Display(Name = "LiquidLevelHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal LiquidLevelHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagProductLevel", ResourceType = typeof(Resources.Layout))]
        public string PhdTagProductLevel { get; set; }

        [Required]
        [Display(Name = "ProductLevelLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal ProductLevelLowExtreme { get; set; }

        [Required]
        [Display(Name = "ProductLevelHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal ProductLevelHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagFreeWaterLevel", ResourceType = typeof(Resources.Layout))]
        public string PhdTagFreeWaterLevel { get; set; }

        [Required]
        [Display(Name = "FreeWaterLevelLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal FreeWaterLevelLowExtreme { get; set; }

        [Required]
        [Display(Name = "FreeWaterLevelHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal FreeWaterLevelHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagFreeWaterVolume", ResourceType = typeof(Resources.Layout))]
        public string PhdTagFreeWaterVolume { get; set; }

        [Required]
        [Display(Name = "FreeWaterVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal FreeWaterVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "FreeWaterVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal FreeWaterVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagObservableDensity", ResourceType = typeof(Resources.Layout))]
        public string PhdTagObservableDensity { get; set; }

        [Required]
        [Display(Name = "ObservableDensityLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal ObservableDensityLowExtreme { get; set; }

        [Required]
        [Display(Name = "ObservableDensityHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal ObservableDensityHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagReferenceDensity", ResourceType = typeof(Resources.Layout))]
        public string PhdTagReferenceDensity { get; set; }

        [Required]
        [Display(Name = "ReferenceDensityLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal ReferenceDensityLowExtreme { get; set; }

        [Required]
        [Display(Name = "ReferenceDensityHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal ReferenceDensityHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagGrossObservableVolume", ResourceType = typeof(Resources.Layout))]
        public string PhdTagGrossObservableVolume { get; set; }

        [Required]
        [Display(Name = "GrossObservableVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossObservableVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "GrossObservableVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossObservableVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagGrossStandardVolume", ResourceType = typeof(Resources.Layout))]
        public string PhdTagGrossStandardVolume { get; set; }

        [Required]
        [Display(Name = "GrossStandardVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossStandardVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "GrossStandardVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal GrossStandardVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagNetStandardVolume", ResourceType = typeof(Resources.Layout))]
        public string PhdTagNetStandardVolume { get; set; }

        [Required]
        [Display(Name = "NetStandardVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal NetStandardVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "NetStandardVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal NetStandardVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagWeightInAir", ResourceType = typeof(Resources.Layout))]
        public string PhdTagWeightInAir { get; set; }

        [Required]
        [Display(Name = "WeightInAirLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal WeightInAirLowExtreme { get; set; }

        [Required]
        [Display(Name = "WeightInAirHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal WeightInAirHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagAverageTemperature", ResourceType = typeof(Resources.Layout))]
        public string PhdTagAverageTemperature { get; set; }

        [Required]
        [Display(Name = "AverageTemperatureLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageTemperatureLowExtreme { get; set; }

        [Required]
        [Display(Name = "AverageTemperatureHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AverageTemperatureHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagTotalObservableVolume", ResourceType = typeof(Resources.Layout))]
        public string PhdTagTotalObservableVolume { get; set; }

        [Required]
        [Display(Name = "TotalObservableVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalObservableVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "TotalObservableVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal TotalObservableVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagWeightInVacuum", ResourceType = typeof(Resources.Layout))]
        public string PhdTagWeightInVacuum { get; set; }

        [Required]
        [Display(Name = "WeightInVacuumLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal WeightInVacuumLowExtreme { get; set; }

        [Required]
        [Display(Name = "WeightInVacuumHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal WeightInVacuumHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagMaxVolume", ResourceType = typeof(Resources.Layout))]
        public string PhdTagMaxVolume { get; set; }

        [Required]
        [Display(Name = "MaxVolumeLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal MaxVolumeLowExtreme { get; set; }

        [Required]
        [Display(Name = "MaxVolumeHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal MaxVolumeHighExtreme { get; set; }

        [Required]
        [Display(Name = "PhdTagAvailableRoom", ResourceType = typeof(Resources.Layout))]
        public string PhdTagAvailableRoom { get; set; }

        [Required]
        [Display(Name = "AvailableRoomLowExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AvailableRoomLowExtreme { get; set; }

        [Required]
        [Display(Name = "AvailableRoomHighExtreme", ResourceType = typeof(Resources.Layout))]
        public decimal AvailableRoomHighExtreme { get; set; }

        [Required]
        [Display(Name = "UnusableResidueLevel", ResourceType = typeof(Resources.Layout))]
        public decimal UnusableResidueLevel { get; set; }

        [Required]
        [Display(Name = "CorrectionFactor", ResourceType = typeof(Resources.Layout))]
        public string CorrectionFactor { get; set; }
    }
}