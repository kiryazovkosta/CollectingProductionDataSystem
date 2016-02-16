namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class ProductType : DeletableEntity, IEntity
    {
        public ProductType()
        {
            this.Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
