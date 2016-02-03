namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Contracts;

    public class RelatedUnitMonthlyConfigs : IEntity
    {
        public int UnitMonthlyConfigId { get; set; }

        public int RelatedUnitMonthlyConfigId { get; set; }

        public virtual UnitMonthlyConfig UnitMonthlyConfig { get; set; }

        public virtual UnitMonthlyConfig RelatedUnitMonthlyConfig { get; set; }

        public int Position { get; set; }

        [NotMapped]
        public int Id { get; set; }
    }
}