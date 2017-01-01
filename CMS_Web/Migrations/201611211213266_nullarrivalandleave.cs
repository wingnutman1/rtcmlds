namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullarrivalandleave : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RosterModel", "ActualArrivalTime", c => c.DateTime());
            AlterColumn("dbo.RosterModel", "ActualLeaveTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RosterModel", "ActualLeaveTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RosterModel", "ActualArrivalTime", c => c.DateTime(nullable: false));
        }
    }
}
