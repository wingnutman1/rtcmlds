namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class perfreport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettingsModel", "staffIDForPerformanceReporting", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalSettingsModel", "staffIDForPerformanceReporting");
        }
    }
}
