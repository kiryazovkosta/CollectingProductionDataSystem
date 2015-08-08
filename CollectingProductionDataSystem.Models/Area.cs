namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Area : EntityBase
    {
        private ICollection<InventoryPark> inventoryParks;

        public Area()
        {
            this.inventoryParks = new HashSet<InventoryPark>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<InventoryPark> InventoryParks
        {
            get { return this.inventoryParks; }
            set { this.inventoryParks = value; }
        }
    }
}
