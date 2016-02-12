namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels
{
    using System;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using System.ComponentModel.DataAnnotations;
    using Resources = App_GlobalResources.Resources;

    public class TanksStatusesViewModel : IMapFrom<StatusOfTankViewModel>
    {
        public int Id { get; set; }
        public string TankName { get; set; }
        public string ParkName { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int ParkId { get; set; }
        public int TankStatusDataId { get; set; }
    }

    //    public class TanksStatusesViewModel : IMapFrom<StatusOfTankViewModel>, IHaveCustomMappings
    //{
    //    public int Id { get; set; }
    //    public string TankName { get; set; }
    //    public int ParkId { get; set; }
    //    public ParkViewModel ParkName { get; set; }
    //    public int TankStatusDataId { get; set; }
    //    public TankStatusDataViewModel TankStatusData { get; set; }
    //    public void CreateMappings(IConfiguration configuration)
    //    {
    //        configuration.CreateMap<StatusOfTankViewModel, TanksStatusesViewModel>()
    //            .ForMember(p => p., opt => opt.MapFrom(p => p.Name));
    //    }
    //}

    public class TankStatusDataViewModel : IMapFrom<TankStatusData>
    {
        public DateTime RecordTimestamp { get; set; }
        public int TankStatusId { get; set; }
        public TankStatusViewModel TankStatus { get; set; }
    }
 
    public class TankStatusViewModel : IMapFrom<TankStatus>
    {
        public int Id { get; set; }
        public int FlagValue { get; set; }
        public string Name { get; set; }
    }

        public class StatusOfTankViewModel
    {
        public int Id { get; set; }
        public string TankName { get; set; }
        public string ParkName { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public TankStatus Status { get; set; }
    }
}