using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class MeasurementPoint
    {
        public MeasurementPoint()
        {
            this.MeasurementPointsProductsConfigs = new List<MeasurementPointsProductsConfig>();
        }

        public int Id { get; set; }
        public string ControlPoint { get; set; }
        public string Name { get; set; }
        public int TransportTypeId { get; set; }
        public int ZoneId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual TransportType TransportType { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<MeasurementPointsProductsConfig> MeasurementPointsProductsConfigs { get; set; }
    }
}
