namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Common;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel;

    public class InventoryPark : IAccessRoles, IActiveEntity
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

        [StringLength(50)]
        public string FullAccessRole { get; set; }

        [StringLength(50)]
        public string ReadOnlyRole { get; set; }

        [Index]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual Area Area { get; set; }

        public virtual ExciseStore ExciseStore { get; set; }

        public virtual ICollection<InventoryTank> InventoryTanks 
        {
            get { return this.inventoryTanks; }
            set { this.inventoryTanks = value; } 
        }
    }
}
