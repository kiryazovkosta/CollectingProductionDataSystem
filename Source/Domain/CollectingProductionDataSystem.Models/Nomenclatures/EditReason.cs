namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class EditReason : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
