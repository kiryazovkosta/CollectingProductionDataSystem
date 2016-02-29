namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels
{
    using System;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.TankDataServices;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Inventories;
    using System.ComponentModel.DataAnnotations;
    using Resources = App_GlobalResources.Resources;

    public class TanksStatusDataViewModel : IMapFrom<StatusOfTankDto>, IHaveCustomMappings
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "ControlPoint", ResourceType = typeof(Resources.Layout))]
        public string ControlPoint { get; set; }

        [Required]
        [UIHint("Hidden")]
        public int TankConfigId { get; set; }

        [Required]
        [Display(Name = "TankName", ResourceType = typeof(Resources.Layout))]
        public string TankName { get; set; }

        public int ParkId { get; set; }

        [Required]
        [Display(Name = "ParkName", ResourceType = typeof(Resources.Layout))]
        public string ParkName { get; set; }

        [Required]
        [Display(Name = "RecordTimestamp", ResourceType = typeof(Resources.Layout))]
        public DateTime RecordTimestamp { get; set; }

        [Required]
        [Display(Name = "TankStatus", ResourceType = typeof(Resources.Layout))]
        public int TankStatusId { get; set; }

        public int TankNumber
        {
            get
            {
                var numberString = new string(this.TankName.Where(x => Char.IsDigit(x)).ToArray());
                numberString = string.IsNullOrEmpty(numberString) ? "0" : numberString;
                return Int32.Parse(numberString);
            }
        }

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

        public bool IsEditable { get { return true; } }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<StatusOfTankDto, TanksStatusDataViewModel>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => p.Tank.Id))
                .ForMember(p => p.ControlPoint, opt => opt.MapFrom(p => p.Tank.ControlPoint))
                .ForMember(p => p.TankConfigId, opt => opt.MapFrom(p => p.Tank.Id))
                .ForMember(p => p.TankName, opt => opt.MapFrom(p => p.Tank.TankName))
                .ForMember(p => p.ParkName, opt => opt.MapFrom(p => p.Tank.Park.Name))
                .ForMember(p => p.TankStatusId, opt => opt.MapFrom(p => p.Quantity.TankStatus.Id))
                .ForMember(p=>p.ParkId, opt=>opt.MapFrom(p=>p.Tank.ParkId));
            configuration.CreateMap<TanksStatusDataViewModel, TankStatusData>()
                .ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}