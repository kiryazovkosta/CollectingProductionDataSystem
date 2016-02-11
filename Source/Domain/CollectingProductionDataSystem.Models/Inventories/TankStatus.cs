namespace CollectingProductionDataSystem.Models.Inventories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class TankStatus : DeletableEntity, IEntity
    {
        private ICollection<TankStatusData> tankStatusDatas;

        public TankStatus()
        {
            this.tankStatusDatas = new HashSet<TankStatusData>();
        }

        public int Id { get; set; }
        public int FlagValue { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TankStatusData> TankStatusData 
        {
            get { return this.tankStatusDatas; }
            set { this.tankStatusDatas = value; }
        }
    }
}
