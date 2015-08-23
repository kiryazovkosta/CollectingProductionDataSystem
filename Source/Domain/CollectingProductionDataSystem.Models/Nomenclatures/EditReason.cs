namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Inventories;

    public class EditReason : DeletableEntity, IEntity
    {
        public EditReason()
        {
            this.UnitsDatas = new HashSet<UnitsData>();
            this.TankDatas = new HashSet<TankData>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UnitsData> UnitsDatas { get; set; }
        public virtual ICollection<TankData> TankDatas { get; set; }
    }
}
