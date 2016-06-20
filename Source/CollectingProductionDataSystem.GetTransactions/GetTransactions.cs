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
        private Timer tradeReportDateTimer = null;
        private Timer productionReportDateTimer = null;
        private Timer activeTransactionsTimer = null;
        private Timer reportTransactionsTimer = null;

        private static readonly object lockObjectTransactionsData = new object();
        private static readonly object lockObjectTradeReportData = new object();
        private static readonly object lockObjectProductionReportData = new object();
        private static readonly object lockObjectActiveTransactionsData = new object();
        private static readonly object lockObjectReportTransactionsData = new object();

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

                if (Properties.Settings.Default.SYNC_TRADE_REPORT_DATA)
                {
                    this.tradeReportDateTimer = new Timer(TimerHandlerTradeReportData, null, 0, Timeout.Infinite);
                }

                if (Properties.Settings.Default.SYNC_PRODUCT_REPORT_DATA)
                {
                    this.productionReportDateTimer = new Timer(TimerHandlerProductionReportData, null, 0, Timeout.Infinite);
                }

                if (Properties.Settings.Default.SYNC_ACTIVE_TRANSACTIONS)
                {
                    this.activeTransactionsTimer = new Timer(TimerHandlerActiveTransactionsData, null, 0, Timeout.Infinite);
                }

                if (Properties.Settings.Default.SYNC_TRANSACTIONS_BY_PRODUCTS)
                {
                    this.reportTransactionsTimer = new Timer(TimerHandlerReportTransactionsData, null, 0, Timeout.Infinite);  
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

        private void TimerHandlerTradeReportData(object state)
        {
            lock (lockObjectTradeReportData)
            {
                try
                {
                    SetRegionalSettings();
                    this.tradeReportDateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    GetTransactionsMain.ProcessTradeReportTransactionsData();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    this.tradeReportDateTimer.Change(
                        Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_TRADE_REPORT_DATA.TotalMilliseconds), 
                        System.Threading.Timeout.Infinite);
                }
            }
        }

        private void TimerHandlerProductionReportData(object state)
        {
            lock (lockObjectProductionReportData)
            {
                try
                {
                    SetRegionalSettings();
                    this.productionReportDateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    GetTransactionsMain.ProcessProductionReportTransactionsData();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    this.productionReportDateTimer.Change(
                        Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_PRODUCTION_REPORT_DATA.TotalMilliseconds), 
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
                    GetTransactionsMain.ProcessActiveTransactionsData(Properties.Settings.Default.ACTIVE_TRANS_OFFSET_IN_DAYS);
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

        private void TimerHandlerReportTransactionsData(object state)
        {
            lock (lockObjectReportTransactionsData)
            {
                try
                {
                    SetRegionalSettings();
                    this.reportTransactionsTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    GetTransactionsMain.ProcesReportTransactionsData(DateTime.Today.AddDays(Properties.Settings.Default.TRANSACTIONS_BY_PRODUCTS_OFFSET_IN_DAYS));
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    this.reportTransactionsTimer.Change(
                        Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_REPORT_TRANSACTIONS_DATA.TotalMilliseconds), 
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
