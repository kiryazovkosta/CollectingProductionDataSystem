using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.ViewModels.Transactions
{
    public class MeasuringPointsDataViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal AvtoQuantity { get; set; }

        public decimal JpQuantity { get; set; }

        public decimal SeaQuantity { get; set; }

        public decimal PipeQuantity { get; set; }

        public decimal TotalQuantity { get; set; }
    }
}