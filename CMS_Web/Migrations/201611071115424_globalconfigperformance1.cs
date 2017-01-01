namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class globalconfigperformance1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettingsModel", "maximumPercentEarlyShiftLeaving", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "maximumPercentShiftCancellations", c => c.Int(nullable: false));
            DropColumn("dbo.GlobalSettingsModel", "minimumPercentEarlyShiftLeaving");
            DropColumn("dbo.GlobalSettingsModel", "minimumPercentShiftCancellations");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GlobalSettingsModel", "minimumPercentShiftCancellations", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettingsModel", "minimumPercentEarlyShiftLeaving", c => c.Int(nullable: false));
            DropColumn("dbo.GlobalSettingsModel", "maximumPercentShiftCancellations");
            DropColumn("dbo.GlobalSettingsModel", "maximumPercentEarlyShiftLeaving");
        }
    }
}
