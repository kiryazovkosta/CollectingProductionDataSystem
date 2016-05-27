namespace CollectingProductionDataSystem.Models.Productions
{
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial  class RelatedProductionPlanConfigs : IEntity
    {
        public int ProductionPlanConfigId { get; set; }
        public int RelatedProductionPlanConfigId { get; set; }
        public virtual ProductionPlanConfig ProductionPlanConfig { get; set; }
        public virtual ProductionPlanConfig RelatedProductionPlanConfig { get; set; }
        public int Position { get; set; }

        [NotMapped]
        public int Id { get; set; }
    }
}
