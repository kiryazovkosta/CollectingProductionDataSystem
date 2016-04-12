namespace CollectingProductionDataSystem.Infrastructure.Log
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Management;

    class CustomWebInfoEvent : WebApplicationLifetimeEvent
    {
        public CustomWebInfoEvent(string msg, object eventSource, int eventCode)
            : base(msg, eventSource, eventCode)
        {

        }
    }
}
