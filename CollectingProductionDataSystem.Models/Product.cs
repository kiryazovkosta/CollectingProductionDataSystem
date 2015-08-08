namespace CollectingProductionDataSystem.Models
{
    using CollectingProductionDataSystem.Common.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product : IActiveEntity
    {
        private ICollection<Unit> units;

        public Product()
        {
            this.units = new HashSet<Unit>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ProductType ProductType { get; set; }

        public virtual ICollection<Unit> Units
        {
            get { return this.units; }
            set { this.units = value; }
        }
    }
}
