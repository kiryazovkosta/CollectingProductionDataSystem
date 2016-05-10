namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Contracts;

    public class ProductionPlanConfigUnitMonthlyConfigPlanMembers: ProductionPlanConfigUnitMonthlyConfigAbstract, IEntity
    {
        [NotMapped]
        public int Id { get; set; }
    }
}
