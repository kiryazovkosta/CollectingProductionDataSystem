namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class MeasureUnit : EntityBase
    {
        private ICollection<Unit> units;

        public MeasureUnit()
        {
            this.units = new HashSet<Unit>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Unit> Units
        {
            get { return this.units; }
            set { this.units = value; }
        }
    }
}
