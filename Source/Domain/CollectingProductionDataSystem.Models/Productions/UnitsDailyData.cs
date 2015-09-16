namespace CollectingProductionDataSystem.Models.Productions
{
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;

    public partial class UnitsDailyData : AuditInfo, IApprovableEntity, IEntity
    {
        public UnitsDailyData()
        {
            this.UnitsDatas = new HashSet<UnitsData>();
        }

        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int UnitsDailyConfigId { get; set; }
        public decimal Value { get; set; }
        public bool IsApproved { get; set; }
        public bool HasManualData { get; set; }
        public virtual UnitsDailyConfig UnitsDailyConfig { get; set; }
        public virtual UnitsManualDailyData UnitsManualDailyData { get; set; }
        public virtual ICollection<UnitsData> UnitsDatas { get; set; }
    }
}
