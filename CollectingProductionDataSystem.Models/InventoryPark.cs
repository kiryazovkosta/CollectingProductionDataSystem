namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class InventoryPark : EntityBase
    {
        private ICollection<InventoryTank> inventoryTanks;

        public InventoryPark()
        {
            this.inventoryTanks = new HashSet<InventoryTank>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        public int AreaId { get; set; }

        public int ExciseStoreId { get; set; }

        public virtual Area Area { get; set; }

        public virtual ExciseStore ExciseStore { get; set; }

        public virtual ICollection<InventoryTank> InventoryTanks 
        {
            get { return this.inventoryTanks; }
            set { this.inventoryTanks = value; } 
        }
    }
}
