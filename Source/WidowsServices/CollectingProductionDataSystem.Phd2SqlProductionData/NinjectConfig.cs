namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Log;
    using Ninject;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using log4net;

    public class NinjectConfig : IDisposable
    {
        private readonly IKernel kernel;
        public NinjectConfig() 
        {
            this.kernel = new Ninject.StandardKernel();
            kernel.Bind<DbContext>().To<CollectingDataSystemDbContext>();
            kernel.Bind(typeof(IDeletableEntityRepository<>)).To(typeof(DeletableEntityRepository<>));
            kernel.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>));
            kernel.Bind<IProductionData>().To<ProductionData>();
            kernel.Bind<IPersister>().To<AuditablePersister>();
            kernel.Bind<IEfStatus>().To<EfStatus>();
            kernel.Bind<ILogger>().To<Logger>();
            kernel.Bind<ILog>().ToMethod(context => LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData")).InSingletonScope();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.kernel.Dispose();
        }

        public IKernel Kernel
        {
            get
            {
                return this.kernel;
            }
        } 
    }
}
