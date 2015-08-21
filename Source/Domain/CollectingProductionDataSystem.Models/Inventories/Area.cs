namespace CollectingProductionDataSystem.Models.Inventories
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class Area: DeletableEntity, IEntity
    {
        private ICollection<Park> parks;
        public Area()
        {
            this.parks = new HashSet<Park>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Park> Parks 
        {
            get { return this.parks; }
            set { this.parks = value; }
        }
    }
}
