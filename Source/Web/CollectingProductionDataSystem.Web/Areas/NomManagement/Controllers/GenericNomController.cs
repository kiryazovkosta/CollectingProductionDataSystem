using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Abstract;
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
            IEnumerable<TModel> records = new List<TModel>();
            if (User.IsInRole("Administrator") && typeof(IDeletableEntity).IsAssignableFrom(typeof(TModel)))
            {
                records = this.repository.AllWithDeleted().ToList();
            }
            else
            {
                records = this.repository.All().ToList();
            }
            try
            {
                DataSourceResult result = records.ToDataSourceResult(request, ModelState, Mapper.Map<TView>);
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
                try
                {
                    //var entity = Mapper.Map<TModel>(inputViewModel);
                    // this way we create proxy object associated with database which can
                    // update navigation properties during change of the foreign keys
                    var entity = this.data.DbContext.DbContext.Set<TModel>().Create();

                    Mapper.Map(inputViewModel, entity);

                    this.repository.Add(entity);

                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }

                    inputViewModel.Id = entity.Id;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message + ex.StackTrace);
                }
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