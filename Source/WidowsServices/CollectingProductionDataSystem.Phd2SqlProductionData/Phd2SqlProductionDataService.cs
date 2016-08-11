namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Enumerations;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using Ninject;
    using log4net;
    using System;
    using System.ServiceProcess;
    using System.Threading;
    using CollectingProductionDataSystem.Application.Contracts;
    using Application.MonthlyTechnologicalDataServices;

    public partial class Phd2SqlProductionDataService : ServiceBase
    {
        private Timer primaryDataTimer = null;

        private Timer inventoryDataTimer = null;

        private static int lastTargetShiftId = 0;

        private TransactionOptions transantionOption;

        private static TimeSpan lastTimeDuration = TimeSpan.FromMinutes(0);

        private static readonly object lockObjectPrimaryData = new object();

        private static readonly object lockObjectInventoryData = new object();

        private static volatile TreeState isFirstPhdInterfaceCompleted = TreeState.Null;

        private readonly ILog logger;

        private readonly IKernel kernel;

        public Phd2SqlProductionDataService(ILog loggerParam)
        {
            InitializeComponent();
            this.kernel = NinjectConfig.GetInjector;
            this.logger = loggerParam;
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
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

            }
            catch (Exception ex)
            {
                logger.Info(ex);
                var mailer = kernel.Get<IMailerService>();
                mailer.SendMail(ex.Message + ex.StackTrace, "OnStart");
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
                using (var service = kernel.Get<IPhdPrimaryDataService>())
                {
                    isFirstPhdInterfaceCompleted = TreeState.Null;
                    PrimaryDataSourceType dataSource = (PrimaryDataSourceType)Enum.ToObject(typeof(PrimaryDataSourceType), Properties.Settings.Default.PHD_DATA_SOURCE);
                    isFirstPhdInterfaceCompleted = GetDataFromPhd(service, dataSource, isFirstPhdInterfaceCompleted, this.primaryDataTimer);
                    dataSource = (PrimaryDataSourceType)Enum.ToObject(typeof(PrimaryDataSourceType), Properties.Settings.Default.PHD_DATA_SOURCE_SECOND);
                    GetDataFromPhd(service, dataSource, isFirstPhdInterfaceCompleted, this.primaryDataTimer, true);
                }
            }
        }

        /// <summary>
        /// Gets the data from PHD.
        /// </summary>
        /// <param name="srvVmMesPhdA">The SRV vm mes PHD A.</param>
        private TreeState GetDataFromPhd(IPhdPrimaryDataService service, PrimaryDataSourceType dataSourceParam, TreeState isFirstPhdInteraceCompleted, Timer timer, bool last = false)
        {
            bool lastOperationSucceeded = false;
            bool inTimeSlot = false;
            DateTime beginDateTime = DateTime.Now;
            var targetTime = DateTime.Now;

            try
            {
                Utility.SetRegionalSettings();
                this.primaryDataTimer.Change(Timeout.Infinite, Timeout.Infinite);

                Shift targetShift = service.GetObservedShiftByDateTime(targetTime);

                int targetShiftId = targetShift != null ? targetShift.Id : 0;
                if (lastTargetShiftId != targetShiftId && lastTargetShiftId != 0)
                {
                    var shift = service.GetShiftById(lastTargetShiftId);
                    var targetLastShiftTime = GetTargetRecordTimestamp(DateTime.Now, shift);
                    service.FinalizeShiftObservation(targetLastShiftTime, shift);
                }

                lastTargetShiftId = targetShiftId;

                if (targetShift != null)
                {
                    inTimeSlot = true;
                    bool isForcedResultCalculation = CheckIfForcedCalculationNeeded(DateTime.Now + lastTimeDuration, targetShift);
                    DateTime recordTimeStamp = GetTargetRecordTimestamp(targetTime, targetShift);
                    lastOperationSucceeded = service.ProcessPrimaryProductionData(dataSourceParam, recordTimeStamp, targetShift, isForcedResultCalculation, isFirstPhdInteraceCompleted);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace, ex);
            }
            finally
            {
                if (last)
                {
                    DateTime endDateTime = DateTime.Now;
                    lastTimeDuration = endDateTime - beginDateTime;
                    TimeSpan nextStartDuration = GetNextTimeDuration(lastOperationSucceeded, inTimeSlot);
                    //logger.InfoFormat("Timer {0} for {1} is set to: {2}", timer.ToString(), "next GetDataFromPhd iteration", DateTime.Now + nextStartDuration);
                    timer.Change(Convert.ToInt64(nextStartDuration.TotalMilliseconds), Timeout.Infinite);
                }
            }

            return lastOperationSucceeded ? TreeState.True : TreeState.False;
        }

        /// <summary>
        /// Shows the shifts params.
        /// </summary>
        /// <param name="shifts">The shifts.</param>
        private void ShowShiftsParams(IEnumerable<Shift> shifts)
        {
            foreach (var shift in shifts)
            {
                this.logger.InfoFormat("{0}", shift.ToString());
            }
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
                var time = DateTime.Now.AddSeconds(10);
                var result = time - DateTime.Now;

                if (result.TotalSeconds <= 0)
                {
                    result = TimeSpan.FromSeconds(10);
                }

                return result;
            }
            else 
            {
                var time = DateTime.Now;
                time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0).AddMinutes(1);
                var result = time - DateTime.Now;
                if (result.TotalSeconds <= 0)
                {
                    result = TimeSpan.FromSeconds(1);
                }
                return result;
                //    var tens = ((time.Minute + 1) / 10) + 1;

                //    if (tens < 6)
                //    {
                //        time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 10 * tens, 0);
                //    }
                //    else
                //    {
                //        if (time.Hour + 1 > 23)
                //        {
                //            time = time.Date.AddDays(1);
                //        }
                //        else
                //        {
                //            time = new DateTime(time.Year, time.Month, time.Day, time.Hour + 1, 0, 0);
                //        }
                //    }

                //    var result = time - DateTime.Now;

                //    if (result.TotalMinutes <= 0)
                //    {
                //        result = TimeSpan.FromMinutes(10);
                //    }

                //    return result;
            }
            //else
            //{
            //   
            //}
        }

        /// <summary>
        /// Checks if forced calculation needed.
        /// </summary>
        /// <param name="targetTime">The target time.</param>
        /// <param name="shift">The shift.</param>
        /// <returns></returns>
        public bool CheckIfForcedCalculationNeeded(DateTime targetDateTime, Shift shift)
        {
            //var baseDate = (targetDateTime.Date + shift.BeginTime).Date;
            var lastPosibleTimeForOperation = targetDateTime.Date + shift.ReadOffset + shift.ReadPollTimeSlot - TimeSpan.FromMinutes(1.5);//baseDate + shift.EndTime + shift.ReadPollTimeSlot - TimeSpan.FromMinutes(1);

            if (targetDateTime >= lastPosibleTimeForOperation)
            {
                return true;
            }

            return false;
        }

        private void TimerHandlerInventory(object state)
        {
            lock (lockObjectInventoryData)
            {
                using (var service = kernel.Get<IPhdPrimaryDataService>())
                {
                    try
                    {
                        Utility.SetRegionalSettings();
                        this.inventoryDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        service.ProcessInventoryTanksData();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message, ex);
                    }
                    finally
                    {
                        TimeSpan nextStartDuration = GetNextTimeDuration(true, false);
                        this.inventoryDataTimer.Change(Convert.ToInt64(nextStartDuration.TotalMilliseconds),//Properties.Settings.Default.IDLE_TIMER_INVENTORY.TotalMilliseconds),
                            System.Threading.Timeout.Infinite);
                    }
                }
            }
        }
    }
}
