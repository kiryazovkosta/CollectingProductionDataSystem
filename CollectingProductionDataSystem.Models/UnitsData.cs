namespace CollectingProductionDataSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UnitsData")]
    public class UnitsData
    {
        [Key, Column(Order = 0)]
        public int UnitId { get; set; }

        [Key, Column(Order = 1)]
        public DateTime RecordTimestamp { get; set; }

        public decimal Value1 { get; set; }

        public decimal Value2 { get; set; }

        public decimal Value3 { get; set; }

        public decimal Value4 { get; set; }

        public decimal Value5 { get; set; }

        public bool IsFirstConfirmed { get; set; }

        public bool IsSecondConfirmed { get; set; }

        public virtual Unit Unit { get; set; }
    }
}
