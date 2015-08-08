namespace CollectingProductionDataSystem.Models
{
    using CollectingProductionDataSystem.Common.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Plant : IAccessRoles, IActiveEntity
    {
        private ICollection<Factory> factories;

        public Plant()
        {
            this.factories = new HashSet<Factory>();
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

        public virtual ICollection<Factory> Factories
        {
            get { return this.factories; }
            set { this.factories = value; }
        }
    }
}
