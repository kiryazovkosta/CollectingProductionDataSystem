namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Identity;

    public partial class ProcessUnit : DeletableEntity, IEntity
    {
        private ICollection<ApplicationUserProcessUnit> applicationUserProcessUnits;
        private ICollection<ProductionPlanConfig> productionPlanConfigs;
        public ProcessUnit()
        {
            this.applicationUserProcessUnits = new HashSet<ApplicationUserProcessUnit>();
            this.UnitsConfigs = new HashSet<UnitConfig>();
            this.UnitsDailyConfigs = new HashSet<UnitDailyConfig>();
            this.productionPlanConfigs = new HashSet<ProductionPlanConfig>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int FactoryId { get; set; }
        public virtual Factory Factory { get; set; }
        public virtual ICollection<UnitConfig> UnitsConfigs { get; set; }
        public virtual ICollection<UnitDailyConfig> UnitsDailyConfigs { get; set; }
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
    }
}
