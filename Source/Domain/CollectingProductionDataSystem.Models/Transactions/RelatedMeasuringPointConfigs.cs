namespace CollectingProductionDataSystem.Models.Transactions
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;

    public partial class RelatedMeasuringPointConfigs
    {
        public int Position { get; set; }
        public int MeasuringPointConfigId { get; set; }
        public int RelatedMeasuringPointConfigId { get; set; }
        public virtual MeasuringPointConfig MeasuringPointConfig { get; set; }
        public virtual MeasuringPointConfig RelatedMeasuringPointConfig { get; set; }
    }
}
