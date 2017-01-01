namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rel1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incident", "UserProfileID", c => c.Int());
            AddColumn("dbo.Incident", "UserProfile_UserId", c => c.Int());
            CreateIndex("dbo.Incident", "LocationID");
            CreateIndex("dbo.Incident", "ClientID");
            CreateIndex("dbo.Incident", "UserProfile_UserId");
            AddForeignKey("dbo.Incident", "ClientID", "dbo.Client", "ID");
            AddForeignKey("dbo.Incident", "LocationID", "dbo.Location", "ID");
            AddForeignKey("dbo.Incident", "UserProfile_UserId", "dbo.UserProfile", "UserId");
            DropColumn("dbo.Incident", "StaffID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Incident", "StaffID", c => c.Int());
            DropForeignKey("dbo.Incident", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Incident", "LocationID", "dbo.Location");
            DropForeignKey("dbo.Incident", "ClientID", "dbo.Client");
            DropIndex("dbo.Incident", new[] { "UserProfile_UserId" });
            DropIndex("dbo.Incident", new[] { "ClientID" });
            DropIndex("dbo.Incident", new[] { "LocationID" });
            DropColumn("dbo.Incident", "UserProfile_UserId");
            DropColumn("dbo.Incident", "UserProfileID");
        }
    }
}
