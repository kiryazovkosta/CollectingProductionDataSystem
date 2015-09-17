namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public partial class UnitsData : AuditInfo, IApprovableEntity, IEntity
    {
        private ICollection<UnitsDailyData> unitsDailyData;
        public UnitsData() 
        {
            this.unitsDailyData = new HashSet<UnitsDailyData>();
        }
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int UnitConfigId { get; set; }
        public ShiftType ShiftId { get; set; }
        public decimal? Value { get; set; }
        public bool IsApproved { get; set; }
        public virtual UnitConfig UnitConfig { get; set; }
        public virtual UnitsManualData UnitsManualData { get; set; }
        public virtual ICollection<UnitsDailyData> UnitsDailyData 
        { 
            get { return this.unitsDailyData; }
            set { this.unitsDailyData = value; }
        } 

        public override string ToString()
        {
            if(this.UnitsManualData != null)
            {
                return this.UnitsManualData.Value.ToString();
            }

            if (this.Value.HasValue)
            {
               return this.Value.Value.ToString(); 
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
