using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    public partial class Product: DeletableEntity, IEntity
    {
        public Product()
        {
            this.InventoryTanksDatas = new HashSet<TankData>();
            this.Units = new HashSet<UnitConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public virtual ICollection<TankData> InventoryTanksDatas { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<UnitConfig> Units { get; set; }
    }
}
