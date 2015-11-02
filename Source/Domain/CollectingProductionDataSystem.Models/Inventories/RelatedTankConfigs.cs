namespace CollectingProductionDataSystem.Models.Inventories
{
    using System;
    using System.Linq;

    public partial class RelatedTankConfigs
    {
        public int TankConfigId { get; set; }
        public int RelatedTankConfigId { get; set; }
        public virtual TankConfig TankConfig { get; set; }
        public virtual TankConfig RelatedTankConfig { get; set; }
    }
}
