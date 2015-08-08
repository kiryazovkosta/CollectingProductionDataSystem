namespace CollectingProductionDataSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UnitsInspectionData")]
    public class UnitsInspectionData
    {
        [Key, Column(Order = 0)]
        public int UnitId { get; set; }

        [Key, Column(Order = 1)]
        public DateTime RecordTimestamp { get; set; }

        public decimal Value { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastUpdateTimestamp { get; set; }

        public virtual Unit Unit { get; set; }
    }
}
