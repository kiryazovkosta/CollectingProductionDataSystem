namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitDailyConfigUnitMonthlyConfig : IEntity
    {
        [NotMapped]
        public int Id { get; set; }
            
        public int UnitDailyConfigId { get; set; }

        public int UnitMonthlyConfigId { get; set; }

        public virtual UnitDailyConfig UnitDailyConfig { get; set; }

        public virtual UnitMonthlyConfig UnitMonthlyConfig { get; set; }

        public int Position { get; set; }
    }
}