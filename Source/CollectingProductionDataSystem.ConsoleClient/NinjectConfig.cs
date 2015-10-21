
namespace CollectingProductionDataSystem.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.FileServices;
    using CollectingProductionDataSystem.Application.TankDataServices;
    using CollectingProductionDataSystem.Application.UnitsDataServices;
    using CollectingProductionDataSystem.Application.UserServices;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Infrastructure.MadelBinders;
    using Ninject;

    public class NinjectConfig: IDisposable
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
            kernel.Bind<ITankDataKendoService>().To<TankDataKendoService>();
            kernel.Bind<IUnitsDataService>().To<UnitsDataService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IEfStatus>().To<EfStatus>();
            kernel.Bind<IFileUploadService>().To<FileUploadService>();
            
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
