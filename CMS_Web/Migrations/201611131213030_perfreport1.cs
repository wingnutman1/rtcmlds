namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class perfreport1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettingsModel", "staffShiftLeaveRateOverTarget", c => c.Boolean(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "staffShiftCancelRateOverTarget", c => c.Boolean(nullable: false));
            DropColumn("dbo.GlobalSettingsModel", "staffShiftLeaveRateUnderTarget");
            DropColumn("dbo.GlobalSettingsModel", "staffShiftCancelRateUnderTarget");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GlobalSettingsModel", "staffShiftCancelRateUnderTarget", c => c.Boolean(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "staffShiftLeaveRateUnderTarget", c => c.Boolean(nullable: false));
            DropColumn("dbo.GlobalSettingsModel", "staffShiftCancelRateOverTarget");
            DropColumn("dbo.GlobalSettingsModel", "staffShiftLeaveRateOverTarget");
        }
    }
}
