namespace CollectingProductionDataSystem.Models.Inventories
{
    using System;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class TanksApprovedData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public int ShiftId { get; set; }
        public int ParkId { get; set; }
        public bool Approved { get; set; }
    }
}
