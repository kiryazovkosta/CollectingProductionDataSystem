namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Identity;

    public partial class ProcessUnit : DeletableEntity, IEntity
    {
        private ICollection<ApplicationUser> users;
        private ICollection<ProductionPlanConfig> productionPlanConfigs;
        public ProcessUnit()
        {
            this.users = new HashSet<ApplicationUser>();
            this.UnitsConfigs = new HashSet<UnitConfig>();
            this.UnitsDailyConfigs = new HashSet<UnitsDailyConfig>();
            this.productionPlanConfigs = new HashSet<ProductionPlanConfig>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int FactoryId { get; set; }
        public virtual Factory Factory { get; set; }
        public virtual ICollection<UnitConfig> UnitsConfigs { get; set; }
        public virtual ICollection<UnitsDailyConfig> UnitsDailyConfigs { get; set; }
        public virtual ICollection<ApplicationUser> Users 
        {
            get { return users; }
            set { this.users = value; }
        }
        public virtual ICollection<ProductionPlanConfig> ProductionPlanConfigs
        {
            get { return this.productionPlanConfigs; }
            set { this.productionPlanConfigs = value; }
        }
    }
}
