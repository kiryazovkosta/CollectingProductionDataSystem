namespace CollectingProductionDataSystem.Models
{
    public abstract class EntityBase
    {
        public bool IsActive { get; set; }

        public string ReadOnlyRole { get; set; }

        public string FullAccessRole { get; set; }
    }
}
