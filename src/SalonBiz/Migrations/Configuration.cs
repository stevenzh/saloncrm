namespace SalonCRM.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SalonCRM.Models;
    using SalonCRM.Tools;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //ApplicationRole role2 = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "管理员" };
            //ApplicationRole role1 = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "美容师" };
            //ApplicationRole role3 = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "超级管理员" };
            //ApplicationUser user1 = new ApplicationUser
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    UserName = "admin",
            //    Email = "admin@ymail.com",
            //    FirstName = "Admin",
            //    Password = Security.ToEncrypt("123456"),
            //    IsActive = true,
            //    HostId = 1,
            //    OrganId = 1,
            //    CreateDate = DateTime.UtcNow,
            //    Roles = new List<ApplicationRole>()
            //};
            //user1.Roles.Add(role2);
            //user1.Roles.Add(role3);
            //context.Roles.Add(role1);
            //context.Roles.Add(role2);
            //context.Roles.Add(role3);
            //context.Users.Add(user1);
        }
    }
}
