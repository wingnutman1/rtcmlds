namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class todoperf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettingsModel", "numberOfDaysDurationToDoPerformanceAnalysis", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalSettingsModel", "numberOfDaysDurationToDoPerformanceAnalysis");
        }
    }
}
