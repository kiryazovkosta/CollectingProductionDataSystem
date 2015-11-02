namespace CollectingProductionDataSystem.Models.Contracts
{

    public interface IAggregatable
    {
        string Code { get; set; }

        string AggregationMembers { get; set; }
    }
}