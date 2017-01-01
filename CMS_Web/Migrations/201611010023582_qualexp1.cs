namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qualexp1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffQualification", "expiryExceptionID", c => c.Int(nullable: false));
            AddColumn("dbo.StaffQualification", "expiryWarningExceptionID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffQualification", "expiryWarningExceptionID");
            DropColumn("dbo.StaffQualification", "expiryExceptionID");
        }
    }
}
