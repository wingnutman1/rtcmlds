namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class todoexp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoListItem", "itemNotCompleteInAllocatedTimeExceptionRaised", c => c.Boolean(nullable: false));
            AddColumn("dbo.ToDoListItem", "itemNotCompleteInAllocatedTimeExceptionID", c => c.Int(nullable: false));
            AddColumn("dbo.ToDoListItem", "itemBumpedOutExceptionRaised", c => c.Boolean(nullable: false));
            AddColumn("dbo.ToDoListItem", "itemBumpedOutExceptionID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoListItem", "itemBumpedOutExceptionID");
            DropColumn("dbo.ToDoListItem", "itemBumpedOutExceptionRaised");
            DropColumn("dbo.ToDoListItem", "itemNotCompleteInAllocatedTimeExceptionID");
            DropColumn("dbo.ToDoListItem", "itemNotCompleteInAllocatedTimeExceptionRaised");
        }
    }
}
