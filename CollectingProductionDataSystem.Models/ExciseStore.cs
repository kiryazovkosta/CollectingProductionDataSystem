namespace CollectingProductionDataSystem.Models
{
    using CollectingProductionDataSystem.Common;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExciseStore : IAccessRoles, IActiveEntity
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

        [StringLength(50)]
        public string FullAccessRole { get; set; }

        [StringLength(50)]
        public string ReadOnlyRole { get; set; }

        [Index]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<InventoryPark> InventoryParks
        {
            get { return this.inventoryParks; }
            set { this.inventoryParks = value; }
        }
    }
}
