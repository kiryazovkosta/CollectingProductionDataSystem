namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.MailerService;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Log;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using CollectingProductionDataSystem.PhdApplication.PrimaryDataServices;
    using Ninject;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using log4net;
    using Application.MonthlyTechnologicalDataServices;

    public class NinjectConfig : IDisposable
    {
        private static readonly object lockObj = new object();
        private static NinjectConfig injector;
        private static IKernel kernel;

        /// <summary>
        /// Prevents a default instance of the <see cref="NinjectInjector" /> class from being created.
        /// </summary>
        private NinjectConfig()
        {
            kernel = new StandardKernel();
            InitializeKernel(kernel);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            kernel.Dispose();
        }

        public static IKernel GetInjector
        {
            get
            {
                if (injector == null)
                {
                    lock (lockObj)
                    {
                        if (injector == null)
                        {
                            injector = new NinjectConfig();
                        }
                    }
                }

                return kernel;
            }
        }

        /// <summary>
        /// Initializes the kernel.
        /// </summary>
        public static void InitializeKernel(IKernel kernel)
        {
            kernel.Bind<DbContext>().To<CollectingDataSystemDbContext>();
            kernel.Bind(typeof(IDeletableEntityRepository<>)).To(typeof(DeletableEntityRepository<>));
            kernel.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>));
            kernel.Bind<IProductionData>().To<ProductionData>();
            kernel.Bind<IPersister>().To<AuditablePersister>();
            kernel.Bind<IEfStatus>().To<EfStatus>();
            kernel.Bind<ILogger>().To<Logger>();
            kernel.Bind<ILog>().ToMethod(context => LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData"));
            kernel.Bind<IMailerService>().To<MailerService>();
            kernel.Bind<IPhdPrimaryDataService>().To<PhdPrimaryDataService>();
        }
    }


}
