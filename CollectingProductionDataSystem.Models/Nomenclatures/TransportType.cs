namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class TransportType : DeletableEntity, IEntity
    {
        public TransportType()
        {
            this.MeasurementPoints = new HashSet<MeasurementPoint>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<MeasurementPoint> MeasurementPoints { get; set; }
    }
}
