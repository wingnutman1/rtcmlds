namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _38161 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncidentType", "templateFileLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IncidentType", "templateFileLocation");
        }
    }
}
