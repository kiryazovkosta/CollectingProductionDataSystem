namespace CollectingProductionDataSystem.Web.ViewModels.Home
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Inventories;

    public class IndexInventoryTanksViewModel : IMapFrom<TankConfig>
    {
        public string TankName { get; set; }
    }
}