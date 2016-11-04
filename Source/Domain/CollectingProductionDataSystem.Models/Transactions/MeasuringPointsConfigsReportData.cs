namespace CollectingProductionDataSystem.Models.Transactions
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class MeasuringPointsConfigsReportData : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal AvtoQuantity { get; set; }
        public decimal JpQuantity { get; set; }
        public decimal SeaQuantity { get; set; }
        public decimal PipeQuantity { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalMonthQuantity { get; set; }
        public decimal AvtoActiveQuantity { get; set; }
        public decimal JpActiveQuantity { get; set; }
        public decimal SeaActiveQuantity { get; set; }
        public decimal PipeActiveQuantity { get; set; }
        public decimal TotalActiveQuantity { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int Direction { get; set; }
    }
}
