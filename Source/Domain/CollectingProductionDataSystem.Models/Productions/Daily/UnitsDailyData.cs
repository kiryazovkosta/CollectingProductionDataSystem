namespace CollectingProductionDataSystem.Models.Productions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;

    public partial class UnitsDailyData : AuditInfo, IApprovableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int UnitsDailyConfigId { get; set; }
        public decimal Value { get; set; }
        public bool IsApproved { get; set; }
        public bool HasManualData { get; set; }
        public virtual UnitDailyConfig UnitsDailyConfig { get; set; }
        public virtual UnitsManualDailyData UnitsManualDailyData { get; set; }
        public decimal TotalMonthQuantity { get; set; }

        [NotMapped]
        public bool IsManual 
        { 
            get
            {
                return this.UnitsManualDailyData != null;
            }
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

        [NotMapped]
        public bool GotManualData
        {
            get
            {
                return this.UnitsManualDailyData != null;
            }
        }

        [NotMapped]
        public double RealValueTillDay
        {
            get
            {
                return (double)this.TotalMonthQuantity + this.RealValue;
            }
        }
    }
}
