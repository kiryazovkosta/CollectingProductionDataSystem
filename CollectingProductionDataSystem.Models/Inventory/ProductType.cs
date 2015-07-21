namespace CollectingProductionDataSystem.Models.Inventory
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductType
    {
        private ICollection<Product> products;

        public ProductType()
        {
            this.products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<Product> Products
        {
            get { return this.products; }
            set { this.products = value; }
        }
    }
}
