namespace CollectingProductionDataSystem.GetTransactions
{
    using System.Globalization;
    using log4net;
    using System;
    using System.Linq;
    using System.ServiceProcess;
    using System.Threading;

    public partial class GetTransactions : ServiceBase
    {
        private Timer transactionsDataTimer = null;
        private Timer scaleDateTimer = null;
        private Timer activeTransactionsTimer = null;

        private static readonly object lockObjectTransactionsData = new object();
        private static readonly object lockObjectScaleData = new object();
        private static readonly object lockObjectActiveTransactionsData = new object();

        private readonly ILog logger;

        public GetTransactions(ILog loggerParam)
        {
            InitializeComponent();
            this.logger = loggerParam;
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Service is started!");
            try
            {
                if (Properties.Settings.Default.SYNC_TRANSACTIONS)
                {
                    this.transactionsDataTimer = new Timer(TimerHandlerTransactionsData, null, 0, Timeout.Infinite);
                }

                //if (Properties.Settings.Default.SYNC_SCALE)
                //{
                //    this.scaleDateTimer = new Timer(TimerHandlerScalesData, null, 0, Timeout.Infinite);
                //}

                if (Properties.Settings.Default.SYNC_ACTIVE_TRANSACTIONS)
                {
                    this.activeTransactionsTimer = new Timer(TimerHandlerActiveTransactionsData, null, 0, Timeout.Infinite);
                }

            }
            catch (Exception ex)
            {
                logger.Info(ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                logger.Info("Service is stopped!");
            }
            catch (Exception ex)
            {
                logger.Info(ex);
            }
        }

        private void TimerHandlerTransactionsData(object state)
        {
            lock (lockObjectTransactionsData)
            {
                try
                {
                    SetRegionalSettings();
                    this.transactionsDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    GetTransactionsMain.ProcessTransactionsData();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    this.transactionsDataTimer.Change(
                        Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_TRANSDACTION_DATA.TotalMilliseconds), 
                        System.Threading.Timeout.Infinite);
                }
            }
        }

        private void TimerHandlerScalesData(object state)
        {
            lock (lockObjectScaleData)
            {
                try
                {
                    SetRegionalSettings();
                    this.scaleDateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    GetTransactionsMain.ProcessScalesData();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    this.scaleDateTimer.Change(
                        Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_SCALE_DATA.TotalMilliseconds), 
                        System.Threading.Timeout.Infinite);
                }
            }
        }

        private void TimerHandlerActiveTransactionsData(object state)
        {
            lock (lockObjectActiveTransactionsData)
            {
                try
                {
                    SetRegionalSettings();
                    this.activeTransactionsTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    GetTransactionsMain.ProcessActiveTransactionsData();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    this.activeTransactionsTimer.Change(
                        Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_ACTIVE_TRANSACTIONS_DATA.TotalMilliseconds), 
                        System.Threading.Timeout.Infinite);
                }
            }
        }

        public static void SetRegionalSettings()
        {
	        if (Properties.Settings.Default.FORCE_REGIONAL_SETTINGS) 
            {
		        CultureInfo newCi = new CultureInfo(Properties.Settings.Default.CULTURE_INFO, false);
		        newCi.NumberFormat.CurrencyDecimalSeparator = ".";
		        newCi.NumberFormat.CurrencyGroupSeparator = string.Empty;
		        newCi.NumberFormat.NumberDecimalSeparator = ".";
		        newCi.NumberFormat.NumberGroupSeparator = string.Empty;
		        newCi.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
		        newCi.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
		        newCi.DateTimeFormat.LongTimePattern = "HH:mm:ss";
		        newCi.DateTimeFormat.LongDatePattern = "dd.MM.yyyy";
		        System.Threading.Thread.CurrentThread.CurrentCulture = newCi;
	        }
        }
    }
}
