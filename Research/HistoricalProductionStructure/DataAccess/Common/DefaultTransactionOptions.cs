namespace HistoricalProductionStructure.DataAccess.Common
{
    using System;
    using System.Transactions;

    /// <summary>
    /// Summary description for DefaultTransactionOptions
    /// </summary>
    public sealed class DefaultTransactionOptions
    {
        private static volatile DefaultTransactionOptions instance;
        private static object syncRoot = new Object();
        private TransactionOptions transactionOptions;  

        private DefaultTransactionOptions() 
        {
            this.transactionOptions = new TransactionOptions();
            this.transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
            this.transactionOptions.Timeout = TransactionManager.MaximumTimeout;
        }

        public static DefaultTransactionOptions Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DefaultTransactionOptions();
                    }
                }

                return instance;
            }
        }

        public TransactionOptions TransactionOptions { get { return this.transactionOptions; } }
    }
    
}
