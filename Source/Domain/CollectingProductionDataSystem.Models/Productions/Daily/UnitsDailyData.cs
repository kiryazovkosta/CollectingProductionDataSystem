namespace CollectingProductionDataSystem.Models.Productions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
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

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} | ", this.CreatedOn);
            sb.AppendFormat("{0} | ", this.Id.ToString().PadLeft(10, '0'));
            sb.AppendFormat("{0:10} | ", this.UnitsDailyConfig.Code);
            sb.AppendFormat("{0:50} | ", this.UnitsDailyConfig.Name);
            sb.AppendFormat("{0:0.00000} | ", this.Value);
            sb.AppendFormat("{0:0.00000} | ", this.UnitsManualDailyData.Value);
            sb.AppendFormat("{0:0.00000} | ", this.RealValue);
            sb.AppendFormat("{0} | ", this.TotalMonthQuantity);

            return sb.ToString();
        }
    }
}
