namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class RelatedUnitDailyConfigs
    {
        public int UnitsDailyConfigId { get; set; }
        public int RelatedUnitsDailyConfigId { get; set; }
        public virtual UnitsDailyConfig UnitsDailyConfig { get; set; }
        public virtual UnitsDailyConfig RelatedUnitsDailyConfig { get; set; }
    }
}
