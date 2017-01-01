namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qualifications2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientRequiredQualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClientID = c.Int(nullable: false),
                        QualificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClientRequiredQualification");
        }
    }
}
