namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1608 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncidentType", "firstStaffEscID", c => c.Int());
            AddColumn("dbo.IncidentType", "firstStaffEsc_UserId", c => c.Int());
            CreateIndex("dbo.IncidentType", "firstStaffEsc_UserId");
            AddForeignKey("dbo.IncidentType", "firstStaffEsc_UserId", "dbo.UserProfile", "UserId");
            DropColumn("dbo.IncidentType", "firstStaffEsc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IncidentType", "firstStaffEsc", c => c.Int());
            DropForeignKey("dbo.IncidentType", "firstStaffEsc_UserId", "dbo.UserProfile");
            DropIndex("dbo.IncidentType", new[] { "firstStaffEsc_UserId" });
            DropColumn("dbo.IncidentType", "firstStaffEsc_UserId");
            DropColumn("dbo.IncidentType", "firstStaffEscID");
        }
    }
}
