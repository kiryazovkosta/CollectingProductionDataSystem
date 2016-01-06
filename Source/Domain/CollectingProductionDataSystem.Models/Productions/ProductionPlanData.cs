namespace CollectingProductionDataSystem.Models.Productions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;
    using System.Linq;

    public class ProductionPlanData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public int ProductionPlanConfigId { get; set; }
        public virtual ProductionPlanConfig ProductionPlanConfig { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public decimal PercentagesPlan { get; set; }
        public decimal QuanityPlan { get; set; }
        public decimal PercentagesFact { get; set; }
        public decimal QuantityFact { get; set; }
        public decimal QuanityFactCurrentMonth { get; set; }
        public decimal PercentagesFactCurrentMonth { get; set; }
        public string Name { get; set; }
        public int FactoryId { get; set; }
        public int ProcessUnitId { get; set; }
    }
}
