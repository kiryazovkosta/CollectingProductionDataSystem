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
using CollectingProductionDataSystem.Models.UtilityEntities;
using CollectingProductionDataSystem.Web.Areas.Administration.Controllers;
using CollectingProductionDataSystem.Web.Areas.RecordsHistory.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

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

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Read([DataSourceRequest]DataSourceRequest request, int? id, string entityName)
        {
            try
            {
                IEnumerable<AuditLogRecord> dbHistory = this.data.AuditLogRecords.All().Where(x => (id == null) || (x.EntityName == entityName && x.EntityId == id));
                if ((dbHistory.Count() == 0) && id.HasValue)
                {
                    dbHistory = GetCreationData(id.Value, entityName);
                }

                DataSourceResult result = dbHistory.ToDataSourceResult(request, ModelState, Mapper.Map<AuditLogRecordViewModel>);
                return Json(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                DataSourceResult result = new object[] { null }.ToDataSourceResult(request, ModelState);
                return Json(result);
            }
        }

        /// <summary>
        /// Gets the creation data.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        private IEnumerable<AuditLogRecord> GetCreationData(int id, string entityName)
        {
            var modelAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.GetName().Name == "CollectingProductionDataSystem.Models");
            if (modelAssembly == null)
            {
                return new List<AuditLogRecord>();
            }
            var type = modelAssembly.GetTypes().FirstOrDefault(x => x.Name == entityName);
            if (type == null)
            {
                return new List<AuditLogRecord>();
            }

            MethodInfo method = this.GetType().GetMethod("GetCreationFromRecord", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo generic = method.MakeGenericMethod(type);
            var result = generic.Invoke(this, new object[] { id }) as AuditLogRecord;
            if (result == null)
            {
                return new List<AuditLogRecord>();
            }
            return new List<AuditLogRecord> { result };
        }

        private AuditLogRecord GetCreationFromRecord<T>(int id)
            where T : class, IAuditInfo, IEntity
        {
            var result = this.data.DbContext.Set<T>().FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return null;
            }
            return new AuditLogRecord
            {
                EntityName = typeof(T).Name,
                OperationType = EntityState.Added,
                TimeStamp = result.CreatedOn,
                UserName = result.CreatedFrom
            };
        }
    }
}