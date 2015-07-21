namespace CollectingProductionDataSystem.Models.Mec
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Factory : EntityBase
    {
        private ICollection<ProcessUnit> processUnits { get; set; }

        public Factory()
        {
            this.processUnits = new HashSet<ProcessUnit>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int PlantId { get; set; }

        public virtual Plant Plant { get; set; }

        public virtual ICollection<ProcessUnit> ProcessUnits
        {
            get { return this.processUnits; }
            set { this.processUnits = value; }
        }
    }
}
