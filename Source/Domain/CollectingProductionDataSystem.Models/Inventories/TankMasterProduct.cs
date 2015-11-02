namespace CollectingProductionDataSystem.Models.Inventories
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.ComponentModel.DataAnnotations;

    public partial class TankMasterProduct : DeletableEntity, IEntity
    {
        public int Id { get; set; }

        public int TankMasterProductCode { get; set; }

        public string Name { get; set; }

        public virtual Product Product { get; set; }
    }
}
