namespace CollectingProductionDataSystem.Models.Productions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public partial class UnitsAggregateDailyConfig : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProcessUnitId { get; set; }
        public int ProductTypeId { get; set; }
        public int MeasureUnitId { get; set; }
        public int AggregationFormulaId { get; set; }
        public AggregationPriority AggregationPriority { get; set; }
        public virtual AggregationsFormula AggregationsFormula { get; set; }
        public virtual MeasureUnit MeasureUnit { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}
