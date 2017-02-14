namespace HistoricalProductionStructure.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Transactions;
    using Common;
    using Concrete;
    using Configuration;
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using HistoricalProductionStructure.Common.Extentions;

    public class AppDbContext : DbContext, IAuditableDbContext
    {
        private readonly IPersister persister;
        private TransactionOptions transantionOption;

        static AppDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Migrations.Configuration>());
        }

        public AppDbContext()
            : this(new AuditablePersister())
        {
        }

        public AppDbContext(IPersister param)
            : base("AppConnectionString")
        {
            this.persister = param;
           
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
        }

        public AppDbContext(IPersister param, string connectionString)
            : base(connectionString)
        {
            this.persister = param;
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
        }

        public IDbSet<Plant> Plants { get; set; }

        public IDbSet<Factory> Factories { get; set; }

        public IDbSet<ProcessUnit> ProcessUnit { get; set; }

        public IDbSet<AuditLogRecord> AuditLogRecords { get; set; }

        public IDbSet<ProcessUnitToFactoryHistory> ProcessUnitToFactoryHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelBingConfig.RegisterMappings(modelBuilder);
            base.OnModelCreating(modelBuilder);
            //#if DEBUG
            this.Database.Log = (c) => { Debug.WriteLine(c); };
            //#endif
        }

        public override int SaveChanges()
        {
            return SaveChanges(null);
        }

        /// <summary>
        /// Gets or sets the db context.
        /// </summary>
        /// <value>The db context.</value>
        public DbContext DbContext
        {
            get
            {
                return this;
            }
        }

        public int SaveChanges(string userName)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
            {
                if (userName == null)
                {
                    // performed only if some MicroSoft API calls original SaveChanges
                    userName = "System Change";
                }

                var addedRecords = this.persister.PrepareSaveChanges(this, userName);
                int returnValue = 0;
                returnValue = base.SaveChanges();
                //Append added records with their Ids into audit log
                if (addedRecords != null && addedRecords.Any())
                {
                    this.persister.GetAddedEntityes(addedRecords, this.AuditLogRecords);
                    base.SaveChanges();
                }
            transaction.Complete();
            return returnValue;
        }
    }

        public IEfStatus SaveChangesWithValidation(string userName)
        {
            var result = new EfStatus();
            try
            {
                result.ResultRecordsCount = SaveChanges(userName); //then update it
            }
            catch (DbEntityValidationException ex)
            {
                //logger.Error(ex.Message, this, ex);
                return result.SetErrors(ex.EntityValidationErrors);
            }
            catch(DbUpdateException ex)
            {
                //logger.Error(ex.Message, this, ex);
                var sqlException = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                if ( (sqlException != null) && (sqlException.Number == 2601 || sqlException.Number == 2627))
                {
                    return result.SetErrors(new List<ValidationResult>{new ValidationResult( "Unic Constraint Violation")});
                }
                else
                {
                    return result.SetErrors(new List<ValidationResult> { new ValidationResult("Saving data error") });
                }
            }
            //else it isn't an exception we understand so it throws in the normal way

            return result;
        }

        public void BulkInsert<T>(IEnumerable<T> entities, string userName) where T : class
        {
            if (entities.FirstOrDefault() is IAuditInfo)
            {
                entities.ForEach(x =>
                {
                    ((IAuditInfo)x).CreatedFrom = userName;
                    ((IAuditInfo)x).CreatedOn = DateTime.Now;
                });
            }

            this.BulkInsert(entities);
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        protected override DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (typeof(System.ComponentModel.DataAnnotations.IValidatableObject).IsAssignableFrom(entityEntry.Entity.GetType()))
            {
                var type = entityEntry.Entity.GetType();
                if(IsProxy(type))
                {
                    type = entityEntry.Entity.GetType().BaseType;
                }

                MethodInfo method = this.GetType().GetMethod("GetNavigationProperties");
                MethodInfo generic = method.MakeGenericMethod(type);
                var navigationProperties = generic.Invoke(this, new object[] { entityEntry.Entity }) as List<PropertyInfo>;

                foreach (var property in navigationProperties)
                {
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        this.Entry(entityEntry.Entity).Collection(property.Name);
                    }
                    else
                    {
                        this.Entry(entityEntry.Entity).Reference(property.Name);
                    }
                }

                var result = new DbEntityValidationResult(entityEntry, ((IValidatableObject)entityEntry.Entity).Validate(new System.ComponentModel.DataAnnotations.ValidationContext(entityEntry.Entity)).Select(x => new DbValidationError(x.MemberNames.FirstOrDefault(), x.ErrorMessage)));
                if (!result.IsValid)
                {
                    return result;
                }
            }

            return base.ValidateEntity(entityEntry, items);
        }

        /// <summary>
        /// Determines whether the specified type is proxy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private bool IsProxy(Type type)
        {
            return type != null && System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(type) != type;
        }

        public List<PropertyInfo> GetNavigationProperties<T>(T entity) where T : class
        {
            var entityType = typeof(T);
            var elementType = ((IObjectContextAdapter)this).ObjectContext.CreateObjectSet<T>().EntitySet.ElementType;
            return elementType.NavigationProperties.Select(property => entityType.GetProperty(property.Name)).ToList();
        }
    }
}
