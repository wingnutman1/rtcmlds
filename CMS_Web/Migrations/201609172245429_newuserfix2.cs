namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newuserfix2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.webpages_Roles", "Membership_UserId", c => c.Int());
            CreateIndex("dbo.webpages_Roles", "Membership_UserId");
            AddForeignKey("dbo.webpages_Roles", "Membership_UserId", "dbo.webpages_Membership", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.webpages_Roles", "Membership_UserId", "dbo.webpages_Membership");
            DropIndex("dbo.webpages_Roles", new[] { "Membership_UserId" });
            DropColumn("dbo.webpages_Roles", "Membership_UserId");
        }
    }
}
