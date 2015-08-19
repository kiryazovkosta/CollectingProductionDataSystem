namespace CollectingProductionDataSystem.Models.Transactions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System.Collections.Generic;

    public partial class Ikunk : DeletableEntity, IEntity
    {
        public Ikunk()
        {
            this.Zones = new HashSet<Zone>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Zone> Zones { get; set; }
    }
}
