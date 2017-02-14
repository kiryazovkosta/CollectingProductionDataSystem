namespace Domain.Contracts
{

    public interface IAggregatable
    {
        string Code { get; set; }

        string AggregationMembers { get; set; }
    }
}