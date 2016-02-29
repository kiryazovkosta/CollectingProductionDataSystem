namespace CollectingProductionDataSystem.Models.Transactions
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class MeasuringPointProductsData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int MeasuringPointConfigId { get; set; }
        public decimal? Value { get; set; }
        public virtual MeasuringPointConfig MeasuringPointConfig { get; set; }
        public int ProductId { get; set; }
        public int DirectionId { get; set; }
    }
}
