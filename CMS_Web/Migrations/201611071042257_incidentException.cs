namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class incidentException : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncidentHistory", "actionByDateExceededExceptionRaised", c => c.Boolean(nullable: false));
            AddColumn("dbo.IncidentHistory", "actionByDateExceededExceptionID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IncidentHistory", "actionByDateExceededExceptionID");
            DropColumn("dbo.IncidentHistory", "actionByDateExceededExceptionRaised");
        }
    }
}
