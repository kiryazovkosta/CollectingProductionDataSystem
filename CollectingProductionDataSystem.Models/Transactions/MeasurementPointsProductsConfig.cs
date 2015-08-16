namespace CollectingProductionDataSystem.Models.Transactions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public partial class MeasurementPointsProductsConfig : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public string PhdTotalCounterTag { get; set; }
        public int ProductId { get; set; }
        public int MeasurementPointId { get; set; }
        public int DirectionId { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual MeasurementPoint MeasurementPoint { get; set; }
        public virtual Product Product { get; set; }
    }
}
