namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Direction
    {
        private ICollection<Unit> units;

        public Direction()
        {
            this.units = new HashSet<Unit>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        public string Name { get; set; }

        public virtual ICollection<Unit> Units
        {
            get { return this.units; }
            set { this.units = value; }
        }
    }
}
