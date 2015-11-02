namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;

    public partial class ShiftProductType: DeletableEntity, IEntity
    {
        public ShiftProductType()
        {
            this.UnitConfigs = new HashSet<UnitConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UnitConfig> UnitConfigs { get; set; }
    }
}
