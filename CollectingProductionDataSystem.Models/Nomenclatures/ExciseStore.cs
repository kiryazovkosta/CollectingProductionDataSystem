using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Inventories;

namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    public partial class ExciseStore: DeletableEntity, IEntity
    {
        public ExciseStore()
        {
            this.InventoryParks = new HashSet<Park>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Park> InventoryParks { get; set; }
    }
}
