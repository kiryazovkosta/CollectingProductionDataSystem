namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Contracts;

    public abstract class ProductionPlanConfigUnitMonthlyConfigAbstract
    {
       
        public int ProductionPlanConfigId { get; set; }

        public int UnitMonthlyConfigId { get; set; }

        public virtual ProductionPlanConfig ProductionPlanConfig { get; set; }

        public virtual UnitMonthlyConfig UnitMonthlyConfig { get; set; }

        public int Position { get; set; }
    }
}
