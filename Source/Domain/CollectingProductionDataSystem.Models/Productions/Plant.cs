using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Identity;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Models.Productions
{
    public partial class Plant: DeletableEntity, IEntity
    {
        public Plant()
        {
            this.Factories = new HashSet<Factory>();
        }

        public int Id { get; set; }
        
        public string ShortName { get; set; }
        
        public string FullName { get; set; }

        public virtual ICollection<Factory> Factories { get; set; }

    }
}
