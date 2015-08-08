namespace CollectingProductionDataSystem.Models
{
    using CollectingProductionDataSystem.Common.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MaterialType : IActiveEntity
    {
        private ICollection<Unit> units;

        public MaterialType()
        {
            this.units = new HashSet<Unit>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [Index]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<Unit> Units
        {
            get { return this.units; }
            set { this.units = value; }
        }
    }
}
