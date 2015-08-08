namespace CollectingProductionDataSystem.Models
{
    using CollectingProductionDataSystem.Common.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Direction : IActiveEntity
    {
        private ICollection<Unit> units;

        public Direction()
        {
            this.units = new HashSet<Unit>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
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
