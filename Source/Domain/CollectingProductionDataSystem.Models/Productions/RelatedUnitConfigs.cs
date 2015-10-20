namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Linq;

    public partial class RelatedUnitConfigs
    {
        public int UnitConfigId { get; set; }

        public int RelatedUnitConfigId { get; set; }

        public virtual UnitConfig UnitConfig { get; set; }

        public virtual UnitConfig RelatedUnitConfig { get; set; }

        public int Position { get; set; }
    }
}
