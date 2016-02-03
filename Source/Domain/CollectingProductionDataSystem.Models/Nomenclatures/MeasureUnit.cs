/// <summary>
/// 
/// </summary>
namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    /// <summary>
    /// 
    /// </summary>
    public partial class MeasureUnit : DeletableEntity, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureUnit"/> class.
        /// </summary>
        public MeasureUnit()
        {
            this.UnitsConfigs = new HashSet<UnitConfig>();
            this.UnitsDailyConfigs = new HashSet<UnitDailyConfig>();
            this.EnteredUnitConfigs = new HashSet<UnitConfig>();
            this.ProductionPlanConfigs = new HashSet<ProductionPlanConfig>();
            this.UnitsMonthlyConfigs = new HashSet<UnitMonthlyConfig>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<UnitConfig> UnitsConfigs { get; set; }

        public virtual ICollection<ProductionPlanConfig> ProductionPlanConfigs{ get; set; }
        /// <summary>
        /// Gets or sets the units daily configs.
        /// </summary>
        /// <value>
        /// The units daily configs.
        /// </value>
        public virtual ICollection<UnitDailyConfig> UnitsDailyConfigs { get; set; }

        public virtual ICollection<UnitConfig> EnteredUnitConfigs { get; set; }

        public virtual ICollection<UnitMonthlyConfig> UnitsMonthlyConfigs{get;set;}

    }
}
