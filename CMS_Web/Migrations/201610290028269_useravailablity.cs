namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useravailablity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "delegateID", c => c.Int());
            AddColumn("dbo.UserProfile", "available", c => c.Boolean());
            AddColumn("dbo.UserProfile", "availablityStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "availablityStatus");
            DropColumn("dbo.UserProfile", "available");
            DropColumn("dbo.UserProfile", "delegateID");
        }
    }
}
