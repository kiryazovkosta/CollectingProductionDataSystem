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
        private ICollection<TankData> tanksDatas;
        private ICollection<UnitConfig> unitConfigs;

        public Product()
        {
            this.tanksDatas = new HashSet<TankData>();
            this.unitConfigs = new HashSet<UnitConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public virtual ICollection<TankData> TanksDatas 
        {
            get { return this.tanksDatas; }
            set { this.tanksDatas = value; }
        }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<UnitConfig> Units 
        {
            get { return this.unitConfigs; }
            set { this.unitConfigs = value; }
        }
    }
}
