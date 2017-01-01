namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reporting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StaffReportsTo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StaffID = c.Int(nullable: false),
                        ReportsToStaffID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.UserProfile", "PositionDescription", c => c.String());
            DropTable("dbo.StaffReporting");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StaffReporting",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StaffID = c.Int(nullable: false),
                        ReportStaffID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.UserProfile", "PositionDescription");
            DropTable("dbo.StaffReportsTo");
        }
    }
}
