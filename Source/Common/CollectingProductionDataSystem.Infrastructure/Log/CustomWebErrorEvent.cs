using System;
using System.Collections.Generic;
using System.Web.Management;

namespace CollectingProductionDataSystem.Infrastructure.Log
{
    internal class CustomWebErrorEvent : WebErrorEvent
    {
        private readonly IEnumerable<string> customEventDetails;
        public CustomWebErrorEvent(string message, object eventSource, Exception exception, IEnumerable<string> customEventDetails = null)
            : base(message, eventSource,
            WebEventCodes.WebExtendedBase + WebEventCodes.RuntimeErrorUnhandledException, exception)
        {
            this.customEventDetails = customEventDetails ?? new List<string>();
        }

        public override void FormatCustomEventDetails(WebEventFormatter formatter)
        {
            foreach (var message in this.customEventDetails)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    formatter.AppendLine(message); 
                }
            }

            base.FormatCustomEventDetails(formatter);
        }
    }
}