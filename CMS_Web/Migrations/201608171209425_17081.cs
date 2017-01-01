namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17081 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IncidentType", "firstStaffEsc_UserId", "dbo.UserProfile");
            DropIndex("dbo.IncidentType", new[] { "firstStaffEsc_UserId" });
            DropColumn("dbo.IncidentType", "firstStaffEsc_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IncidentType", "firstStaffEsc_UserId", c => c.Int());
            CreateIndex("dbo.IncidentType", "firstStaffEsc_UserId");
            AddForeignKey("dbo.IncidentType", "firstStaffEsc_UserId", "dbo.UserProfile", "UserId");
        }
    }
}
