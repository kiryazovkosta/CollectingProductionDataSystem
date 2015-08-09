

namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

    public partial class Phd2SqlProductionDataService : ServiceBase
    {
        public Timer inspectionDataTimer = null;

        private static readonly object lockObjectInspectionPointsData = new object();

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
                if (Properties.Settings.Default.SYNC_INSPECTION_POINTS)
                {
                    this.inspectionDataTimer = new Timer(TimerHandlerTInspectionPoints, null, 0, Timeout.Infinite);
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

        private void TimerHandlerTInspectionPoints(object state)
        {
            lock (lockObjectInspectionPointsData)
            {
                try
                {
                    this.inspectionDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    Phd2SqlProductionDataMain.ProcessInformationLogsData();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    this.inspectionDataTimer.Change(Convert.ToInt64(Properties.Settings.Default.IDLE_TIMER_INSPECTION_POINTS.TotalMilliseconds), 
                        System.Threading.Timeout.Infinite);
                }
            }
        }
    }
}
