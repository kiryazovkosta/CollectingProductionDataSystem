namespace CollectingProductionDataSystem.Web.ViewModels.Tank
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Inventories;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Web.InputModels;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public class TankDataViewModel : IMapFrom<TankData>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Display(Name = "ControlPoint", ResourceType = typeof(Resources.Layout))]
        public string ControlPoint { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        public int ParkId { get; set; }

        public ParkViewModel Park { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ProductName", ResourceType = typeof(Resources.Layout))]
        public string ProductName { get; set; }

        [Display(Name = "LiquidLevel", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "LiquidLevel", ErrorMessageResourceType = typeof(Resources.Layout))]
        public decimal? LiquidLevel { get; set; }

        [Display(Name = "ProductLevel", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "ProductLevel", ErrorMessageResourceType = typeof(Resources.Layout))]
        public decimal? ProductLevel { get; set; }

        [Display(Name = "NetStandardVolume", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "NetStandardVolume", ErrorMessageResourceType = typeof(Resources.Layout))]
        public decimal? NetStandardVolume { get; set; }

        [Display(Name = "ReferenceDensity", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "ReferenceDensity", ErrorMessageResourceType = typeof(Resources.Layout))]
        public decimal? ReferenceDensity { get; set; }

        [Display(Name = "WeightInAir", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "WeightInAir", ErrorMessageResourceType = typeof(Resources.Layout))]
        public decimal? WeightInAir { get; set; }

        [Display(Name = "WeightInVacuum", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "WeightInVacuum", ErrorMessageResourceType = typeof(Resources.Layout))]
        public decimal? WeightInVacuum { get; set; }

        [Display(Name = "FreeWaterLevel", ResourceType = typeof(Resources.Layout))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = "FreeWaterLevel", ErrorMessageResourceType = typeof(Resources.Layout))]
        public decimal? FreeWaterLevel { get; set; }

        public int ShiftId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "TankName", ResourceType = typeof(Resources.Layout))]
        public string TankName { get; set; }



        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "ParkName", ResourceType = typeof(Resources.Layout))]
        public string ParkName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "TankStatus", ResourceType = typeof(Resources.Layout))]
        public string StatusOfTank { get; set; }

        public int TankId { get; set; }

        public string SortableName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(this.ParkId.ToString("d2"));
                sb.Append(" ");
                sb.Append(this.ParkName);
                return sb.ToString();
            }
        }


        public int TankNumber
        {
            get
            {
                var numberString = new string(this.TankName.Where(x => Char.IsDigit(x)).ToArray());
                numberString = string.IsNullOrEmpty(numberString) ? "0" : numberString;
                return Int32.Parse(numberString);
            }

        }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<TankData, TankDataViewModel>()
                .ForMember(p => p.TankId, opt => opt.MapFrom(p => p.TankConfigId))
                .ForMember(p => p.TankName, opt => opt.MapFrom(p => p.TankConfig.TankName))
                .ForMember(p => p.ParkName, opt => opt.MapFrom(p => p.TankConfig.Park.Name))
                .ForMember(p => p.LiquidLevel, opt => opt.MapFrom(p => p.CorrectedLiquidLevel))
                .ForMember(p => p.ProductLevel, opt => opt.MapFrom(p => p.CorrectedProductLevel))
                .ForMember(p => p.FreeWaterLevel, opt => opt.MapFrom(p => p.CorrectedFreeWaterLevel))
                .ForMember(p => p.ControlPoint, opt => opt.MapFrom(p => p.TankConfig.ControlPoint == "n/a" ? string.Format("{0}{1}", p.TankConfig.Park.Name, p.TankConfig.TankName) : p.TankConfig.ControlPoint));

        }
    }
}