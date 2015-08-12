using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Contracts;

namespace CollectingProductionDataSystem.Data.Concrete
{
    public class AuditablePersister:IPersister
    {
        public void PrepareSaveChanges(DbContext data, string userName)
        {
            ApplyAuditInfoRules(data, userName);
            ApplyDeletableEntityRules(data, userName);
        }

        public void PrepareSaveChanges(DbContext data)
        {
            PrepareSaveChanges(data, null);
        }

        
        private void ApplyAuditInfoRules(DbContext data, string userName)
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                data.ChangeTracker.Entries()
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
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                    entity.ModifiedFrom = userName;
                }

            }
        }

        private void ApplyDeletableEntityRules(DbContext data, string userName)
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (
                var entry in
                    data.ChangeTracker.Entries()
                        .Where(e => e.Entity is IDeletableEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entity.DeletedFrom = userName;
                entry.State = EntityState.Modified;
            }
        }
    }
}
