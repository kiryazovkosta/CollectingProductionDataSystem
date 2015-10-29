namespace CollectingProductionDataSystem.Models.Productions
{
    using CollectingProductionDataSystem.Models.Contracts;
    using System;
    using System.Linq;

    public partial class RelatedUnitConfigs : IEntity
    {
        public int UnitConfigId { get; set; }

        public int RelatedUnitConfigId { get; set; }

        public virtual UnitConfig UnitConfig { get; set; }

        public virtual UnitConfig RelatedUnitConfig { get; set; }

        public int Position { get; set; }

        public int Id { get; set; }
    }
}
