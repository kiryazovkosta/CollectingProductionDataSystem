namespace CollectingProductionDataSystem.Models.Inventories
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class InnerPipelinesApprovedDate: DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public bool Approved { get; set; }
    }
}
