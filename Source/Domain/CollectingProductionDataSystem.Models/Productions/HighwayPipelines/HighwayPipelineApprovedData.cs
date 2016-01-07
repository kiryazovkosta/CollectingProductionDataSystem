namespace CollectingProductionDataSystem.Models.Productions.HighwayPipelines
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class HighwayPipelineApprovedData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public bool Approved { get; set; }
    }
}
