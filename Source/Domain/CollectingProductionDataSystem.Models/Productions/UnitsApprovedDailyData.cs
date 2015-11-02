namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Abstract;

    public partial class UnitsApprovedDailyData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public int ProcessUnitId { get; set; }
        public bool Approved { get; set; }
    }
}
