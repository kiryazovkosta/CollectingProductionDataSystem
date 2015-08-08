namespace CollectingProductionDataSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public abstract class EntityBase
    {
        public bool IsActive { get; set; }

        [StringLength(50)]
        public string FullAccessRole { get; set; }

        [StringLength(50)]
        public string ReadOnlyRole { get; set; }
    }
}
