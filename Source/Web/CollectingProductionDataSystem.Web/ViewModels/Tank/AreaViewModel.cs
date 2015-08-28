using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.ViewModels.Tank
{
    public class AreaViewModel: IMapFrom<Area>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}