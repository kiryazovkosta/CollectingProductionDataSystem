namespace CollectingProductionDataSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnitsManualData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnitsManualDatas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                        DeletedFrom = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedFrom = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UnitsDatas", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UnitsManualDatas", "Id", "dbo.UnitsDatas");
            DropIndex("dbo.UnitsManualDatas", new[] { "Id" });
            DropTable("dbo.UnitsManualDatas");
        }
    }
}
