using System;

namespace CollectingProductionDataSystem.Infrastructure.Contracts
{
    public interface ILogger
    {
        void AuthenticationError(string msg, object eventSource, string userName);
        void Error(string message, object eventSource, Exception exception); 
    }
}