namespace CollectingProductionDataSystem.Models.Transactions
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public partial class MeasuringPointProductsConfig : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public int MeasuringPointConfigId { get; set; }
        public string PhdProductTotalizerTag { get; set; }
        public int ProductId { get; set; }
        public int DirectionId { get; set; }
        public virtual MeasuringPointConfig MeasuringPointConfig { get; set; }
        public virtual Product Product { get; set; }
        public virtual Direction Direction { get; set; }
    }
}
