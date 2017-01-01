namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sysstatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        incidentEngineEnabled = c.Boolean(nullable: false),
                        incidentEngineStatus = c.String(),
                        todoEngineEnabled = c.Boolean(nullable: false),
                        todoEngineStatus = c.String(),
                        exceptionEngineEnabled = c.Boolean(nullable: false),
                        exceptionEngineStatus = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemStatus");
        }
    }
}
