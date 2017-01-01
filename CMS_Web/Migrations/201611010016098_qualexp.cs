namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qualexp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffQualification", "expiryExceptionRaised", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffQualification", "expiryWarningExceptionRaised", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffQualification", "expiryWarningExceptionRaised");
            DropColumn("dbo.StaffQualification", "expiryExceptionRaised");
        }
    }
}
