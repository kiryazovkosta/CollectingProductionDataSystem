namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Transactions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class ActiveTransactionsDataMap : EntityTypeConfiguration<ActiveTransactionsData>
    {
        public ActiveTransactionsDataMap()
        { 
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("ActiveTransactionsDatas");

            this.HasRequired(t => t.MeasuringPointConfig)
                .WithMany(t => t.ActiveTransactionsDatas)
                .HasForeignKey(t => t.MeasuringPointConfigId);
        }
    }
}
