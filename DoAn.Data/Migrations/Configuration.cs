namespace DoAn.Data.Migrations
{
    using DoAn.Data.Model;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DoAn.Data.BkresContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DoAn.Data.BkresContext context)
        {
            CreateUser(context)
        ;
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
        private void CreateUser(BkresContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new BkresContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BkresContext()));
            if (!manager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    UserName = "trangngo",
                    Email = "ngothitrang.set@gmail.com",
                    EmailConfirmed = true,

                    FullName = "Ngô Thị Trang"

                };

                manager.Create(user, "123456");

                if (!roleManager.Roles.Any())
                {
                    roleManager.Create(new IdentityRole { Name = "Admin" });
                    roleManager.Create(new IdentityRole { Name = "User" });
                }

                var adminUser = manager.FindByEmail("tedu.international@gmail.com");

                manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
            }
        }
    }
}
