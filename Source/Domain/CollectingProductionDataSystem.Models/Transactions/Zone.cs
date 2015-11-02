namespace CollectingProductionDataSystem.Models.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class Zone : DeletableEntity, IEntity
    {
        private ICollection<MeasuringPointConfig> measuringPointConfigs;
        public Zone()
        {
            this.measuringPointConfigs = new HashSet<MeasuringPointConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int IkunkId { get; set; }
        public virtual Ikunk Ikunk { get; set; }
        public virtual ICollection<MeasuringPointConfig> MeasuringPointConfigs 
        {
            get { return this.measuringPointConfigs; }
            set { this.measuringPointConfigs = value; } 
        }
    }
}
