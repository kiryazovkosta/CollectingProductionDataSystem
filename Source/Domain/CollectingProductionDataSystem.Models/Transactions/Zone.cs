namespace CollectingProductionDataSystem.Models.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class Zone : DeletableEntity, IEntity
    {
        public Zone()
        {
            this.MeasurementPoints = new HashSet<MeasurementPoint>();
            this.MeasuringPointConfigs = new HashSet<MeasuringPointConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int IkunkId { get; set; }
        public virtual Ikunk Ikunk { get; set; }
        public virtual ICollection<MeasurementPoint> MeasurementPoints { get; set; }
        public virtual ICollection<MeasuringPointConfig> MeasuringPointConfigs { get; set; }
    }
}
