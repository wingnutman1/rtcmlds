namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _410 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Client", "Inactive", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Client", "Inactive", c => c.Boolean(nullable: false));
        }
    }
}
