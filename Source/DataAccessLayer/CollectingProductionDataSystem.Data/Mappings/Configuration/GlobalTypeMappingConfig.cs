/// <summary>
/// Summary description for GlobalTypeMappingConfig
/// </summary>
namespace CollectingProductionDataSystem.Data.Mappings.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GlobalTypeMappingConfig:Convention
    {
        public GlobalTypeMappingConfig() 
        {
            this.Properties<decimal>().Configure(p=>p.HasColumnType("decimal").HasPrecision(18,5));
        }
    }
}
