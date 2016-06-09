namespace CollectingProductionDataSystem.Models.Productions.Daily
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class RelatedUnitDailyConfigAspenCode : IEntity
    {
        public int Id { get; set; }

        public int UnitDailyConfigId { get; set; }

        public string AspenCode { get; set; }
    }
}
