namespace CollectingProductionDataSystem.Models.Transactions
{
    using System;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class MaxAsoMeasuringPointDataSequenceNumber : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public long MaxSequenceNumber { get; set; }
        public long MaxScaleSequenceNumber { get; set; }

        public DateTime? LastFetchScales { get; set; }
    }
}