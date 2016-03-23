namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.ServiceProcess;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using Ninject;
    using log4net;

    static class Phd2SqlProductionDataMain
    {

        internal static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] 
            {
                new Phd2SqlProductionDataService()
            };
            ServiceBase.Run(servicesToRun);
        }

    }
}