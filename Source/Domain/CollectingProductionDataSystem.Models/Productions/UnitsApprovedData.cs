namespace CollectingProductionDataSystem.Models.Productions
{
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;

    public partial class UnitsApprovedData  : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public int ShiftId { get; set; }
        public int ProcessUnitId { get; set; }
        public bool Approved { get; set; }
    }
}
