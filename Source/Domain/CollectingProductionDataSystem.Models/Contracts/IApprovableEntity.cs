namespace CollectingProductionDataSystem.Models.Contracts
{
    public interface IApprovableEntity
    {
        bool IsApproved { get; set; }
    }
}
