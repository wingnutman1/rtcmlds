namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roster_import_errors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RosterImportErrorDetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StaffID = c.Int(nullable: false),
                        ImportDate = c.DateTime(nullable: false),
                        ErrorDetail = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RosterImportErrorDetail");
        }
    }
}
