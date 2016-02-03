namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class UnitApprovedMonthlyData : DeletableEntity, IEntity
    {
        public int Id { get; set; }

        public DateTime RecordDate { get; set; }

        // Todo: Decide about Criteria

        public int MonthlyProductTypeId { get; set; }
        
        public virtual MonthlyProductType MonthlyProductType { get; set; }

        public bool Approved { get; set; }

    }
}