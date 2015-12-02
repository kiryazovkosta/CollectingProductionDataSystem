namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MathExpressionMap : EntityTypeConfiguration<MathExpression>
    {
        public MathExpressionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();

            this.Property(t => t.Expression)
                .HasMaxLength(1024)
                .IsRequired();

            this.Property(t => t.Name)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("MathExpressions");
        }
    }
}
