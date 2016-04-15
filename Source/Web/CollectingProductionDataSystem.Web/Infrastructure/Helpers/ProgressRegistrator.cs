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
        private int progressCounter;
        /// <summary>
        /// Resets the progress.
        /// </summary>
        public void ResetProgress()
        {
            progressCounter = 0;
        }

        /// <summary>
        /// Registers the progress.
        /// </summary>
        /// <param name="value">The value.</param>
        public void RegisterProgress(int value)
        {
            progressCounter += value;
            MessagesHub.JobStatus("#current-proccess-progress", progressCounter);
        }
    }
}