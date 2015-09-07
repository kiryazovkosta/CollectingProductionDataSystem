namespace CollectingProductionDataSystem.Web.Infrastructure.MadelBinders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.IdentityInfrastructure;
    using CollectingProductionDataSystem.Web.ViewModels.Identity;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Microsoft.AspNet.Identity.Owin;
    using Ninject;

    public class EditUserViewModelBinder : IModelBinder
    {

        private readonly IProductionData data;

        public EditUserViewModelBinder(IProductionData dataParam) 
        {
            this.data = dataParam;
        }

        /// <summary>
        /// Binds the model to a value by using the specified controller context
        /// and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>The bound value.</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            EditUserViewModel user = new EditUserViewModel();
            var props = typeof(EditUserViewModel).GetProperties();
            var form = controllerContext.HttpContext.Request.Form;
            var formKeys = form.AllKeys;
            var spetialKeys = new string[] { "UserRoles", "ProcessUnits", "Parks" };
            var normalProps = props.Where(x => !spetialKeys.Any(y => x.Name == y)).ToList();
            foreach (PropertyInfo prop in normalProps)
            {
                if (formKeys.Contains(prop.Name))
                {
                    if (form[prop.Name] != null)
                    {
                        prop.SetValue(user, form[prop.Name]);
                    }
                }
            }

            //IKernel kernel = controllerContext.HttpContext.GetService(typeof(IKernel)) as IKernel;
            //var data = kernel.Get(typeof(IProductionData));
            //var data = controllerContext.HttpContext.ApplicationInstance.Context.;
            ////add Roles to model 
            if (formKeys.Contains("UserRoles"))
            {
                var rolesIds = form["UserRoles"].Split(',').Select(x => int.Parse(x));	

                user.UserRoles = Mapper.Map<IEnumerable<RoleViewModel>>(data.Roles.All().Where(x => rolesIds.Any(y => y == x.Id)));
            }

            if (formKeys.Contains("ProcessUnits"))
            {
                var processUnitIds = form["ProcessUnits"].Split(',').Select(x => int.Parse(x));
                user.ProcessUnits = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(data.ProcessUnits.All().Where(x => processUnitIds.Any(y => y == x.Id)));
            }

            if (formKeys.Contains("Parks"))
            {
                var parksIds = form["Parks"].Split(',').Select(x => int.Parse(x));
                user.Parks = Mapper.Map<IEnumerable<ParkViewModel>>(data.Parks.All().Where(x => parksIds.Any(y => y == x.Id)));
            }

            return user;
        }

    }
}