namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class systemmetrics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettingsModel", "todoRateUnderTarget", c => c.Boolean(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "staffShiftArrivalsRateUnderTarget", c => c.Boolean(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "staffShiftLeaveRateUnderTarget", c => c.Boolean(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "staffShiftCancelRateUnderTarget", c => c.Boolean(nullable: false));
            AddColumn("dbo.RosterModel", "ActualArrivalTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.RosterModel", "ActualLeaveTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RosterModel", "ActualLeaveTime");
            DropColumn("dbo.RosterModel", "ActualArrivalTime");
            DropColumn("dbo.GlobalSettingsModel", "staffShiftCancelRateUnderTarget");
            DropColumn("dbo.GlobalSettingsModel", "staffShiftLeaveRateUnderTarget");
            DropColumn("dbo.GlobalSettingsModel", "staffShiftArrivalsRateUnderTarget");
            DropColumn("dbo.GlobalSettingsModel", "todoRateUnderTarget");
        }
    }
}
