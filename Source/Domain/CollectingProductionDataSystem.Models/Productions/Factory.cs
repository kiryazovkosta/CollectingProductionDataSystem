using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using System.Text;

namespace CollectingProductionDataSystem.Models.Productions
{
    public partial class Factory: DeletableEntity, IEntity
    {
        public Factory()
        {
            this.ProcessUnits = new HashSet<ProcessUnit>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public Nullable<int> PlantId { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual ICollection<ProcessUnit> ProcessUnits { get; set; }

        public string FactorySortableName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(this.Id.ToString("d2"));
                sb.Append(" ");
                sb.Append(this.ShortName);
                return sb.ToString();
            }
        }
    }
}
