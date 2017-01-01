namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class escallationstate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RosterModel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Description = c.String(),
                        IsAllDay = c.Boolean(nullable: false),
                        Recurrence = c.String(),
                        RecurrenceRule = c.String(),
                        RecurrenceException = c.String(),
                        EndTimezone = c.String(),
                        StartTimezone = c.String(),
                        StaffName = c.String(),
                        ClientName = c.String(),
                        SiteName = c.String(),
                        escallationStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RosterModel");
        }
    }
}
