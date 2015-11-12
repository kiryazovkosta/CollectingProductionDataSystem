namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Transactions;

    public partial class Direction: DeletableEntity, IEntity
    {
        private ICollection<UnitConfig> unitConfigs;
        private ICollection<MeasuringPointConfig> measuringPointConfigs;
        private ICollection<MeasuringPointProductsConfig> measuringPointProductsConfigs;

        public Direction()
        {
            this.unitConfigs = new HashSet<UnitConfig>();
            this.measuringPointConfigs = new HashSet<MeasuringPointConfig>();
            this.measuringPointProductsConfigs = new HashSet<MeasuringPointProductsConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UnitConfig> UnitConfigs 
        {
            get { return this.unitConfigs; }
            set { this.unitConfigs = value; } 
        }
        public virtual ICollection<MeasuringPointConfig> MeasuringPointConfigs 
        {
            get { return this.measuringPointConfigs; }
            set { this.measuringPointConfigs = value; } 
        }
        public virtual ICollection<MeasuringPointProductsConfig> MeasuringPointProductsConfigs 
        {
            get { return this.measuringPointProductsConfigs; }
            set { this.measuringPointProductsConfigs = value; }
        }
    }
}
