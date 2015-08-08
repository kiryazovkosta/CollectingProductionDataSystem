namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ExciseStore : EntityBase
    {
        private ICollection<InventoryPark> inventoryParks;

        public ExciseStore()
        {
            this.inventoryParks = new HashSet<InventoryPark>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<InventoryPark> InventoryParks
        {
            get { return this.inventoryParks; }
            set { this.inventoryParks = value; }
        }
    }
}
