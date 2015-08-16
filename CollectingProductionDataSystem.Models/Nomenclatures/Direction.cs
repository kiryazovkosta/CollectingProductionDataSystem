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
        public Direction()
        {
            this.Units = new HashSet<UnitConfig>();
            this.MeasurementPointsProductsConfigs = new HashSet<MeasurementPointsProductsConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UnitConfig> Units { get; set; }
        public virtual ICollection<MeasurementPointsProductsConfig> MeasurementPointsProductsConfigs { get; set; }
    }
}
