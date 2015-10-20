/// <summary>
/// Summary description for UnitConfigUnitDailyConfig
/// </summary>
namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class UnitConfigUnitDailyConfig : IEntity
    {
        [NotMapped]
        public int Id { get; set; }
            
        public int UnitConfigId { get; set; }

        public int UnitDailyConfigId { get; set; }

        public virtual UnitConfig UnitConfig { get; set; }

        public virtual UnitDailyConfig UnitDailyConfig { get; set; }
    }
}
