using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Models.Productions
{
    public partial class UnitsInspectionData: DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public System.DateTime RecordTimestamp { get; set; }
        public decimal Value { get; set; }
        public Nullable<System.DateTime> LastUpdateTimestamp { get; set; }
        public virtual UnitConfig Unit { get; set; }
    }
}
