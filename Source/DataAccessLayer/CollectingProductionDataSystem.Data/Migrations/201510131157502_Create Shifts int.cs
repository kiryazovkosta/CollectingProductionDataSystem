namespace CollectingProductionDataSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateShiftsint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "BeginTicks", c => c.Long(nullable: false));
            AddColumn("dbo.Shifts", "ReadOffsetTicks", c => c.Long(nullable: false));
            AddColumn("dbo.Shifts", "ReadPollTimeSlotTicks", c => c.Long(nullable: false));
            DropColumn("dbo.Shifts", "BeginTime");
            DropColumn("dbo.Shifts", "ReadOffset");
            DropColumn("dbo.Shifts", "ReadPollTimeSlot");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shifts", "ReadPollTimeSlot", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Shifts", "ReadOffset", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Shifts", "BeginTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Shifts", "ReadPollTimeSlotTicks");
            DropColumn("dbo.Shifts", "ReadOffsetTicks");
            DropColumn("dbo.Shifts", "BeginTicks");
        }
    }
}
