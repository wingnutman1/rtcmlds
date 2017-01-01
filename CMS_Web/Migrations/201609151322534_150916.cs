namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _150916 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IncidentHistory", "actionByDate", c => c.DateTime());
            AlterColumn("dbo.IncidentHistory", "actionByStaffID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IncidentHistory", "actionByStaffID", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentHistory", "actionByDate", c => c.DateTime(nullable: false));
        }
    }
}
