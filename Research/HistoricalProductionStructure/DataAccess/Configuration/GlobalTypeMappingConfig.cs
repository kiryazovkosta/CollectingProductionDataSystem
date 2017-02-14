namespace HistoricalProductionStructure.DataAccess.Configuration
{
    using System.Data.Entity.ModelConfiguration.Conventions;

    /// <summary>
    /// Summary description for GlobalTypeMappingConfig
    /// </summary>
    public class GlobalTypeMappingConfig:Convention
    {
        public GlobalTypeMappingConfig() 
        {
            Properties<decimal>().Configure(p=>p.HasColumnType("decimal").HasPrecision(18,5));
        }
    }
}
