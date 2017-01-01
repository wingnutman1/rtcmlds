namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class now1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incident", "TypeID", c => c.Int(nullable: false));
            DropColumn("dbo.Incident", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Incident", "Type", c => c.String());
            DropColumn("dbo.Incident", "TypeID");
        }
    }
}
