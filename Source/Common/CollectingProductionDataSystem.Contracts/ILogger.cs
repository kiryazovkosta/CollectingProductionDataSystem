namespace CollectingProductionDataSystem.Contracts
{
    public interface ILogger
    {
        void AuthenticationError(string msg, object eventSource, string userName);
    }
}