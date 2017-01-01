namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17083 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncidentType", "secondStaffEscID", c => c.Int());
            AddColumn("dbo.IncidentType", "thirdStaffEscID", c => c.Int());
            DropColumn("dbo.IncidentType", "secondStaffEsc");
            DropColumn("dbo.IncidentType", "thirdStaffEsc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IncidentType", "thirdStaffEsc", c => c.Int());
            AddColumn("dbo.IncidentType", "secondStaffEsc", c => c.Int());
            DropColumn("dbo.IncidentType", "thirdStaffEscID");
            DropColumn("dbo.IncidentType", "secondStaffEscID");
        }
    }
}
