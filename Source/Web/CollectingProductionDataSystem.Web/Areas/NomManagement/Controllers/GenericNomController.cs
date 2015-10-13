using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public abstract class GenericNomController<TModel, TView> : AreaBaseController
        where TModel : class, IDeletableEntity, IEntity
        where TView : class, IEntity, new()
    {
        private readonly IDeletableEntityRepository<TModel> repository;
        public GenericNomController(IProductionData dataParam)
            : base(dataParam)
        {
            var method = this.data.GetType().GetMethod("GetDeletableEntityRepository", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo generic = method.MakeGenericMethod(typeof(TModel));
            this.repository = generic.Invoke(this.data, new object[] { }) as IDeletableEntityRepository<TModel>;
        }

        public virtual ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var processUnits = this.repository.All();
            try
            {
                DataSourceResult result = processUnits.ToDataSourceResult(request, Mapper.Map<TView>);
                return Json(result);

            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                DataSourceResult result = new object[] { null }.ToDataSourceResult(request, ModelState);
                return Json(result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create([DataSourceRequest]DataSourceRequest request, TView inputViewModel)
        {
            if (ModelState.IsValid)
            {

                var entity = Mapper.Map<TModel>(inputViewModel);

                this.repository.Add(entity);

                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }

                inputViewModel.Id = entity.Id;
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Update([DataSourceRequest]DataSourceRequest request, TView inputViewModel)
        {
            if (ModelState.IsValid)
            {

                var dbEntity = this.repository.GetById(inputViewModel.Id);

                if (dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, inputViewModel.Id));
                }
                else
                {
                    Mapper.Map(inputViewModel, dbEntity);

                    this.repository.Update(dbEntity);

                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, TView inputViewModel)
        {
            if (ModelState.IsValid)
            {
                this.repository.Delete(inputViewModel.Id);
                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult ValueMapper(int[] values)
        {
            var indices = new List<int>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var item in this.repository.All())
                {
                    if (values.Contains(item.Id))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices, JsonRequestBehavior.AllowGet);
        }
    }
}