namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Factory : EntityBase
    {
        private ICollection<ProcessUnit> processUnits;

        public Factory()
        {
            this.processUnits = new HashSet<ProcessUnit>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        public virtual ICollection<ProcessUnit> ProcessUnits
        {
            get { return this.processUnits; }
            set { this.processUnits = value; }
        }
    }
}
