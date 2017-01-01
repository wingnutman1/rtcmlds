namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expprocessing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "currentLocaitonID", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfile", "lastOnlineEventTime", c => c.DateTime());
            AddColumn("dbo.UserProfile", "lastOfflineEventTime", c => c.DateTime());
            AddColumn("dbo.GlobalSettingsModel", "minutesBeforeShiftToCheckIfStaffOnline", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "minutesAllowedForLateShiftArrival", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "minutesAllowedForEarlyShiftLeave", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "minutesAllowedForStaffOfflineDuringShift", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalSettingsModel", "minutesAllowedForStaffOfflineDuringShift");
            DropColumn("dbo.GlobalSettingsModel", "minutesAllowedForEarlyShiftLeave");
            DropColumn("dbo.GlobalSettingsModel", "minutesAllowedForLateShiftArrival");
            DropColumn("dbo.GlobalSettingsModel", "minutesBeforeShiftToCheckIfStaffOnline");
            DropColumn("dbo.UserProfile", "lastOfflineEventTime");
            DropColumn("dbo.UserProfile", "lastOnlineEventTime");
            DropColumn("dbo.UserProfile", "currentLocaitonID");
        }
    }
}
