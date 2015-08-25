namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Inventories;

    public class EditReason : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
