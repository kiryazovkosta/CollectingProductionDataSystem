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
        private const string deletableEntityInterfase = "CollectingProductionDataSystem.Models.Contracts.IDeletableEntity";

        public IEnumerable<AuditLogRecord> GetAddedEntityes(IEnumerable<DbEntityEntry> addedRecords, IDbSet<AuditLogRecord> auditRecords)
        {
            List<AuditLogRecord> changes = new List<AuditLogRecord>();

            foreach (var entry in addedRecords)
            {
                GetInfoForAdded(entry, auditRecords);
            }

            return changes;
        }

        /// <summary>
        /// Gets the info for added.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        private void GetInfoForAdded(DbEntityEntry entry, IDbSet<AuditLogRecord> auditRecords)
        {
            var entityName = entry.Entity.GetType().Name.Split('_')[0];

            auditRecords.Add(new AuditLogRecord()
            {
                TimeStamp = ((IAuditInfo)entry.Entity).CreatedOn,
                EntityName = entityName,
                EntityId = ((IEntity)entry.Entity).Id,
                FieldName = string.Empty,//propertyName,
                OperationType = EntityState.Added,
                OldValue = string.Empty,
                NewValue = string.Empty,//newValue,
                UserName = ((IAuditInfo)entry.Entity).CreatedFrom
            });
        }

        public IEnumerable<DbEntityEntry> PrepareSaveChanges(IAuditableDbContext data, string userName)
        {
            var addedRecords = ApplyAuditInfoRules(data, userName);
            ApplyDeletableEntityRules(data, userName);
            return addedRecords;
        }

        private IEnumerable<DbEntityEntry> ApplyAuditInfoRules(IAuditableDbContext data, string userName)
        {
            var addedRecords = new List<DbEntityEntry>();
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                data.DbContext.ChangeTracker
                    .Entries()
                    .Where(
                         e =>
                             e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                // Remove deleted entities
                if (entry.Entity.GetType().GetInterface(deletableEntityInterfase) != null
                    && (((IDeletableEntity)entry.Entity).IsDeleted == true))
                {
                    continue;
                }

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
                    GetChangedProperties(entry, userName, data.AuditLogRecords);
                    entity.ModifiedOn = DateTime.Now;
                    entity.ModifiedFrom = userName;
                }
            }

            return addedRecords;
        }

        private void GetChangedProperties(DbEntityEntry entry, string userName, IDbSet<AuditLogRecord> auditRecords)
        {
            var entityName = entry.Entity.GetType().Name.Split('_')[0];
            // List<AuditLogRecord> auditRecords = new List<AuditLogRecord>();

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
        }

        private void ApplyDeletableEntityRules(IAuditableDbContext data, string userName)
        {
            foreach (
                var entry in
                data.DbContext.ChangeTracker
                    .Entries()
                    .Where(e => (e.Entity is IDeletableEntity &&
                           ((e.State == EntityState.Modified) && (((IDeletableEntity)e.Entity)).IsDeleted == true))
                        || (e.Entity is IDeletableEntity && e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntity)entry.Entity;
                data.AuditLogRecords.Add(GetDeletedEntities(entry, userName));
                entity.DeletedFrom = userName;
                entry.State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Gets the deleted entities.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        private AuditLogRecord GetDeletedEntities(DbEntityEntry entry, string userName)
        {
            var entityName = entry.Entity.GetType().Name.Split('_')[0];

            return new AuditLogRecord()
             {
                 TimeStamp = DateTime.Now,
                 EntityName = entityName,
                 EntityId = ((IEntity)entry.Entity).Id,
                 FieldName = string.Empty,
                 OperationType = EntityState.Deleted,
                 OldValue = string.Empty,
                 NewValue = string.Empty,
                 UserName = userName
             };
        }
    }
}