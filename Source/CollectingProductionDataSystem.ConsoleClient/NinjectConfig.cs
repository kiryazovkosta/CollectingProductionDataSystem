namespace CollectingProductionDataSystem.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Application.CalculatorService;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.FileServices;
    using CollectingProductionDataSystem.Application.MailerService;
    using CollectingProductionDataSystem.Application.TankDataServices;
    using CollectingProductionDataSystem.Application.UnitsDataServices;
    using CollectingProductionDataSystem.Application.UserServices;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Log;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using Moq;
    using Ninject;
    using log4net;
    using CollectingProductionDataSystem.PhdApplication.PrimaryDataServices;
    using Application.TransactionsDailyDataServices;

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
            var MailerMoq = new Mock<IMailerService>();
//          kernel = new Ninject.StandardKernel();
            kernel.Bind<DbContext>().To<CollectingDataSystemDbContext>();
            kernel.Bind(typeof(IDeletableEntityRepository<>)).To(typeof(DeletableEntityRepository<>));
            kernel.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>));
            kernel.Bind<IProductionData>().To<ProductionData>().InSingletonScope();
            kernel.Bind<IPersister>().To<AuditablePersister>();
            kernel.Bind<IEfStatus>().To<EfStatus>();
            kernel.Bind<ILogger>().To<Logger>();
            kernel.Bind<ILog>().ToMethod(context => LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData")).InSingletonScope();
            kernel.Bind<IMailerService>().ToMethod(context =>MailerMoq.Object);
            kernel.Bind<IPhdPrimaryDataService>().To<PhdPrimaryDataService>();
            kernel.Bind<ICalculatorService>().To<CalculatorService>();
            kernel.Bind<ITransactionsDailyDataService>().To<TransactionsDailyDataService>();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            kernel.Dispose();
        }
    }
}
