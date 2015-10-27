using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Web.Areas.Administration.Controllers;
using CollectingProductionDataSystem.Web.Areas.RecordsHistory.ViewModels;

namespace CollectingProductionDataSystem.Web.Areas.RecordsHistory.Controllers
{
    public class HistoryController : AreaBaseController
    {
        public HistoryController(IProductionData dataParam) :
            base(dataParam)
        {
        }
        // GET: Administration/History
        public ActionResult Index(int id, string entityName)
        {
            TempData["recordId"] = id;
            TempData["entityName"] = entityName;

            var dbHistory = this.data.AuditLogRecords.All().Where(x => x.EntityName == entityName && x.EntityId == id).ToList();
            var model = Mapper.Map<IEnumerable<AuditLogRecordViewModel>>(dbHistory);
            if (model.Count() == 0)
            {
                model = GetCreationData(id, entityName);
            }

            return View(model);
        }

        /// <summary>
        /// Gets the creation data.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        private IEnumerable<AuditLogRecordViewModel> GetCreationData(int id, string entityName)
        {
            var modelAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.GetName().Name == "CollectingProductionDataSystem.Models");
            if (modelAssembly == null)
            {
                return new List<AuditLogRecordViewModel> ();
            }
            var type = modelAssembly.GetTypes().FirstOrDefault(x => x.Name == entityName);
            if (type == null)
            {
                return new List<AuditLogRecordViewModel> ();
            }

            MethodInfo method = this.GetType().GetMethod("GetCreationFromRecord", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo generic = method.MakeGenericMethod(type);
            var result = generic.Invoke(this, new object[] { id }) as AuditLogRecordViewModel;
            if (result ==null)
            {
                return new List<AuditLogRecordViewModel>();
            }
            return new List<AuditLogRecordViewModel> { result };
        }

        private AuditLogRecordViewModel GetCreationFromRecord<T>(int id)
            where T : class, IAuditInfo, IEntity
        {
            var result = this.data.DbContext.Set<T>().FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return null;
            }
            return new AuditLogRecordViewModel
            {
                EntityName = typeof(T).Name,
                OperationType = EntityState.Added,
                TimeStamp = result.CreatedOn,
                UserName = result.CreatedFrom
            };
        }
    }
}