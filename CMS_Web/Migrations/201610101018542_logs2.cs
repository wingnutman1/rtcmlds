namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logs2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemLogEntry", "module", c => c.String());
            AddColumn("dbo.SystemLogEntry", "function", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemLogEntry", "function");
            DropColumn("dbo.SystemLogEntry", "module");
        }
    }
}
