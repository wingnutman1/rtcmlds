namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemLogEntry",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false),
                        StaffID = c.Int(nullable: false),
                        Description = c.String(),
                        LogType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserActivityLogEntry",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false),
                        StaffID = c.Int(nullable: false),
                        Description = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserActivityLogEntry");
            DropTable("dbo.SystemLogEntry");
        }
    }
}
