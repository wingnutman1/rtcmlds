namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class systemmetrics1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RosterModel", "CancelledByStaff", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RosterModel", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RosterModel", "Deleted", c => c.Boolean());
            DropColumn("dbo.RosterModel", "CancelledByStaff");
        }
    }
}
