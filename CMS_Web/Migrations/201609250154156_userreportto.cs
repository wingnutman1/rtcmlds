namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userreportto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "reportsTo", c => c.Int());
            DropTable("dbo.StaffReportsTo");
        }
        
        public override void Down()
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
            
            DropColumn("dbo.UserProfile", "reportsTo");
        }
    }
}
