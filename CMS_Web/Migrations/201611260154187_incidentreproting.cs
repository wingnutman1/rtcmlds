namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class incidentreproting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incident", "currentActionByDate", c => c.DateTime());
            AddColumn("dbo.ToDoListItem", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoListItem", "Deleted");
            DropColumn("dbo.Incident", "currentActionByDate");
        }
    }
}
