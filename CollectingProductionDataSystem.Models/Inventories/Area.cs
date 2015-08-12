using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;

namespace CollectingProductionDataSystem.Models.Inventories
{
    public partial class Area: DeletableEntity, IEntity
    {
        public Area()
        {
            this.InventoryParks = new HashSet<Park>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Park> InventoryParks { get; set; }
    }
}
