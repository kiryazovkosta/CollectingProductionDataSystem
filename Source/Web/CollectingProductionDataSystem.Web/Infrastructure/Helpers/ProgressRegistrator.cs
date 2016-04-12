namespace CollectingProductionDataSystem.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Web.Hubs;

    public class ProgressRegistrator : IProgressRegistrator
    {
        /// <summary>
        /// Registers the progress.
        /// </summary>
        /// <param name="value">The value.</param>
        public void RegisterProgress(int value)
        {
            MessagesHub.JobStatus("#current-proccess-progress", value);
        }
    }
}