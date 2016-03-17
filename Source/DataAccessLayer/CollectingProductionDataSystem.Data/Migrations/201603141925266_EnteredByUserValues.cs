namespace CollectingProductionDataSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnteredByUserValues : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.UnitEnteredForCalculationDatas", "Id", "dbo.UnitsDatas");
            //DropColumn("dbo.UnitEnteredForCalculationDatas", "UnitDataId");
            //RenameColumn(table: "dbo.UnitEnteredForCalculationDatas", name: "Id", newName: "UnitDataId");
            //RenameIndex(table: "dbo.UnitEnteredForCalculationDatas", name: "IX_Id", newName: "IX_UnitDataId");
            //DropPrimaryKey("dbo.UnitEnteredForCalculationDatas");
            //AddColumn("dbo.UnitEnteredForCalculationDatas", "Id", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.UnitEnteredForCalculationDatas", "Id");
            //AddForeignKey("dbo.UnitEnteredForCalculationDatas", "UnitDataId", "dbo.UnitsDatas", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            ////DropForeignKey("dbo.UnitEnteredForCalculationDatas", "UnitDataId", "dbo.UnitsDatas");
            ////DropPrimaryKey("dbo.UnitEnteredForCalculationDatas");
            ////AlterColumn("dbo.UnitEnteredForCalculationDatas", "Id", c => c.Int(nullable: false));
            ////AddPrimaryKey("dbo.UnitEnteredForCalculationDatas", "Id");
            ////RenameIndex(table: "dbo.UnitEnteredForCalculationDatas", name: "IX_UnitDataId", newName: "IX_Id");
            ////RenameColumn(table: "dbo.UnitEnteredForCalculationDatas", name: "UnitDataId", newName: "Id");
            ////AddColumn("dbo.UnitEnteredForCalculationDatas", "UnitDataId", c => c.Int(nullable: false));
            ////AddForeignKey("dbo.UnitEnteredForCalculationDatas", "Id", "dbo.UnitsDatas", "Id");
        }
    }
}
