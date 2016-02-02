namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Contracts;

    public class RelatedUnitMonthlyConfigs : IEntity
    {
        public int UnitsMonthlyConfigId { get; set; }

        public int RelatedUnitsMonthlyConfigId { get; set; }

        public virtual UnitMonthlyConfig UnitsMonthlyConfig { get; set; }

        public virtual UnitMonthlyConfig RelatedUnitsMonthlyConfig { get; set; }

        public int Position { get; set; }

        [NotMapped]
        public int Id { get; set; }
    }
}