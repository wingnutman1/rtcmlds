namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qualifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Qualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ValidityPeriod = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SiteRequiredQualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SiteID = c.Int(nullable: false),
                        QualificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StaffQualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StaffID = c.Int(nullable: false),
                        QualificationID = c.Int(nullable: false),
                        RenewalDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        expired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StaffQualification");
            DropTable("dbo.SiteRequiredQualification");
            DropTable("dbo.Qualification");
        }
    }
}
