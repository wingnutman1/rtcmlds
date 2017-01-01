namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qualifications1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Qualification", "MonthsValidFor", c => c.Int(nullable: false));
            DropColumn("dbo.Qualification", "ValidityPeriod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Qualification", "ValidityPeriod", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Qualification", "MonthsValidFor");
        }
    }
}
