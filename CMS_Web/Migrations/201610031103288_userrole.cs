namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userrole : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.webpages_UsersInRoles");
            AddPrimaryKey("dbo.webpages_UsersInRoles", new[] { "UserId", "RoleId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.webpages_UsersInRoles");
            AddPrimaryKey("dbo.webpages_UsersInRoles", new[] { "RoleId", "UserId" });
        }
    }
}
