namespace CollectingProductionDataSystem.Models.Inventories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class TankStatusData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int TankConfigId { get; set; }
        public virtual TankConfig TankConfig { get; set; }
        public int TankStatusId { get; set; }
        public virtual TankStatus TankStatus { get; set; }
    }
}
