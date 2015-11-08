namespace CollectingProductionDataSystem.Models.Transactions
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public class ActiveTransactionsData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public decimal Mass { get; set; }
        public decimal MassReverse { get; set; }
        public int MeasuringPointConfigId { get; set; }
        public virtual MeasuringPointConfig MeasuringPointConfig { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
