namespace CollectingProductionDataSystem.Models.Productions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using System;
    using CollectingProductionDataSystem.Models.Contracts;
    using System.Collections.Generic;

    public class ProductionPlanConfig : DeletableEntity, IEntity
    {
        private ICollection<UnitDailyConfig> unitsDailyConfigs;

        public ProductionPlanConfig()
        {
            this.unitsDailyConfigs = new HashSet<UnitDailyConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Percentages { get; set; }
        public string QuantityPlanFormula { get; set; }
        public string QuantityPlanMembers { get; set; }
        public string QuantityFactFormula { get; set; }
        public string QuantityFactMembers { get; set; }
        public int ProcessUnitId { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        //public int UnitsDailyConfigId { get; set; }
        //public virtual UnitsDailyConfig UnitsDailyConfigPlan { get; set; }
        public virtual ICollection<UnitDailyConfig> UnitsDailyConfigsFact 
        {
            get { return this.unitsDailyConfigs; }
            set { this.unitsDailyConfigs = value; } 
        }

    }
}