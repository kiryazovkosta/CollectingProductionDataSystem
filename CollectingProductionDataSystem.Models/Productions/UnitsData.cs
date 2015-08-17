using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Models.Productions
{
    public partial class UnitsData : AuditInfo, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int UnitConfigId { get; set; }
        public decimal? Value { get; set; }
        public virtual UnitConfig Unit { get; set; }
    }
}
