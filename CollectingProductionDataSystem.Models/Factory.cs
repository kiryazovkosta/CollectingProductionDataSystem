namespace CollectingProductionDataSystem.Models
{
    using CollectingProductionDataSystem.Common;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Factory : IAccessRoles, IActiveEntity
    {
        private ICollection<ProcessUnit> processUnits;

        public Factory()
        {
            this.processUnits = new HashSet<ProcessUnit>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string FullAccessRole { get; set; }

        [StringLength(50)]
        public string ReadOnlyRole { get; set; }

        [Index]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<ProcessUnit> ProcessUnits
        {
            get { return this.processUnits; }
            set { this.processUnits = value; }
        }
    }
}
