namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exception1909 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExceptionHistory", "state", c => c.Int(nullable: false));
            AddColumn("dbo.ExceptionDetail", "state", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExceptionDetail", "state");
            DropColumn("dbo.ExceptionHistory", "state");
        }
    }
}
