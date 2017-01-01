namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qualrenew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettingsModel", "daysBeforeQualificaitonExpiryForReminder", c => c.Int(nullable: false));
            AddColumn("dbo.Qualification", "renewalActionMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Qualification", "renewalActionMessage");
            DropColumn("dbo.GlobalSettingsModel", "daysBeforeQualificaitonExpiryForReminder");
        }
    }
}
