/// <summary>
/// Summary description for DefaultTransactionOptions
/// </summary>
namespace CollectingProductionDataSystem.Data.Common
{
    using System;
    using System.Linq;
    using System.Transactions;

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
