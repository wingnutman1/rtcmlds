namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newuserfix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfile", "LoggedOn", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfile", "LoggedOn", c => c.Boolean(nullable: false));
        }
    }
}
