

namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
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

        private static TimeSpan lastTimeDuration = TimeSpan.FromMinutes(0);

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

                //if (Properties.Settings.Default.SYNC_PRIMARY_SECOND)
                //{
                //    this.measurementDataTimer = new Timer(TimerHandlerMeasurement, null, 0, Timeout.Infinite); 
                //}
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
            bool lastOperationSucceeded = false;
            var targetTime = DateTime.Now;

            lock (lockObjectPrimaryData)
            {
                DateTime beginDateTime = DateTime.Now;
                try
                {
                    Utility.SetRegionalSettings();
                    this.primaryDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    var dataSourceId = Properties.Settings.Default.PHD_DATA_SOURCE;
                    PrimaryDataSourceType dataSource = (PrimaryDataSourceType)Enum.ToObject(typeof(PrimaryDataSourceType), dataSourceId);
                    Shift targetShift = Phd2SqlProductionDataMain.GetTargetShiftByDateTime(targetTime);
                    if (targetShift != null)
                    {
                        bool isForcedResultCalculation = CheckIfForcedCalculationNeeded(targetTime + lastTimeDuration, targetShift);
                        lastOperationSucceeded = Phd2SqlProductionDataMain.ProcessPrimaryProductionData(dataSource, targetTime, targetShift, isForcedResultCalculation);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    DateTime endDateTime = DateTime.Now;
                    lastTimeDuration = endDateTime - beginDateTime;
                    var operationTimeout = Convert.ToInt64((endDateTime - beginDateTime).TotalMilliseconds);
                    var callbackTimeout = Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_PRIMARY.TotalMilliseconds);
                    TimeSpan duration = GetNextTimeDuration(lastOperationSucceeded);
                    this.primaryDataTimer.Change(callbackTimeout - operationTimeout, Timeout.Infinite);
                }
            }
        }

        /// <summary>
        /// Gets the duration of the next time.
        /// </summary>
        /// <param name="lastOperationSucceeded">The last operation succeeded.</param>
        /// <returns></returns>
        private TimeSpan GetNextTimeDuration(bool lastOperationSucceeded)
        {
            if (!lastOperationSucceeded)
            {
                return TimeSpan.FromMinutes(1);
            }
            else
            {
                var time = DateTime.Now + TimeSpan.FromMinutes(30);
                if (time.Minute < 30)
                {
                    time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 30, 0);
                }
                else
                {
                    time = new DateTime(time.Year, time.Month, time.Day, time.Hour + 1, 0, 0);
                }

                return time - DateTime.Now;
            }
        }

        /// <summary>
        /// Checks if forced calculation needed.
        /// </summary>
        /// <param name="targetTime">The target time.</param>
        /// <param name="shift">The shift.</param>
        /// <returns></returns>
        public bool CheckIfForcedCalculationNeeded(DateTime targetDateTime, Shift shift)
        {
            var baseDate = targetDateTime.Date;
            var lastPosibleTimeForOperation = baseDate + shift.ReadOffset + shift.ReadPollTimeSlot - TimeSpan.FromMinutes(1);

            if (lastPosibleTimeForOperation >= targetDateTime)
            {
                return true;
            }

            return false;
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

        //private void TimerHandlerMeasurement(object state)
        //{
        //    lock (lockObjectMeasurementData)
        //    {
        //        DateTime targetDateTime = DateTime.Now;
        //        try
        //        {
        //            Utility.SetRegionalSettings();
        //            this.measurementDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
        //            var dataSourceId = Properties.Settings.Default.PHD_DATA_SOURCE_SECOND;
        //            Shift targetShift = Phd2SqlProductionDataMain.GetTargetShiftByDateTime(targetDateTime);
        //            PrimaryDataSourceType dataSource = (PrimaryDataSourceType)Enum.ToObject(typeof(PrimaryDataSourceType), dataSourceId);
        //            bool isForcedResultCalculation = CheckIfForcedCalculationNeeded(targetDateTime, targetShift);
        //            Phd2SqlProductionDataMain.ProcessPrimaryProductionData(dataSource,targetDateTime, targetShift,isForcedResultCalculation);
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error(ex.Message, ex);
        //        }
        //        finally
        //        {
        //            DateTime endDateTime = DateTime.Now;
        //            var operationTimeout = Convert.ToInt64((endDateTime - targetDateTime).TotalMilliseconds);
        //            var callbackTimeout = Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_PRIMARY_SECOND.TotalMilliseconds);
        //            this.measurementDataTimer.Change(callbackTimeout - operationTimeout, Timeout.Infinite);
        //        }
        //    }
        //}
    }
}
