namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1708 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncidentType", "firstTime", c => c.Time(precision: 7));
            DropColumn("dbo.IncidentType", "firstTimeToEsc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IncidentType", "firstTimeToEsc", c => c.Time(precision: 7));
            DropColumn("dbo.IncidentType", "firstTime");
        }
    }
}
