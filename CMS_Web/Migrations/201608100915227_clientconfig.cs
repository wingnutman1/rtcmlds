namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clientconfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientConfigurationModel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        LicenceExpiryDate = c.DateTime(nullable: false),
                        LicenceReminderDate = c.DateTime(nullable: false),
                        MaxLogins = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClientConfigurationModel");
        }
    }
}
