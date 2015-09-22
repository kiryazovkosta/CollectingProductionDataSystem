namespace CollectingProductionDataSystem.Models.Productions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using System;
    using CollectingProductionDataSystem.Models.Contracts;

    public class ProductionPlanConfig : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Percentages { get; set; }
        public string QuantityPlanFormula { get; set; }
        public string QuantityPlanMembers { get; set; }
        public string QuantityFactFormula { get; set; }
        public string QuantityFactMembers { get; set; }
        public int ProcessUnitId { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
    }
}