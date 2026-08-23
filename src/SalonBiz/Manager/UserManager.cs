using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using Common.Logging;
using SalonCRM.Models;

namespace SalonCRM.Manager
{
    public class UserManager
    {
        static ApplicationDbContext dbcontent = new ApplicationDbContext();
        private static readonly ILog logger = LogManager.GetLogger(typeof(UserManager));
        public static ApplicationUser FindById(string id)
        {
            return dbcontent.Users.FirstOrDefault(t => t.Id == id);
        }

        public static int Delete(ApplicationUser user)
        {
            var u = FindById(user.Id);
            u.IsActive = false;
            dbcontent.SaveChanges();
            return 1;
        }

        public static String[] GetRoles(string userId)
        {
            var l = (from v in dbcontent.Users.Where(t => t.Id == userId).FirstOrDefault().Roles
                     select v.Name).ToArray();
            return l;
        }

        public static int AddToRoles(ApplicationUser user, string[] roles)
        {
            var s = (from v in user.Roles select v.Name).ToArray();
            foreach (var r in roles)
            {
                if (!s.Contains(r))
                    user.Roles.Add(dbcontent.Roles.Where(t => t.HostID == user.HostId).FirstOrDefault(t => t.Name == r));
            }

            dbcontent.SaveChanges();
            return 1;
        }

        public static bool IsInRole(string id, string roleId)
        {
            var l = (from v in dbcontent.Users.Where(t => t.Id == id).FirstOrDefault().Roles
                     select v.Id).ToArray();
            return l.Contains(roleId);
        }

        public static int Create(ApplicationUser user)
        {
            user.Id = Guid.NewGuid().ToString();
            dbcontent.Users.Add(user);
            dbcontent.SaveChanges();
            return 1;
        }

        public static int AddToRole(ApplicationUser u, string roleName)
        {
            try
            {
                var s = (from v in u.Roles select v.Name).ToArray();
                if (!s.Contains(roleName))
                    u.Roles.Add(dbcontent.Roles.FirstOrDefault(t => t.Name == roleName));

                dbcontent.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                logger.Error("Project Create failure.", ex);
                foreach (var eve in ex.EntityValidationErrors)
                {
                    logger.Error(eve.ValidationErrors.First().ErrorMessage);
                }
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 丢弃角色
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static int RemoveFromRoles(ApplicationUser user, string[] roles)
        {
            var s = (from v in user.Roles select v.Name).ToArray();
            foreach (var r in roles)
            {
                if (s.Contains(r))
                    user.Roles.Remove(user.Roles.FirstOrDefault(t => t.Name == r));
            }

            dbcontent.SaveChanges();
            return 1;
        }

        public static ApplicationUser FindByName(string un, int hostId)
        {
            return dbcontent.Users.Where(t => t.HostId == hostId && t.UserName == un).FirstOrDefault();
        }

        public static ApplicationUser FindByEmail(string mail)
        {
            return dbcontent.Users.Where(t => t.Email == mail).FirstOrDefault();
        }

        /// <summary>
        /// 检查用户名称重复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="hostId"></param>
        /// <returns></returns>
        public static int CheckByName(string id, string userName, int hostId)
        {
            var u = dbcontent.Users.Where(t => t.HostId == hostId && t.UserName == userName && t.Id != id).FirstOrDefault();
            if (u != null)
                return 1;
            return 0;
        }

        /// <summary>
        /// 检查信箱重复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static int CheckByEmail(string id, string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var u = dbcontent.Users.Where(t => t.Email == email && t.Id != id).FirstOrDefault();
                if (u != null)
                    return 1;
            }
            return 0;
        }

        public static IList<MenuViewModel> GetMenus(string userId)
        {
            IDictionary<int, MenuViewModel> result = new Dictionary<int, MenuViewModel>();
            var l = dbcontent.Users.Where(t => t.Id == userId).First().Roles;

            foreach (ApplicationRole t in l)
            {
                foreach (MenuItem d in t.Menus)
                {
                    if (!result.ContainsKey(d.Id))
                    {
                        result.Add(d.Id, new MenuViewModel
                        {
                            MenuId = d.Id,
                            Level = d.Level,
                            MenuPath = d.MenuPath,
                            Name = d.Name,
                            ParentId = d.ParentId,
                            SortOrder = d.SortOrder,
                            Icon = d.Icon,
                            SiteNav = d.SiteNav,
                            SiteNavNext = d.SiteNavNext,
                            IsActive = d.IsActive
                        });
                    }
                }
            }

            return result.Select(t => t.Value).ToList();
        }


        public static IList<MenuViewModel> GetAllMenus()
        {
            var result = (from d in dbcontent.MenuItems.Where(t => t.IsActive == true)
                          select new MenuViewModel
                          {
                              MenuId = d.Id,
                              Level = d.Level,
                              MenuPath = d.MenuPath,
                              Name = d.Name,
                              ParentId = d.ParentId,
                              SortOrder = d.SortOrder,
                              Icon = d.Icon,
                              SiteNav = d.SiteNav,
                              SiteNavNext = d.SiteNavNext,
                              IsActive = d.IsActive
                          }).ToList();
            return result;
        }
    }
}