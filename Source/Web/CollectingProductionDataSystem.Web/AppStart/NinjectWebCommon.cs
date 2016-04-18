[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CollectingProductionDataSystem.Web.AppStart.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(CollectingProductionDataSystem.Web.AppStart.NinjectWebCommon), "Stop")]

namespace CollectingProductionDataSystem.Web.AppStart
{
    using System;
    using System.Web;
    using CollectingProductionDataSystem.Application.CalculatorService;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.FileServices;
    using CollectingProductionDataSystem.Application.HighwayPipelinesDataServices;
    using CollectingProductionDataSystem.Application.MailerService;
    using CollectingProductionDataSystem.Application.MonthlyServices;
    using CollectingProductionDataSystem.Application.PhdLogProxy;
    using CollectingProductionDataSystem.Application.ProductionDataServices;
    using CollectingProductionDataSystem.Application.TankDataServices;
    using CollectingProductionDataSystem.Application.UnitDailyDataServices;
    using CollectingProductionDataSystem.Application.UserServices;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Log;
    //using CollectingProductionDataSystem.PhdApplication.Contracts;
    //using CollectingProductionDataSystem.PhdApplication.PrimaryDataServices;
    using CollectingProductionDataSystem.Web.Infrastructure.Helpers;
    using CollectingProductionDataSystem.Web.Infrastructure.ModelBinders;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using System.Data.Entity;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Application.UnitsDataServices;
    using CollectingProductionDataSystem.Application.ProductionPlanDataServices;
    using CollectingProductionDataSystem.Application.ShiftServices;
    using log4net;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        internal static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<DbContext>().To<CollectingDataSystemDbContext>();
            kernel.Bind(typeof(IDeletableEntityRepository<>)).To(typeof(DeletableEntityRepository<>));
            kernel.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>));
            kernel.Bind<IProductionData>().To<ProductionData>().InRequestScope();
            kernel.Bind<IPersister>().To<AuditablePersister>();
            kernel.Bind<ITankDataKendoService>().To<TankDataKendoService>();
            kernel.Bind<IUnitsDataService>().To<UnitsDataService>();
            kernel.Bind<EditUserViewModelBinder>().To<EditUserViewModelBinder>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IEfStatus>().To<EfStatus>();
            kernel.Bind<IFileUploadService>().To<FileUploadService>().InRequestScope();
            kernel.Bind<ICalculatorService>().To<CalculatorService>();
            kernel.Bind<IUnitDailyDataService>().To<UnitDailyDataService>();
            kernel.Bind<IProductionDataCalculatorService>().To<ProductionDataCalculatorService>();
            kernel.Bind<CollectingProductionDataSystem.Infrastructure.Contracts.ILogger>().To<Logger>();
            kernel.Bind<IProductionPlanDataService>().To<ProductionPlanDataService>();
            kernel.Bind<IHighwayPipelinesDataService>().To<HighwayPipelinesDataService>();
            kernel.Bind<ITestUnitDailyCalculationService>().ToMethod(context => TestUnitDailyCalculationService.GetInstance()).InSingletonScope();
            kernel.Bind<ITestUnitMonthlyCalculationService>().ToMethod(context => TestUnitMonthlyCalculationService.GetInstance()).InSingletonScope();
            kernel.Bind<IPipelineServices>().To<PipelineServices>();
            kernel.Bind<IInventoryTanksService>().To<InventoryTanksService>();
            kernel.Bind<IUnitMothlyDataService>().To<UnitMothlyDataService>();
            kernel.Bind<IShiftService>().To<ShiftService>();
            //kernel.Bind<ILog>().ToMethod(context => LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData")).InSingletonScope();
            kernel.Bind<IMailerService>().To<MailerService>();
            //kernel.Bind<IPhdPrimaryDataService>().To<PhdPrimaryDataService>();
            kernel.Bind<log4net.ILog>().To<LoggerToLogAdapter>();
            kernel.Bind<IProgressRegistrator>().To<ProgressRegistrator>();
        }        
    }
}
