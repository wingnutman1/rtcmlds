namespace CMS_Web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CMS_Web.Models;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<CMS_Web.DAL.CMS_WebContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CMS_Web.DAL.CMS_WebContext context)
        {
            // Default Users
            WebSecurity.InitializeDatabaseConnection(
                "CMS_WebContext",
                "UserProfile",
                "UserId",
                "UserName", autoCreateTables: true);

            if (!Roles.RoleExists("Admin"))
            {
                Roles.CreateRole("Admin");
            }

            if (!Roles.RoleExists("Manager"))
            {
                Roles.CreateRole("Manager");
            }

            if (!Roles.RoleExists("Staff"))
            {
                Roles.CreateRole("Staff");
            }

            if (!WebSecurity.UserExists("john@ldssa.org.au"))
            {
                WebSecurity.CreateUserAndAccount("john@ldssa.org.au", "password", new { LoggedOn = false });
            }
            if (!Roles.GetRolesForUser("john@ldssa.org.au").Contains("Admin"))
            {
                Roles.AddUsersToRoles(new[] { "john@ldssa.org.au" }, new[] { "Admin" });
            }

            if (!WebSecurity.UserExists("manager@ldssa.org.au"))
            {
                WebSecurity.CreateUserAndAccount("manager@ldssa.org.au", "password", new { LoggedOn = false });
            }
            if (!Roles.GetRolesForUser("manager@ldssa.org.au").Contains("Manager"))
            {
                Roles.AddUsersToRoles(new[] { "manager@ldssa.org.au" }, new[] { "Manager" });
            }

            if (!WebSecurity.UserExists("staff@ldssa.org.au"))
            {
                WebSecurity.CreateUserAndAccount("staff@ldssa.org.au", "password", new { LoggedOn = false });
            }
            if (!Roles.GetRolesForUser("staff@ldssa.org.au").Contains("Staff"))
            {
                Roles.AddUsersToRoles(new[] { "staff@ldssa.org.au" }, new[] { "Staff" });
            }



            //Default Clients
            var clients = new List<Client>
            {
                new Client{FirstName = "Helen", LastName = "Watts"},
                new Client{FirstName = "David", LastName = "Hale"}
            };

            if (context.Clients.Count() == 0)
            {
                clients.ForEach(c => context.Clients.Add(c));
                context.SaveChanges();
            }


        }
    }
}
