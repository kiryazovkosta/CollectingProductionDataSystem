using System;
using System.Web.Management;

namespace CollectingProductionDataSystem.Infrastructure.Log
{
    internal class CustomWebErrorEvent : WebErrorEvent
    {
        public CustomWebErrorEvent(string message, object eventSource, Exception exception)
            : base(message, eventSource,
            WebEventCodes.WebExtendedBase + WebEventCodes.RuntimeErrorWebResourceFailure, exception)
        {
        }
    }
}