namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;

    public class MaterialType
    {
        private ICollection<Unit> units;

        public MaterialType()
        {
            this.units = new HashSet<Unit>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Unit> Units
        {
            get { return this.units; }
            set { this.units = value; }
        }
    }
}
