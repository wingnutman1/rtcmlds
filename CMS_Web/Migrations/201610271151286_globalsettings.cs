namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class globalsettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GlobalSettingsModel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        daysBeforeShiftToCheckQualifications = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.RosterModel", "StaffFullName", c => c.String());
            AddColumn("dbo.RosterModel", "StaffFirstName", c => c.String());
            AddColumn("dbo.RosterModel", "StaffLastName", c => c.String());
            AddColumn("dbo.RosterModel", "ClientFullName", c => c.String());
            AddColumn("dbo.RosterModel", "ClientFirstName", c => c.String());
            AddColumn("dbo.RosterModel", "ClientLastName", c => c.String());
            AddColumn("dbo.RosterModel", "notOnlineBeforeShiftExceptionGenerated", c => c.Boolean(nullable: false));
            AddColumn("dbo.RosterModel", "notArrivedToStartShiftExceptionGenerated", c => c.Boolean(nullable: false));
            AddColumn("dbo.RosterModel", "offlineDuringShiftExceptionGenerated", c => c.Boolean(nullable: false));
            AddColumn("dbo.RosterModel", "leaveBeforeShiftCompleteExceptionGenerated", c => c.Boolean(nullable: false));
            AddColumn("dbo.RosterModel", "qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated", c => c.Boolean(nullable: false));
            AddColumn("dbo.RosterModel", "qualificationsNotCurrentWhenArrivingAtShiftExceptionGenerated", c => c.Boolean(nullable: false));
            AddColumn("dbo.RosterModel", "qualificationsNotCurrentWarningBeforeShiftExceptionGenerated", c => c.Boolean(nullable: false));
            DropColumn("dbo.RosterModel", "StaffName");
            DropColumn("dbo.RosterModel", "ClientName");
            DropColumn("dbo.RosterModel", "escallationStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RosterModel", "escallationStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RosterModel", "ClientName", c => c.String());
            AddColumn("dbo.RosterModel", "StaffName", c => c.String());
            DropColumn("dbo.RosterModel", "qualificationsNotCurrentWarningBeforeShiftExceptionGenerated");
            DropColumn("dbo.RosterModel", "qualificationsNotCurrentWhenArrivingAtShiftExceptionGenerated");
            DropColumn("dbo.RosterModel", "qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated");
            DropColumn("dbo.RosterModel", "leaveBeforeShiftCompleteExceptionGenerated");
            DropColumn("dbo.RosterModel", "offlineDuringShiftExceptionGenerated");
            DropColumn("dbo.RosterModel", "notArrivedToStartShiftExceptionGenerated");
            DropColumn("dbo.RosterModel", "notOnlineBeforeShiftExceptionGenerated");
            DropColumn("dbo.RosterModel", "ClientLastName");
            DropColumn("dbo.RosterModel", "ClientFirstName");
            DropColumn("dbo.RosterModel", "ClientFullName");
            DropColumn("dbo.RosterModel", "StaffLastName");
            DropColumn("dbo.RosterModel", "StaffFirstName");
            DropColumn("dbo.RosterModel", "StaffFullName");
            DropTable("dbo.GlobalSettingsModel");
        }
    }
}
