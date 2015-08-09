namespace CollectingProductionDataSystem.Common
{
    public interface IAccessRoles
    {
        string FullAccessRole { get; set; }

        string ReadOnlyRole { get; set; }
    }
}
