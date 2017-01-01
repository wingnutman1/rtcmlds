namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eschours1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExceptionHistory", "stateAtHistoryRecordCreation", c => c.Int(nullable: false));
            DropColumn("dbo.ExceptionHistory", "state");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExceptionHistory", "state", c => c.Int(nullable: false));
            DropColumn("dbo.ExceptionHistory", "stateAtHistoryRecordCreation");
        }
    }
}
