//-----------------------------------------------------------------------
// <copyright file="ModelBingConfig.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------
namespace HistoricalProductionStructure.DataAccess.Configuration
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Mappings;

    public static class ModelBingConfig
    {
        internal static void RegisterMappings(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Add(new GlobalTypeMappingConfig());
            modelBuilder.Configurations.Add(new AuditLogRecordMap());
            modelBuilder.Configurations.Add(new FactoryMap());
            modelBuilder.Configurations.Add(new PlantMap());
            modelBuilder.Configurations.Add(new ProcessUnitMap());
            modelBuilder.Configurations.Add(new ProcessUnitToFactoryHistoryMap());
        }
    }
}
