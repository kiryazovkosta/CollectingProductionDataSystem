namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class UnitsData : AuditInfo, IApprovableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int UnitConfigId { get; set; }
        public ShiftType ShiftId { get; set; }
        public decimal? Value { get; set; }
        public bool IsApproved { get; set; }
        public virtual UnitConfig UnitConfig { get; set; }
        public virtual UnitsManualData UnitsManualData { get; set; }

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
    }
}
