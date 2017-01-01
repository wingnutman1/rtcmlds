namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rosterdeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RosterModel", "Deleted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RosterModel", "Deleted");
        }
    }
}
