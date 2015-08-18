namespace CollectingProductionDataSystem.Models.Productions
{
    using System.Collections.Generic;

    public partial class AggregationsFormula
    {
        public AggregationsFormula()
        {
            this.UnitsAggregateDailyDatas = new HashSet<UnitsAggregateDailyConfig>();
        }

        public int Id { get; set; }
        public string Formula { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UnitsAggregateDailyConfig> UnitsAggregateDailyDatas { get; set; }
    }
}
