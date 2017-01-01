namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shiftexp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "currentLocationID", c => c.Int(nullable: false));
            DropColumn("dbo.UserProfile", "currentLocaitonID");
            DropColumn("dbo.RosterModel", "exceptionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RosterModel", "exceptionID", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfile", "currentLocaitonID", c => c.Int(nullable: false));
            DropColumn("dbo.UserProfile", "currentLocationID");
        }
    }
}
