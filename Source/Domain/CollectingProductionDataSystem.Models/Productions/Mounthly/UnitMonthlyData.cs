namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class UnitMonthlyData : AuditInfo, IApprovableEntity, IEntity
    {
        public int Id { get; set; }

        public DateTime RecordTimestamp { get; set; }

        public int UnitMonthlyConfigId { get; set; }

        public decimal Value { get; set; }

        public decimal YearTotalValue { get; set; }

        public bool IsApproved { get; set; }

        public bool HasManualData { get; set; }

        public virtual UnitMonthlyConfig UnitMonthlyConfig { get; set; }

        public virtual UnitManualMonthlyData UnitManualMonthlyData { get; set; }

        public virtual UnitRecalculatedMonthlyData UnitRecalculatedMonthlyData { get; set; }

        //public decimal TotalMonthQuantity { get; set; }

        [NotMapped]
        public double RealValue
        {
            get
            {
                if (this.UnitManualMonthlyData != null)
                {
                    return (double)this.UnitManualMonthlyData.Value;
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
                return this.UnitManualMonthlyData != null;
            }
        }
    }
}