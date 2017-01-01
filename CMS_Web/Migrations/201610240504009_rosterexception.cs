namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rosterexception : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RosterModel", "exceptionID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RosterModel", "exceptionID");
        }
    }
}
