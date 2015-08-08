namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProcessUnit : EntityBase
    {
        private ICollection<Unit> units;

        public ProcessUnit()
        {
            this.units = new HashSet<Unit>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        public int FactoryId { get; set; }

        public virtual Factory Factory { get; set; }

        public virtual ICollection<Unit> Units
        {
            get { return this.units; }
            set { this.units = value; }
        }
    }
}
