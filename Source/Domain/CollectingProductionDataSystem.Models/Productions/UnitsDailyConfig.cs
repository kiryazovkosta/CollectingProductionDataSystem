namespace CollectingProductionDataSystem.Models.Productions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.Collections.Generic;

    public partial class UnitsDailyConfig : DeletableEntity, IEntity
    {
        public UnitsDailyConfig()
        {
            this.UnitsDailyDatas = new HashSet<UnitsDailyData>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProcessUnitId { get; set; }
        public int ProductId { get; set; }
        public int MeasureUnitId { get; set; }
        public string AggregationFormula { get; set; }
        public bool AggregationCurrentLevel { get; set; }
        public string AggregationMembers { get; set; }
        public virtual MeasureUnit MeasureUnit { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<UnitsDailyData> UnitsDailyDatas { get; set; }
    }
}
