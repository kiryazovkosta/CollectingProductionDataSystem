namespace CollectingProductionDataSystem.Models.Inventory
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        public int Id { get; set; }

        public int ProductTypeId { get; set; }

        [Required]
        [MaxLength(150)]
        [Index(IsUnique=true)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ProductType ProductType { get; set; }

    }
}
