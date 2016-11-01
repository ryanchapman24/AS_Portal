namespace AS_TestProject.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AS_TestProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AS_TestProject.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Executive"))
            {
                roleManager.Create(new IdentityRole { Name = "Executive" });
            }

            if (!context.Roles.Any(r => r.Name == "IT"))
            {
                roleManager.Create(new IdentityRole { Name = "IT" });
            }

            if (!context.Roles.Any(r => r.Name == "HR"))
            {
                roleManager.Create(new IdentityRole { Name = "HR" });
            }

            if (!context.Roles.Any(r => r.Name == "Operations"))
            {
                roleManager.Create(new IdentityRole { Name = "Operations" });
            }

            if (!context.Roles.Any(r => r.Name == "Marketing"))
            {
                roleManager.Create(new IdentityRole { Name = "Marketing" });
            }

            if (!context.Roles.Any(r => r.Name == "CSR"))
            {
                roleManager.Create(new IdentityRole { Name = "CSR" });
            }

            if (!context.Roles.Any(r => r.Name == "Payroll"))
            {
                roleManager.Create(new IdentityRole { Name = "Payroll" });
            }

            if (!context.Roles.Any(r => r.Name == "Quality"))
            {
                roleManager.Create(new IdentityRole { Name = "Quality" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "your email address"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "rchapman@anomalysquared.com",
                    Email = "rchapman@anomalysquared.com",
                    FirstName = "Ryan",
                    LastName = "Chapman",
                    DisplayName = "Ryan Chapman",                  
                    PhoneNumber = "(919) 698-2849",
                }, "Chappy24!");
            }

            var userId_Admin = userManager.FindByEmail("rchapman@anomalysquared.com").Id;
            userManager.AddToRole(userId_Admin, "Admin");          
        }
    }
}
