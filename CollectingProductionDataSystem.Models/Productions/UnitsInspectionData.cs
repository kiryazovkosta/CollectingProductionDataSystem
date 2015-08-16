namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class UnitsInspectionData : AuditInfo, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int UnitConfigId { get; set; }
        public decimal Value { get; set; }
        public virtual UnitConfig Unit { get; set; }
    }
}
