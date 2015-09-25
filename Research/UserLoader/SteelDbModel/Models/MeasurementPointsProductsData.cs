using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class MeasurementPointsProductsData
    {
        public int Id { get; set; }
        public System.DateTime RecordTimestamp { get; set; }
        public int MeasurementPointsProductsConfigId { get; set; }
        public Nullable<decimal> Value { get; set; }
        public bool IsApproved { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual MeasurementPointsProductsConfig MeasurementPointsProductsConfig { get; set; }
    }
}
