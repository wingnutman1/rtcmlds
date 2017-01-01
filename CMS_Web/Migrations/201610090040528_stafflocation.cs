namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stafflocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "currentLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "currentLocation");
        }
    }
}
