namespace CollectingProductionDataSystem.Web.ViewModels.Home
{
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class IndexInventoryTanksViewModel : IMapFrom<InventoryTank>
    {
        public string TankName { get; set; }
    }
}