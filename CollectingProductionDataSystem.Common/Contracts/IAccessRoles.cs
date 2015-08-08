namespace CollectingProductionDataSystem.Common.Contracts
{
    public interface IAccessRoles
    {
        string FullAccessRole { get; set; }

        string ReadOnlyRole { get; set; }
    }
}
