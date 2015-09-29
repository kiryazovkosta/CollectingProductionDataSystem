

namespace CollectingProductionDataSystem.GetTransactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Data;
    using log4net;
    using CollectingProductionDataSystem.Data.Concrete;

    static class GetTransactionsMain
    {
        private static readonly ILog logger;

        static GetTransactionsMain()
        {
            logger = LogManager.GetLogger("CollectingProductionDataSystem.GetTransactions");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        internal static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] 
            { 
                new GetTransactions(logger)
            };
            ServiceBase.Run(servicesToRun);
        }

        internal static void ProcessTransactionData()
        {
            try
            {
                logger.Info("[1]");
                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    logger.Info("[2]");
                    var max = context.MaxAsoMeasuringPointDataSequenceNumberMap.All().FirstOrDefault();
                    if (max != null)
	                {
                        logger.Info("[3]");
                        logger.Info(max.MaxSequenceNumber);
                        var maxSequenceNumber = max.MaxSequenceNumber;
                        var adapter = new AsoDataSetTableAdapters.flow_MeasuringPointsDataTableAdapter();
                        var table = new AsoDataSet.flow_MeasuringPointsDataDataTable();
                        adapter.Fill(table, maxSequenceNumber);
                        foreach (AsoDataSet.flow_MeasuringPointsDataRow row in table.Rows)
                        {
                            logger.InfoFormat("{0}:{1}", row.SequenceNumber, row.ProductName);
                        }
	                }
                    else
                    {
                        logger.Info("[4]");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("[5]");
                logger.Error(ex.Message, ex);
            }
            
        }
    }
}
