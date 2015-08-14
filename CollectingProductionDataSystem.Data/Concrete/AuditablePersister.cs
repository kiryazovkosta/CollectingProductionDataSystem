namespace CollectingProductionDataSystem.Data.Concrete
{
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    public class AuditablePersister : IPersister
    {
        /// <summary>
        /// Gets the added entities.
        /// </summary>
        /// <param name="addedRecords">The added records.</param>
        /// <returns></returns>
        public IEnumerable<AuditLogRecord> GetAddedEntityes(List<DbEntityEntry> addedRecords)
        {
            List<AuditLogRecord> changes = new List<AuditLogRecord>();

            foreach (var entry in addedRecords)
            {
                changes.AddRange(GetInfoForAdded(entry));
            }

            return changes;
        }

        /// <summary>
        /// Gets the info for added.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        private IEnumerable<AuditLogRecord> GetInfoForAdded(DbEntityEntry entry)
        {
            var entityName = entry.Entity.GetType().Name;
            List<AuditLogRecord> auditRecords = new List<AuditLogRecord>();

            //var newValues = entry.CurrentValues.Clone();
            //var oldValues = entry.GetDatabaseValues();
            //foreach (string propertyName in entry.CurrentValues.PropertyNames)
            //{
            //    var property = entry.Property(propertyName);
            //    string newValue = (newValues.GetValue<object>(propertyName) ?? string.Empty).ToString();

            auditRecords.Add(new AuditLogRecord()
            {
                TimeStamp = ((IAuditInfo)entry.Entity).CreatedOn,
                EntityName = entityName,
                EntityId = ((IEntity)entry.Entity).Id,
                FieldName = string.Empty,//propertyName,
                OperationType = entry.State,
                OldValue = string.Empty,
                NewValue = string.Empty,//newValue,
                UserName = ((IAuditInfo)entry.Entity).CreatedFrom
            });
            //}
            return auditRecords;
        }

        public List<AuditLogRecord> PrepareSaveChanges(DbContext data, string userName, out List<DbEntityEntry> addedRecords)
        {
            var result = ApplyAuditInfoRules(data, userName, out addedRecords);
            result.AddRange(ApplyDeletableEntityRules(data, userName));
            return result;
        }

        private List<AuditLogRecord> ApplyAuditInfoRules(DbContext data, string userName, out List<DbEntityEntry> addedRecords)
        {
            List<AuditLogRecord> changes = new List<AuditLogRecord>();
            addedRecords = new List<DbEntityEntry>();
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                data.ChangeTracker
                    .Entries()
                    .Where(
                         e =>
                             e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                        entity.CreatedFrom = userName;
                        addedRecords.Add(entry);
                    }
                }
                else
                {
                    changes.AddRange(GetChangedProperties(entry, userName));
                    entity.ModifiedOn = DateTime.Now;
                    entity.ModifiedFrom = userName;
                }
            }

            return changes;
        }

        private List<AuditLogRecord> GetChangedProperties(DbEntityEntry entry, string userName)
        {
            var entityName = entry.Entity.GetType().Name;
            List<AuditLogRecord> auditRecords = new List<AuditLogRecord>();

            var newValues = entry.CurrentValues.Clone();
            var oldValues = entry.GetDatabaseValues();
            foreach (string propertyName in entry.CurrentValues.PropertyNames)
            {
                var property = entry.Property(propertyName);
                string newValue = (newValues.GetValue<object>(propertyName) ?? string.Empty).ToString();
                string oldValue = entry.State == EntityState.Modified ? (oldValues.GetValue<object>(propertyName) ?? string.Empty).ToString() : string.Empty;

                if (newValue != oldValue)
                {
                    auditRecords.Add(new AuditLogRecord()
                    {
                        TimeStamp = DateTime.Now,
                        EntityName = entityName,
                        EntityId = ((IEntity)entry.Entity).Id,
                        FieldName = propertyName,
                        OperationType = entry.State,
                        OldValue = oldValue,
                        NewValue = newValue,
                        UserName = userName
                    });
                }
            }

            // entry.CurrentValues.SetValues(newValues);
            return auditRecords;
        }

        private IEnumerable<AuditLogRecord> ApplyDeletableEntityRules(DbContext data, string userName)
        {
            List<AuditLogRecord> changes = new List<AuditLogRecord>();
            foreach (
                var entry in
                data.ChangeTracker
                    .Entries()
                    .Where(e => e.Entity is IDeletableEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntity)entry.Entity;
                changes.AddRange(GetDeletedEntities(entry, userName));
                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entity.DeletedFrom = userName;
                entry.State = EntityState.Modified;
            }

            return changes;
        }

        /// <summary>
        /// Gets the deleted entities.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        private IEnumerable<AuditLogRecord> GetDeletedEntities(DbEntityEntry entry, string userName)
        {
            var entityName = entry.Entity.GetType().Name;
            List<AuditLogRecord> auditRecords = new List<AuditLogRecord>();
            auditRecords.Add(new AuditLogRecord()
            {
                TimeStamp = DateTime.Now,
                EntityName = entityName,
                EntityId = ((IEntity)entry.Entity).Id,
                FieldName = string.Empty,
                OperationType = entry.State,
                OldValue = string.Empty,
                NewValue = string.Empty,
                UserName = userName
            });

            return auditRecords;
        }
    }
}