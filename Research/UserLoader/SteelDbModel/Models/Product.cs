using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class Product
    {
        public Product()
        {
            this.MeasurementPointsProductsConfigs = new List<MeasurementPointsProductsConfig>();
            this.TankDatas = new List<TankData>();
            this.UnitsConfigs = new List<UnitsConfig>();
        }

        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public virtual ICollection<MeasurementPointsProductsConfig> MeasurementPointsProductsConfigs { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<TankData> TankDatas { get; set; }
        public virtual ICollection<UnitsConfig> UnitsConfigs { get; set; }
    }
}
