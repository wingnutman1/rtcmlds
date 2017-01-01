namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logs3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SystemLogEntry", "StaffID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SystemLogEntry", "StaffID", c => c.Int(nullable: false));
        }
    }
}
