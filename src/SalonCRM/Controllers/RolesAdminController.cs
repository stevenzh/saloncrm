using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using SalonCRM.Models;
using SalonCRM.Manager;
using SalonCRM.Web;

namespace SalonCRM.Controllers
{
    [Authorize(Roles = "管理员")]
    public class RolesAdminController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        //
        // GET: /Roles/
        public ActionResult Index()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            return View(dbcontent.Roles.Where(t => t.HostID == hostId).ToList());
        }

        //
        // GET: /Roles/Details/5
        public ActionResult Details(string id)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = dbcontent.Roles.FirstOrDefault(t => t.Id == id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in dbcontent.Users.Where(t => t.HostId == hostId).ToList())
            {
                if (UserManager.IsInRole(user.Id, role.Id))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(RoleViewModel roleViewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            if (ModelState.IsValid)
            {
                var role = new ApplicationRole(roleViewModel.Name);
                role.HostID = hostId;
                role.IsMajor = roleViewModel.IsMajor;
                var roleresult = RoleManager.Create(role);
                if (roleresult != 1)
                {
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Roles/Edit/Admin
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = RoleManager.FindById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, HostID = role.HostID, IsMajor = role.IsMajor, Name = role.Name };
            ViewData["RoleId"] = id;
            return View(roleModel);
        }

        [AllowAnonymous]
        public ActionResult GetMenuList(string id)
        {
            var result = (from a in dbcontent.MenuItems.Where(t => t.IsActive == true).OrderBy(t => t.SortOrder)
                          select new RoleMenuViewModel
                          {
                              MenuId = a.Id,
                              ParentId = a.ParentId,
                              Name = a.Name
                          }).ToList();

            var re = dbcontent.Roles.Find(id).Menus.Select(t => t.Id).ToArray();
            foreach (RoleMenuViewModel v in result)
            {
                if (re.Contains(v.MenuId))
                    v.IsActive = true;
            }

            return Json(result);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = dbcontent.Roles.Where(t => t.Id == roleModel.Id).FirstOrDefault();
                role.Name = roleModel.Name;
                role.IsMajor = roleModel.IsMajor;
                var newMenus = (string.IsNullOrEmpty(roleModel.MenuItems)) ? new string[0] : roleModel.MenuItems.Split(',').ToArray();
                var oldMenus = role.Menus.Select(t => t.Id.ToString()).ToArray();

                foreach (string str in oldMenus)
                {
                    // 移除
                    int menuitem = Convert.ToInt32(str);
                    if (!newMenus.Contains(str))
                        role.Menus.Remove(dbcontent.MenuItems.Where(t => t.Id == menuitem).FirstOrDefault());
                }

                foreach (string str in newMenus)
                {
                    if (string.IsNullOrEmpty(str)) continue;

                    int menuitem = Convert.ToInt32(str);
                    if (!oldMenus.Contains(str))
                    {
                        role.Menus.Add(dbcontent.MenuItems.Where(t => t.Id == menuitem).FirstOrDefault());
                    }
                }

                dbcontent.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }



        //
        // GET: /Roles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = RoleManager.FindById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var role = RoleManager.FindById(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                int result;
                if (deleteUser != null)
                {
                    result = RoleManager.Delete(role);
                }
                else
                {
                    result = RoleManager.Delete(role);
                }
                if (result != 1)
                {
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
