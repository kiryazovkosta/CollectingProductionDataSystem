using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Infrastructure.Mapping;

namespace CollectingProductionDataSystem.Web.ViewModels.Tank
{
    public class ParkViewModel : IMapFrom<Park>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}