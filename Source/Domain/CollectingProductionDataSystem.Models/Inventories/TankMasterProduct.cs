namespace CollectingProductionDataSystem.Models.Inventories
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class TankMasterProduct : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public int TankMasterProductId { get; set; }
        public string Name { get; set; }
        public int ProductCode { get; set; }
    }
}
