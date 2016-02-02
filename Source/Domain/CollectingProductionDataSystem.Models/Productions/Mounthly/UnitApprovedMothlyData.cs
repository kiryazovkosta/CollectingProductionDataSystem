namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class UnitApprovedMothlyData : DeletableEntity, IEntity
    {
        public int Id { get; set; }

        public DateTime RecordDate { get; set; }

        public int ProcessUnitId { get; set; }

        public bool Approved { get; set; }

    }
}