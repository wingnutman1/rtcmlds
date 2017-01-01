namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class incidents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncidentHistory", "historyEntryCreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.IncidentHistory", "actionByDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.IncidentHistory", "state", c => c.String());
            AddColumn("dbo.IncidentHistory", "currentActionDescription", c => c.String());
            AddColumn("dbo.IncidentHistory", "actionByStaffID", c => c.Int(nullable: false));
            AddColumn("dbo.IncidentHistory", "currentStaffID", c => c.Int(nullable: false));
            AddColumn("dbo.Incident", "incidentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Incident", "reportedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.IncidentHistory", "creationDate");
            DropColumn("dbo.IncidentHistory", "description");
            DropColumn("dbo.IncidentHistory", "staffID");
            DropColumn("dbo.Incident", "creationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Incident", "creationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.IncidentHistory", "staffID", c => c.Int(nullable: false));
            AddColumn("dbo.IncidentHistory", "description", c => c.String());
            AddColumn("dbo.IncidentHistory", "creationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Incident", "reportedDate");
            DropColumn("dbo.Incident", "incidentDate");
            DropColumn("dbo.IncidentHistory", "currentStaffID");
            DropColumn("dbo.IncidentHistory", "actionByStaffID");
            DropColumn("dbo.IncidentHistory", "currentActionDescription");
            DropColumn("dbo.IncidentHistory", "state");
            DropColumn("dbo.IncidentHistory", "actionByDate");
            DropColumn("dbo.IncidentHistory", "historyEntryCreationDate");
        }
    }
}
