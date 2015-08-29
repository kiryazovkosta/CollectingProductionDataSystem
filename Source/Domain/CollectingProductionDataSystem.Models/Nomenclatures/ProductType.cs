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
            this.Products = new HashSet<Product>();
            this.Units = new HashSet<UnitConfig>();
            this.UnitsDailyConfigs = new HashSet<UnitsDailyConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UnitConfig> Units { get; set; }
        public virtual ICollection<UnitsDailyConfig> UnitsDailyConfigs { get; set; }
    }
}
