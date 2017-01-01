namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exceptionnote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExceptionNote", "LastEditStaffID", c => c.Int(nullable: false));
            AddColumn("dbo.ExceptionNote", "EditDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ExceptionNote", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExceptionNote", "Deleted");
            DropColumn("dbo.ExceptionNote", "EditDate");
            DropColumn("dbo.ExceptionNote", "LastEditStaffID");
        }
    }
}
