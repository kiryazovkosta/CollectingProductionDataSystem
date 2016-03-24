namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class UnitsData : AuditInfo, IApprovableEntity, IEntity
    {
        private ICollection<UnitEnteredForCalculationData> unitEnteredForCalculationData;

        public UnitsData()
        {
            this.unitEnteredForCalculationData = new HashSet<UnitEnteredForCalculationData>();
        }
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int UnitConfigId { get; set; }
        public int ShiftId { get; set; }
        public decimal? Value { get; set; }
        public bool IsApproved { get; set; }
        public virtual UnitConfig UnitConfig { get; set; }
        public virtual UnitsManualData UnitsManualData { get; set; }
        public virtual ICollection<UnitEnteredForCalculationData> UnitEnteredForCalculationData 
        { 
            get { return this.unitEnteredForCalculationData; }
            set { this.unitEnteredForCalculationData = value; } 
        }
        public int Confidence { get; set; }

        [NotMapped]
        public bool IsManual 
        { 
            get
            {
                return this.UnitsManualData != null;
            }
        }

        [NotMapped]
        public double RealValue 
        { 
            get
            {
                if (this.UnitsManualData != null)
                {
                    return (double)this.UnitsManualData.Value;
                }
                else
                {
                    if (this.Value.HasValue)
                    {
                        return (double)this.Value.Value;
                    }
                    else
                    {
                        return default(double);
                    }
                }
            }
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

        public string Stringify()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} | ", this.CreatedOn);            
            sb.AppendFormat("{0} | ",this.Id.ToString().PadLeft(10,'0'));
            sb.AppendFormat("{0:10} | ", this.UnitConfig.Code);
            sb.AppendFormat("{0:50} | ", this.UnitConfig.Name);
            sb.AppendFormat("{0:0.00000} | ", this.Value ?? 0);
            sb.AppendFormat("{0} | ", ShiftId);
            sb.AppendFormat("{0} | ", this.UnitConfig.CollectingDataMechanism);
            sb.AppendFormat("{0} | ", this.UnitConfig.PreviousShiftTag);
            sb.AppendFormat("{0} | ", this.Confidence);
            return sb.ToString();
        }
    }
}
