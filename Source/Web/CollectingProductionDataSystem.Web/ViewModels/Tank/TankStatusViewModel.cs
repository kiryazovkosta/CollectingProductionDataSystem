namespace CollectingProductionDataSystem.Web.ViewModels.Tank
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Inventories;

    public class TankStatusViewModel : IMapFrom<TankStatus>
    {
        public int Id { get; set; }
        public int FlagValue { get; set; }
        public string Name { get; set; }
    }
}