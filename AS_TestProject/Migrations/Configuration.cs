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

            if (!context.Users.Any(u => u.Email == "your email address"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "exectutive@as.com",
                    Email = "executive@as.com",
                    FirstName = "Executive",
                    LastName = "Demo",
                    DisplayName = "Executive Demo",
                    PhoneNumber = "(111) 222-3333",
                }, "Password1!");
            }

            var userId_ExecutiveGuest = userManager.FindByEmail("executive@as.com").Id;
            userManager.AddToRole(userId_ExecutiveGuest, "Executive");

            if (!context.Users.Any(u => u.Email == "your email address"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "it@as.com",
                    Email = "it@as.com",
                    FirstName = "IT",
                    LastName = "Demo",
                    DisplayName = "IT Demo",
                    PhoneNumber = "(111) 222-3333",
                }, "Password1!");
            }

            var userId_ITGuest = userManager.FindByEmail("it@as.com").Id;
            userManager.AddToRole(userId_ITGuest, "IT");

            if (!context.Users.Any(u => u.Email == "your email address"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hr@as.com",
                    Email = "hr@as.com",
                    FirstName = "HR",
                    LastName = "Demo",
                    DisplayName = "HR Demo",
                    PhoneNumber = "(111) 222-3333",
                }, "Password1!");
            }

            var userId_HRGuest = userManager.FindByEmail("hr@as.com").Id;
            userManager.AddToRole(userId_HRGuest, "HR");

            if (!context.Users.Any(u => u.Email == "your email address"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "operations@as.com",
                    Email = "operations@as.com",
                    FirstName = "Operations",
                    LastName = "Demo",
                    DisplayName = "Operations Demo",
                    PhoneNumber = "(111) 222-3333",
                }, "Password1!");
            }

            var userId_OperationsGuest = userManager.FindByEmail("operations@as.com").Id;
            userManager.AddToRole(userId_OperationsGuest, "Operations");

            if (!context.Users.Any(u => u.Email == "your email address"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "marketing@as.com",
                    Email = "marketing@as.com",
                    FirstName = "Marketing",
                    LastName = "Demo",
                    DisplayName = "Marketing Demo",
                    PhoneNumber = "(111) 222-3333",
                }, "Password1!");
            }

            var userId_MarketingGuest = userManager.FindByEmail("marketing@as.com").Id;
            userManager.AddToRole(userId_MarketingGuest, "Marketing");


            if (!context.Users.Any(u => u.Email == "your email address"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "csr@as.com",
                    Email = "csr@as.com",
                    FirstName = "CSR",
                    LastName = "Demo",
                    DisplayName = "CSR Demo",
                    PhoneNumber = "(111) 222-3333",
                }, "Password1!");
            }

            var userId_CSRGuest = userManager.FindByEmail("csr@as.com").Id;
            userManager.AddToRole(userId_CSRGuest, "CSR");
        }
    }
}
