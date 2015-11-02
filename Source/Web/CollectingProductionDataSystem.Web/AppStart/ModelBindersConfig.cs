namespace CollectingProductionDataSystem.Web.AppStart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.MadelBinders;
    using Ninject;

    public static class ModelBindersConfig
    {
        public static void RegisterModelBinders(ModelBinderDictionary binders, IDependencyResolver kernel) 
        {
            binders.Add(typeof(ShiftViewModel), kernel.GetService<TimeSpanModelBinder>());
        }
    }
}