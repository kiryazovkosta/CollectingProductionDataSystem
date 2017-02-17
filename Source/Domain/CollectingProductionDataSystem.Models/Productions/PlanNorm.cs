namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class PlanNorm : DeletableEntity, IEntity, IConfigable
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        public int ProductionPlanConfigId { get; set; }
        public virtual ProductionPlanConfig ProductionPlanConfig { get; set; }
        public DateTime Month { get; set; }
        public decimal Value { get; set; }
        public IProcessUnitCangeable Config => this.ProductionPlanConfig;
    }
}
