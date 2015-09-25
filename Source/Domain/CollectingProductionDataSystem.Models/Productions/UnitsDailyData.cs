namespace CollectingProductionDataSystem.Models.Productions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;

    public partial class UnitsDailyData : AuditInfo, IApprovableEntity, IEntity
    {
        private ICollection<UnitsData> unitsDatas;
        public UnitsDailyData()
        {
            this.unitsDatas = new HashSet<UnitsData>();
        }

        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int UnitsDailyConfigId { get; set; }
        public decimal Value { get; set; }
        public bool IsApproved { get; set; }
        public bool HasManualData { get; set; }
        public virtual UnitsDailyConfig UnitsDailyConfig { get; set; }
        public virtual UnitsManualDailyData UnitsManualDailyData { get; set; }
        public virtual ICollection<UnitsData> UnitsDatas 
        { 
            get { return this.unitsDatas; }
            set { this.unitsDatas = value;}
        }

        [NotMapped]
        public double RealValue 
        { 
            get
            {
                if (this.UnitsManualDailyData != null)
                {
                    return (double)this.UnitsManualDailyData.Value;
                }
                else
                {
                    return (double)this.Value;
                }
            }
        }
    }
}
