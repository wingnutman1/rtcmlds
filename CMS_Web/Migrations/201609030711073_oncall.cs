namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oncall : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnCallCalendarItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        calendarID = c.Int(nullable: false),
                        staffID = c.String(),
                        dateStart = c.DateTime(nullable: false),
                        dateEnd = c.DateTime(nullable: false),
                        timeStart = c.DateTime(nullable: false),
                        timeEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OnCallCalendar",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.IncidentType", "useOnCallProcessing", c => c.Boolean(nullable: false));
            AddColumn("dbo.IncidentType", "onCallCalendarID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IncidentType", "onCallCalendarID");
            DropColumn("dbo.IncidentType", "useOnCallProcessing");
            DropTable("dbo.OnCallCalendar");
            DropTable("dbo.OnCallCalendarItem");
        }
    }
}
