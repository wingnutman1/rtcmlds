namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eschours2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RosterModel", "StaffID", c => c.Int());
            AddColumn("dbo.RosterModel", "ClientID", c => c.Int());
            AddColumn("dbo.RosterModel", "SiteID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RosterModel", "SiteID");
            DropColumn("dbo.RosterModel", "ClientID");
            DropColumn("dbo.RosterModel", "StaffID");
        }
    }
}
