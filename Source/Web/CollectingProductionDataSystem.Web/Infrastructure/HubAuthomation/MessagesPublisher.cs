using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Data.Concrete;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.UtilityEntities;
using CollectingProductionDataSystem.Web.Hubs;

namespace CollectingProductionDataSystem.Web.Infrastructure.HubAuthomation
{
    public class MessagesPublisher
    {
        private readonly IProductionData data ;
        private static object myLock = new object();
        private static volatile MessagesPublisher messagesPublisher = null;

        private MessagesPublisher(IProductionData dataParam)
        {
            this.data = dataParam;
            this.data.Messages.OnMessageUpdate += dependency_OnChange;
        }
      
        public static MessagesPublisher GetInstance(IProductionData dataParam)
        {
            if (messagesPublisher == null)
            { // 1st check
                lock (myLock)
                {// 2nd check
                    if (messagesPublisher == null)
                    {
                        messagesPublisher = new MessagesPublisher(dataParam);
                    }
                }
            }

            return messagesPublisher;
        }

        /// <summary>
        /// Handles the SavingChanges event of the MessagesPublisher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private static void dependency_OnChange(Message message)
        {
            MessagesHub.Show();
        }
    }
}