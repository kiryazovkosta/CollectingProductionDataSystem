namespace CollectingProductionDataSystem.Models.Inventories
{
    using System;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class InProcessUnitData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime Recordimestamp { get; set; }
        public int ProcessUnitId { get; set; }
        public int ProductId { get; set; }
        public decimal Mass { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        public virtual Product Product { get; set; }
    }
}
