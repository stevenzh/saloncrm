using SalonCRM.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace SalonCRM.Models
{
    public class DataContextInitilizer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            //ApplicationRole role1 = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "管理员" };
            //ApplicationRole role2 = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "美容师" };
            //ApplicationRole role3 = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "超级管理员" };

            ApplicationUser user1 = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                Email = "admin@ymail.com",
                Password = Security.ToEncrypt("888888"),
                IsActive = true,
                HostId = 1,
                OrganId = 1,
                IsAdminUser = 1,
                CreateDate = DateTime.UtcNow,
                Roles = new List<ApplicationRole>()
            };
            //user1.Roles.Add(role1);
            //context.Roles.Add(role1);
            //context.Roles.Add(role2);
            //context.Roles.Add(role3);
            context.Users.Add(user1);
            context.SaveChanges();
        }
    }
}