namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _140916 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incident", "currentState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Incident", "currentState");
        }
    }
}
