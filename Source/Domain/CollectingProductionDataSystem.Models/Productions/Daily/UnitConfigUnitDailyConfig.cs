/// <summary>
/// Summary description for UnitConfigUnitDailyConfig
/// </summary>
namespace CollectingProductionDataSystem.Models.Productions
{
    using CollectingProductionDataSystem.Models.Contracts;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UnitConfigUnitDailyConfig : IEntity
    {
        [NotMapped]
        public int Id { get; set; }
            
        public int UnitConfigId { get; set; }

        public int UnitDailyConfigId { get; set; }

        public virtual UnitConfig UnitConfig { get; set; }

        public virtual UnitDailyConfig UnitDailyConfig { get; set; }

        public int Position { get; set; }
    }
}
