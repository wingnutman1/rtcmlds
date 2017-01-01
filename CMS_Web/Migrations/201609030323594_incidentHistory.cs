namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class incidentHistory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IncidentHistory", "state", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IncidentHistory", "state", c => c.String());
        }
    }
}
