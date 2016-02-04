namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class MonthlyReportType : DeletableEntity, IEntity
    {

        public MonthlyReportType()
        {
            this.UnitMonthlyConfigs = new HashSet<UnitMonthlyConfig>();
            this.UnitApprovedMonthlyDatas = new HashSet<UnitApprovedMonthlyData>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UnitMonthlyConfig> UnitMonthlyConfigs { get; set; }

        public virtual ICollection<UnitApprovedMonthlyData> UnitApprovedMonthlyDatas { get; set; }
    }
}