namespace CollectingProductionDataSystem.Models.Transactions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class  Zone : DeletableEntity, IEntity
    {
        public Zone()
        {
            this.MeasurementPoints = new HashSet<MeasurementPoint>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int IkunkId { get; set; }
        public virtual Ikunk Ikunk { get; set; }
        public virtual ICollection<MeasurementPoint> MeasurementPoints { get; set; }
    }
}
