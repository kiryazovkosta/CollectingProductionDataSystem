namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.ServiceProcess;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using Ninject;
    using log4net;
    using log4net.Config;

    class Phd2SqlProductionDataMain
    {
        private static ILog logger;

        static Phd2SqlProductionDataMain()
        {
            XmlConfigurator.Configure(); 
            logger = LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData");
        }

        internal static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] 
            {
                new Phd2SqlProductionDataService(logger)
            };
            ServiceBase.Run(servicesToRun);
        }

    }
}