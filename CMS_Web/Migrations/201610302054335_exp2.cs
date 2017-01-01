namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exp2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExceptionDetail", "relatedStaffID", c => c.Int());
            AddColumn("dbo.ExceptionDetail", "relatedClientID", c => c.Int());
            AddColumn("dbo.ExceptionDetail", "realtedLocationID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExceptionDetail", "realtedLocationID");
            DropColumn("dbo.ExceptionDetail", "relatedClientID");
            DropColumn("dbo.ExceptionDetail", "relatedStaffID");
        }
    }
}
