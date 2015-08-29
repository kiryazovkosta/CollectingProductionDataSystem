using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Nomenclatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CollectingProductionDataSystem.Models.Productions
{
    public partial class UnitsManualDailyData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public virtual UnitsDailyData UnitsDailyData { get; set; }
        public int EditReasonId { get; set; }
        public virtual EditReason EditReason { get; set; }
    }
}
