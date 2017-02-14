namespace Domain.Contracts
{
    public interface IApprovableEntity
    {
        bool IsApproved { get; set; }
    }
}
