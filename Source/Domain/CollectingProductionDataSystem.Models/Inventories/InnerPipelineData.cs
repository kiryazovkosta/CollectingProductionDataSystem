namespace CollectingProductionDataSystem.Models.Inventories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public partial class InnerPipelineData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int ProductId { get; set; }
        public decimal Volume { get; set; }
        public decimal Mass { get; set; }
        public virtual Product Product { get; set; }
    }
}
