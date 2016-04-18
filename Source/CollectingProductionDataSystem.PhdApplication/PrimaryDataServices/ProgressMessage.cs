namespace CollectingProductionDataSystem.PhdApplication.PrimaryDataServices
{
    using CollectingProductionDataSystem.Infrastructure.Contracts;

    internal class ProgressMessage : IProgressMessage
    {
        public ProgressMessage( string message, int progressValue)
        {
            this.Message = message;
            this.ProgressValue = progressValue;
        }
        /// <summary>
        /// Gets or sets the progress value.
        /// </summary>
        /// <value>The progress value.</value>
        public int ProgressValue { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        public override string ToString()
        {
            return this.Message;
        }
    }
}