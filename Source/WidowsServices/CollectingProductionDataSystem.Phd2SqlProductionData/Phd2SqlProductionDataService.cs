namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Enumerations;
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

        private static volatile TreeState isFirstPhdInterfaceCompleted = TreeState.Null;

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
                isFirstPhdInterfaceCompleted = TreeState.Null;
                PrimaryDataSourceType dataSource = (PrimaryDataSourceType)Enum.ToObject(typeof(PrimaryDataSourceType), Properties.Settings.Default.PHD_DATA_SOURCE);
                isFirstPhdInterfaceCompleted = GetDataFromPhd(dataSource, isFirstPhdInterfaceCompleted, this.primaryDataTimer);
            }
        }

        private void TimerHandlerMeasurement(object state)
        {
            lock (lockObjectMeasurementData)
            {
                PrimaryDataSourceType dataSource = (PrimaryDataSourceType)Enum.ToObject(typeof(PrimaryDataSourceType), Properties.Settings.Default.PHD_DATA_SOURCE_SECOND);
                GetDataFromPhd(dataSource, isFirstPhdInterfaceCompleted, this.measurementDataTimer);
            }
        }

        /// <summary>
        /// Gets the data from PHD.
        /// </summary>
        /// <param name="srvVmMesPhdA">The SRV vm mes PHD A.</param>
        private TreeState GetDataFromPhd(PrimaryDataSourceType dataSourceParam, TreeState isFirstPhdInteraceCompleted, Timer timer)
        {
            bool lastOperationSucceeded = false;
            bool inTimeSlot = false;
            DateTime beginDateTime = DateTime.Now;
            var targetTime = DateTime.Now;
            try
            {
                Utility.SetRegionalSettings();
                this.primaryDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
                Shift targetShift = Phd2SqlProductionDataMain.GetTargetShiftByDateTime(targetTime);
                if (targetShift != null)
                {
                    inTimeSlot = true;
                    bool isForcedResultCalculation = CheckIfForcedCalculationNeeded(targetTime + lastTimeDuration, targetShift);
                    DateTime recordTimeStamp = GetTargetRecordTimestamp(targetTime, targetShift);
                    lastOperationSucceeded = Phd2SqlProductionDataMain.ProcessPrimaryProductionData(dataSourceParam, recordTimeStamp, targetShift, isForcedResultCalculation, isFirstPhdInteraceCompleted);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace, ex);
            }
            finally
            {
                DateTime endDateTime = DateTime.Now;
                lastTimeDuration = endDateTime - beginDateTime;
                TimeSpan nextStartDuration = GetNextTimeDuration(lastOperationSucceeded, inTimeSlot);
                timer.Change(Convert.ToInt64(nextStartDuration.TotalMilliseconds), Timeout.Infinite);
            }

            return lastOperationSucceeded ? TreeState.True : TreeState.False;
        }

        /// <summary>
        /// Gets the target record timestamp.
        /// </summary>
        /// <param name="targetTime">The target time.</param>
        /// <param name="targetShift">The target shift.</param>
        /// <returns></returns>
        private DateTime GetTargetRecordTimestamp(DateTime targetTime, Shift targetShift)
        {
            return (targetTime.Date + targetShift.BeginTime).Date;
        }

        /// <summary>
        /// Gets the duration of the next time.
        /// </summary>
        /// <param name="lastOperationSucceeded">The last operation succeeded.</param>
        /// <param name="inTimeSlot">In time slot for shift reading.</param>
        /// <returns></returns>
        public TimeSpan GetNextTimeDuration(bool lastOperationSucceeded, bool inTimeSlot)
        {
            if (!lastOperationSucceeded && inTimeSlot)
            {
                return TimeSpan.FromMinutes(1);
            }
            else
            {
                var time = DateTime.Now;
                var tens = (time.Minute / 10) + 1;

                if (tens < 6)
                {
                    time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 10 * tens, 0);
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
    }
}
