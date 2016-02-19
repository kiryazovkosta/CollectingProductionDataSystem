

namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class UnitEnteredForCalculationData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public decimal OldValue { get; set; }
        public decimal NewValue { get; set; }
        public int UnitDataId { get; set; }
        public virtual UnitsData UnitsData { get; set; }
    }
}
