namespace CollectingProductionDataSystem.Models.Transactions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class MeasurementPoint : DeletableEntity, IEntity
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
        public virtual TransportType TransportType { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<MeasurementPointsProductsConfig> MeasurementPointsProductsConfigs { get; set; }
    }
}
