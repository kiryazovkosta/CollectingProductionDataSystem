namespace CollectingProductionDataSystem.Models.Mec
{
    public class ProcessUnit : EntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FactoryId { get; set; }

        public virtual Factory Factory { get; set; } 
    }
}
