using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    public partial class MaterialType: DeletableEntity, IEntity
    {
        public MaterialType()
        {
            this.Units = new HashSet<UnitConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UnitConfig> Units { get; set; }
    }
}
