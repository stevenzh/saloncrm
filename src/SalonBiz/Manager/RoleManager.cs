using System;
using System.Linq;
using SalonCRM.Models;

namespace SalonCRM.Manager
{
    public class RoleManager
    {
        static ApplicationDbContext dbcontent = new ApplicationDbContext();

        public static ApplicationRole FindById(string id)
        {
            return dbcontent.Roles.Where(t => t.Id == id).FirstOrDefault();
        }

        public static int Create(ApplicationRole role)
        {
            role.Id = Guid.NewGuid().ToString();
            dbcontent.Roles.Add(role);
            dbcontent.SaveChanges();
            return 1;
        }

        public static void Update(ApplicationRole role)
        {
            var r = FindById(role.Id);
            r.HostID = role.HostID;
            r.Name = role.Name;
            r.Description = role.Description;
            r.Menus = role.Menus;
            dbcontent.SaveChanges();
        }

        public static int Delete(ApplicationRole role)
        {
            dbcontent.Roles.Remove(role);
            dbcontent.SaveChanges();
            return 1;
        }
    }
}