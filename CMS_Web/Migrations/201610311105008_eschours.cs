namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eschours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExceptionDetail", "relatedIncidentID", c => c.Int());
            AddColumn("dbo.GlobalSettingsModel", "hoursBetweenExceptionEscallation", c => c.Int(nullable: false));
            DropColumn("dbo.ExceptionDetail", "IncidentID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExceptionDetail", "IncidentID", c => c.Int());
            DropColumn("dbo.GlobalSettingsModel", "hoursBetweenExceptionEscallation");
            DropColumn("dbo.ExceptionDetail", "relatedIncidentID");
        }
    }
}
