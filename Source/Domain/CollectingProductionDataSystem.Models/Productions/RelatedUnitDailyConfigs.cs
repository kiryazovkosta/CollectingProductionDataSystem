namespace CollectingProductionDataSystem.Models.Productions
{
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class RelatedUnitDailyConfigs : IEntity
    {
        public int UnitsDailyConfigId { get; set; }
        public int RelatedUnitsDailyConfigId { get; set; }
        public virtual UnitDailyConfig UnitsDailyConfig { get; set; }
        public virtual UnitDailyConfig RelatedUnitsDailyConfig { get; set; }
        public int Position { get; set; }

        [NotMapped]
        public int Id { get; set; }
    }
}
