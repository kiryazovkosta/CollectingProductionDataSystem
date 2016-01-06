
namespace CollectingProductionDataSystem.Infrastructure.Log
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Infrastructure.Contracts;

    public class Logger : ILogger
    {
        public void AuthenticationError(string msg, object eventSource, string userName)
        {
            var errEvent =
                new CustomWebAuthenticationFailureAuditEvent(msg, eventSource, userName);
            errEvent.Raise();
        }

        public void Error(string message, object eventSource, Exception exception, IEnumerable<string> customDetails = null)
        {
            var errEvent = new CustomWebErrorEvent(message, eventSource, exception, customDetails);
            errEvent.Raise();
        }
    }


}
