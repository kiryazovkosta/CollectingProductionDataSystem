using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class MeasurementPointsProductsConfig
    {
        public MeasurementPointsProductsConfig()
        {
            this.MeasurementPointsProductsDatas = new List<MeasurementPointsProductsData>();
        }

        public int Id { get; set; }
        public string PhdTotalCounterTag { get; set; }
        public int ProductId { get; set; }
        public int Code { get; set; }
        public int MeasurementPointId { get; set; }
        public int DirectionId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual MeasurementPoint MeasurementPoint { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<MeasurementPointsProductsData> MeasurementPointsProductsDatas { get; set; }
    }
}
