namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class MonthlyProductType : DeletableEntity, IEntity
    {
        public MonthlyProductType()
        {
            this.UnitMonthlyConfigs = new HashSet<UnitMonthlyConfig>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UnitMonthlyConfig> UnitMonthlyConfigs { get; set; }
    }
}