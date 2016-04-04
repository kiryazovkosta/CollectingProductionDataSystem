namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public partial class ProcessUnit : DeletableEntity, IEntity
    {
        private ICollection<ApplicationUserProcessUnit> applicationUserProcessUnits;
        private ICollection<ProductionPlanConfig> productionPlanConfigs;
        private ICollection<InProcessUnitData> inProcessUnitDatas;
        private ICollection<UnitMonthlyConfig> unitMonthlyConfigs;

        public ProcessUnit()
        {
            this.applicationUserProcessUnits = new HashSet<ApplicationUserProcessUnit>();
            this.UnitsConfigs = new HashSet<UnitConfig>();
            this.UnitsDailyConfigs = new HashSet<UnitDailyConfig>();
            this.productionPlanConfigs = new HashSet<ProductionPlanConfig>();
            this.inProcessUnitDatas = new HashSet<InProcessUnitData>();
            this.unitMonthlyConfigs = new HashSet<UnitMonthlyConfig>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public bool HasDailyStatistics { get; set; }
        public bool HasLoadStatistics { get; set; }
        public int FactoryId { get; set; }
        public virtual Factory Factory { get; set; }
        public virtual ICollection<UnitConfig> UnitsConfigs { get; set; }
        public virtual ICollection<UnitDailyConfig> UnitsDailyConfigs { get; set; }

        public string SortableName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(this.Id.ToString("d2"));
                sb.Append(" ");
                sb.Append(this.ShortName);
                return sb.ToString();
            }
        }

        public virtual ICollection<ApplicationUserProcessUnit> ApplicationUserProcessUnits
        {
            get { return applicationUserProcessUnits; }
            set { this.applicationUserProcessUnits = value; }
        }
        public virtual ICollection<ProductionPlanConfig> ProductionPlanConfigs
        {
            get { return this.productionPlanConfigs; }
            set { this.productionPlanConfigs = value; }
        }
        public virtual ICollection<InProcessUnitData> InProcessUnitDatas
        {
            get { return this.inProcessUnitDatas; }
            set { this.inProcessUnitDatas = value; }
        }
        public virtual ICollection<UnitMonthlyConfig> UnitMonthlyConfigs
        {
            get { return this.unitMonthlyConfigs; }
            set { this.unitMonthlyConfigs = value; }
        }
    }
}
