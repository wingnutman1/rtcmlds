namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExceptionEscallationDetail", "HoursDelayBeforeEscallation", c => c.Int(nullable: false));
            AddColumn("dbo.ExceptionEscallationDetail", "MinutesDelayBeforeEscallation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExceptionEscallationDetail", "MinutesDelayBeforeEscallation");
            DropColumn("dbo.ExceptionEscallationDetail", "HoursDelayBeforeEscallation");
        }
    }
}
