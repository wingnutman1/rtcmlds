namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class globalconfigperformance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettingsModel", "numberOfDaysDurationPerformanceMetricAnalysis", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "minimumPercentToDoListCompletions", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "minimumPercentOnTimeShiftArrivals", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "minimumPercentEarlyShiftLeaving", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "minimumPercentShiftCancellations", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalSettingsModel", "minimumPercentShiftCancellations");
            DropColumn("dbo.GlobalSettingsModel", "minimumPercentEarlyShiftLeaving");
            DropColumn("dbo.GlobalSettingsModel", "minimumPercentOnTimeShiftArrivals");
            DropColumn("dbo.GlobalSettingsModel", "minimumPercentToDoListCompletions");
            DropColumn("dbo.GlobalSettingsModel", "numberOfDaysDurationPerformanceMetricAnalysis");
        }
    }
}
