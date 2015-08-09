namespace CollectingProductionDataSystem.Models
{
    using CollectingProductionDataSystem.Common;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProcessUnit : IAccessRoles, IActiveEntity
    {
        private ICollection<Unit> units;

        public ProcessUnit()
        {
            this.units = new HashSet<Unit>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        public int FactoryId { get; set; }

        [StringLength(50)]
        public string FullAccessRole { get; set; }

        [StringLength(50)]
        public string ReadOnlyRole { get; set; }

        [Index]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual Factory Factory { get; set; }

        public virtual ICollection<Unit> Units
        {
            get { return this.units; }
            set { this.units = value; }
        }
    }
}
