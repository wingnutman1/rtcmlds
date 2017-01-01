namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class locationquals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationRequiredQualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LocationID = c.Int(nullable: false),
                        QualificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.SiteRequiredQualification");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SiteRequiredQualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SiteID = c.Int(nullable: false),
                        QualificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.LocationRequiredQualification");
        }
    }
}
