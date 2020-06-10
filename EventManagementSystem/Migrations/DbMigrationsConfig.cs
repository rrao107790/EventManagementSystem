namespace EventManagementSystem.Migrations
{
    using EventManagementSystem.Data;
    using EventManagementSystem.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class DbMigrationsConfig : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // seed data only if database's table is empty
            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminUsername = adminEmail;
                var adminPassword = adminEmail;
                var adminFullName = "System Administrator";
                string adminRole = "Administrator";
                CreateAdminUser(context, adminEmail, adminUsername,  adminPassword, adminFullName,adminRole);
                CreateSeveralEvents(context);
            }
        }

        private void CreateSeveralEvents(ApplicationDbContext context)
        {
            context.Events.Add(new Event()
            {
                Title = "Party at Company",
                StartDateTime = DateTime.Now.Date.AddDays(5).AddHours(21).AddMinutes(30),
                Author = context.Users.First()
            });

            context.Events.Add(new Event()
            {
                Title = "Passed Event <Anonymous>",
                StartDateTime = DateTime.Now.Date.AddDays(-2).AddHours(10).AddMinutes(30),
                Duration = TimeSpan.FromHours(1.5),
                Comments = new HashSet<Comment>() { new Comment() { Text="<Anonymous> comment"},new Comment() { Text="User Comment",Author = context.Users.First()}, }
            });

        }

        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUsername, string adminPassword, string adminFullName,string adminRole)
        {
            // create admin user
            var adminUser = new ApplicationUser
            {
                UserName = adminUsername,
                FullName = adminFullName,
                Email = adminEmail
            };

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore)
            {
                PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 2,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false
                }
            };

            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded) throw new Exception(string.Join(";", userCreateResult.Errors));


            // create "Administrator" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded) throw new Exception(string.Join(";", roleCreateResult.Errors));


            // Add the "admin" user to "Administrator" role
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded) throw new Exception(string.Join(";", addAdminRoleResult.Errors));
        }
    }
}
