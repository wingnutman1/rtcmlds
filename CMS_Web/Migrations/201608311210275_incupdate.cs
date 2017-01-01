namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class incupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incident", "CurrentManagerID", c => c.Int(nullable: false));
            AddColumn("dbo.Incident", "StaffReportedID", c => c.Int(nullable: false));
            DropColumn("dbo.Incident", "ManagerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Incident", "ManagerID", c => c.Int(nullable: false));
            DropColumn("dbo.Incident", "StaffReportedID");
            DropColumn("dbo.Incident", "CurrentManagerID");
        }
    }
}
