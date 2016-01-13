

namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using CollectingProductionDataSystem.Models.Productions;
    using log4net;
    using System;
    using System.ServiceProcess;
    using System.Threading;

    public partial class Phd2SqlProductionDataService : ServiceBase
    {
        private Timer primaryDataTimer = null;

        private Timer inventoryDataTimer = null;

        private Timer measurementDataTimer = null;

        private static readonly object lockObjectPrimaryData = new object();

        private static readonly object lockObjectInventoryData = new object();

        private static readonly object lockObjectMeasurementData = new object();

        private readonly ILog logger;

        public Phd2SqlProductionDataService(ILog loggerParam)
        {
            InitializeComponent();
            this.logger = loggerParam;
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Service is started!");
            try
            {
                if (Properties.Settings.Default.SYNC_PRIMARY)
                {
                    this.primaryDataTimer = new Timer(TimerHandlerPrimary, null, 0, Timeout.Infinite);
                }

                if (Properties.Settings.Default.SYNC_INVENTORY)
                {
                    this.inventoryDataTimer = new Timer(TimerHandlerInventory, null, 0, Timeout.Infinite);
                }

                if (Properties.Settings.Default.SYNC_PRIMARY_SECOND)
                {
                    this.measurementDataTimer = new Timer(TimerHandlerMeasurement, null, 0, Timeout.Infinite); 
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex);
            }
        }

        protected override void OnStop()
        {
            logger.Info("Service is stopped!");
            try
            {

            }
            catch (Exception ex)
            {
                logger.Info(ex);
            }
        }

        private void TimerHandlerPrimary(object state)
        {
            lock (lockObjectPrimaryData)
            {
                DateTime beginDateTime = DateTime.Now;
                try
                {
                    Utility.SetRegionalSettings();
                    this.primaryDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    var dataSourceId = Properties.Settings.Default.PHD_DATA_SOURCE;
                    PrimaryDataSourceType dataSource = (PrimaryDataSourceType)Enum.ToObject(typeof(PrimaryDataSourceType) , dataSourceId);
                    Phd2SqlProductionDataMain.ProcessPrimaryProductionData(dataSource);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    DateTime endDateTime = DateTime.Now;
                    var operationTimeout = Convert.ToInt64((endDateTime - beginDateTime).TotalMilliseconds);
                    var callbackTimeout = Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_PRIMARY.TotalMilliseconds);
                    this.primaryDataTimer.Change(callbackTimeout - operationTimeout, Timeout.Infinite);
                }
            }
        }

        private void TimerHandlerInventory(object state)
        {
            lock (lockObjectInventoryData)
            {
                try
                {
                    Utility.SetRegionalSettings();
                    this.inventoryDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    Phd2SqlProductionDataMain.ProcessInventoryTanksData();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    this.inventoryDataTimer.Change(Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_INVENTORY.TotalMilliseconds), 
                        System.Threading.Timeout.Infinite);
                }
            }
        }

        private void TimerHandlerMeasurement(object state)
        {
            lock (lockObjectMeasurementData)
            {
                DateTime beginDateTime = DateTime.Now;
                try
                {
                    Utility.SetRegionalSettings();
                    this.measurementDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    var dataSourceId = Properties.Settings.Default.PHD_DATA_SOURCE_SECOND;
                    PrimaryDataSourceType dataSource = (PrimaryDataSourceType)Enum.ToObject(typeof(PrimaryDataSourceType) , dataSourceId);
                    Phd2SqlProductionDataMain.ProcessPrimaryProductionData(dataSource);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    DateTime endDateTime = DateTime.Now;
                    var operationTimeout = Convert.ToInt64((endDateTime - beginDateTime).TotalMilliseconds);
                    var callbackTimeout = Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_PRIMARY_SECOND.TotalMilliseconds);
                    this.measurementDataTimer.Change(callbackTimeout - operationTimeout, Timeout.Infinite);
                }
            }
        }
    }
}
