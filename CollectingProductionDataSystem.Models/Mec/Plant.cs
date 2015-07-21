namespace CollectingProductionDataSystem.Models.Mec
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Plant : EntityBase
    {
        private ICollection<Factory> factories;

        public Plant()
        {
            this.factories = new HashSet<Factory>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set;}

        public virtual ICollection<Factory> Factories
        {
            get { return this.factories; }
            set { this.factories = value; }
        }
    }
}
