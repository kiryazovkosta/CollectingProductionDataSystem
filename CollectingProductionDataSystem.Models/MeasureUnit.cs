namespace CollectingProductionDataSystem.Models
{
    using CollectingProductionDataSystem.Common.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MeasureUnit : IActiveEntity
    {
        private ICollection<Unit> units;

        public MeasureUnit()
        {
            this.units = new HashSet<Unit>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

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
