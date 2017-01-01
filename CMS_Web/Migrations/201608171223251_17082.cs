namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17082 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncidentType", "firstEscDays", c => c.Int());
            AddColumn("dbo.IncidentType", "firstEscHours", c => c.Int());
            AddColumn("dbo.IncidentType", "firstEscMinutes", c => c.Int());
            AddColumn("dbo.IncidentType", "secondEscDays", c => c.Int());
            AddColumn("dbo.IncidentType", "secondEscHours", c => c.Int());
            AddColumn("dbo.IncidentType", "secondEscMinutes", c => c.Int());
            AddColumn("dbo.IncidentType", "thirdEscDays", c => c.Int());
            AddColumn("dbo.IncidentType", "thirdEscHours", c => c.Int());
            AddColumn("dbo.IncidentType", "thirdEscMinutes", c => c.Int());
            DropColumn("dbo.IncidentType", "firstTime");
            DropColumn("dbo.IncidentType", "secondTimeToEsc");
            DropColumn("dbo.IncidentType", "thirdTimeToEsc");
            DropColumn("dbo.IncidentType", "fourthStaffEsc");
            DropColumn("dbo.IncidentType", "fourthTimeToEsc");
            DropColumn("dbo.IncidentType", "fifthStaffEsc");
            DropColumn("dbo.IncidentType", "fifthTimeToEsc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IncidentType", "fifthTimeToEsc", c => c.Time(precision: 7));
            AddColumn("dbo.IncidentType", "fifthStaffEsc", c => c.Int());
            AddColumn("dbo.IncidentType", "fourthTimeToEsc", c => c.Time(precision: 7));
            AddColumn("dbo.IncidentType", "fourthStaffEsc", c => c.Int());
            AddColumn("dbo.IncidentType", "thirdTimeToEsc", c => c.Time(precision: 7));
            AddColumn("dbo.IncidentType", "secondTimeToEsc", c => c.Time(precision: 7));
            AddColumn("dbo.IncidentType", "firstTime", c => c.Time(precision: 7));
            DropColumn("dbo.IncidentType", "thirdEscMinutes");
            DropColumn("dbo.IncidentType", "thirdEscHours");
            DropColumn("dbo.IncidentType", "thirdEscDays");
            DropColumn("dbo.IncidentType", "secondEscMinutes");
            DropColumn("dbo.IncidentType", "secondEscHours");
            DropColumn("dbo.IncidentType", "secondEscDays");
            DropColumn("dbo.IncidentType", "firstEscMinutes");
            DropColumn("dbo.IncidentType", "firstEscHours");
            DropColumn("dbo.IncidentType", "firstEscDays");
        }
    }
}
