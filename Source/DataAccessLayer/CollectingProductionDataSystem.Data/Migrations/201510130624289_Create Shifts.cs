namespace CollectingProductionDataSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateShifts : DbMigration
    {
        public override void Up()
        {
             CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BeginTime = c.Time(nullable: false, precision: 7),
                        ReadOffset = c.Time(nullable: false, precision: 7),
                        ReadPollTimeSlot = c.Time(nullable: false, precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                        DeletedFrom = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedFrom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
        }
    }
}
