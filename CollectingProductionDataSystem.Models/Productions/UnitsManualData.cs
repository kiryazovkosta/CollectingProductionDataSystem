namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class UnitsManualData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public virtual UnitsData UnitsData { get; set; }
    }
}
