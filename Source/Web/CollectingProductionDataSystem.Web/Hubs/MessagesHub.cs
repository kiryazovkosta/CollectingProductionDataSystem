namespace CollectingProductionDataSystem.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using Microsoft.AspNet.SignalR;

    public class MessagesHub : Hub
    {
        public static void DisplayNewMessage(Message message)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
            context.Clients.All.displayNewMessage(message);
        }

        public static void GetActualMessagesCount() 
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
            context.Clients.All.getMessagesCount();
        }

        public static void JobStatus(string selector, int value)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
            context.Clients.All.jobStatus(selector, value);
        }
    }
}