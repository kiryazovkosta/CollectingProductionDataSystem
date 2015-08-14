using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Nomenclatures;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    public partial class ProductType: DeletableEntity, IEntity
    {
        public ProductType()
        {
            this.Products = new List<Product>();
            this.Units = new List<UnitConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UnitConfig> Units { get; set; }
    }
}