using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Manager;
using SalonCRM.Tools;

namespace SalonCRM.Controllers
{
    [Authorize(Roles = "管理员")]
    public class UsersAdminController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        //
        // GET: /Users/
        public ActionResult Index(UserAdminViewModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            ViewData["UserName"] = viewModel.UserName;
            ViewData["BranchId"] = viewModel.BranchId;
            if (viewModel.HostId == default(int))
            {
                viewModel.HostId = hostId;
            }
            if (User.IsInRole("超级管理员"))
            {
                ViewBag.HostList = new SelectList(dbcontent.Hosts.ToList(), "HostID", "Name");
            }
            viewModel.UserList = GetUserList(viewModel);
            List<SelectListItem> items = new SelectList(dbcontent.Organs.Where(t => t.HostID == viewModel.HostId).ToList(), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;

            return View(viewModel);
        }

        public ActionResult PageList(UserAdminViewModel viewModel)
        {
            ViewData["UserName"] = viewModel.UserName;
            ViewData["BranchId"] = viewModel.BranchId;
            ViewData["HostId"] = viewModel.HostId;
            List<EditUserViewModel> UserList = GetUserList(viewModel);
            return PartialView("PageList", UserList);
        }
        public ActionResult DevBranchList(UserAdminViewModel model)
        {
            ViewBag.OrganId = new SelectList(dbcontent.Organs.Where(t => t.HostID == model.HostId).ToList(), "OrganID", "Name");
            //if (hostId != viewModel.HostId)
            //{
            //    viewModel.BranchId = null;
            //}
            return PartialView("DevBranchList", model);
        }

        private List<EditUserViewModel> GetUserList(UserAdminViewModel vmodel)
        {
            var query = dbcontent.Users.Where(a => a.HostId == vmodel.HostId && a.Type == "2");
            if (!string.IsNullOrEmpty(vmodel.UserName))
                query = query.Where(t => t.UserName == vmodel.UserName);
            if (vmodel.BranchId != default(int))
                query = query.Where(t => t.OrganId == vmodel.BranchId);
            if (!string.IsNullOrEmpty(vmodel.Status))
                query = query.Where(t => t.Status == vmodel.Status);
            return (from d in query.OrderBy(a => a.Id)
                    select new EditUserViewModel
                    {
                        Id = d.Id,
                        UserName = d.UserName,
                        Email = d.Email,
                        OrganId = d.OrganId,
                        OrganName = dbcontent.Organs.Where(t => t.OrganID == d.OrganId).FirstOrDefault().Name,
                        UserCnName = d.UserCnName
                    }).ToList();
        }
        //
        // GET: /Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);

            ViewBag.RoleNames = UserManager.GetRoles(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        public ActionResult Create()
        {
            InitDrop();
            EditUserViewModel model = new EditUserViewModel();
            if (User.IsInRole("超级管理员"))
            {
                model.HostId = GlobalContext.Current.UserHost.HostID;
            }
            return View(model);
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public ActionResult Create(EditUserViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var u = UserManager.FindByName(userViewModel.UserName, userViewModel.HostId);
                if (u != null)
                {
                    InitDrop();
                    ModelState.AddModelError("UserName", "用户名已存在.");
                    return View();
                }

                // 重复验证 email,host+username
                if (!string.IsNullOrEmpty(userViewModel.Email))
                {
                    InitDrop();
                    var u2 = UserManager.FindByEmail(userViewModel.Email);
                    if (u2 != null)
                    {
                        ModelState.AddModelError("Email", "邮件地址已存在.");
                        return View();
                    }
                }

                string Password = Security.ToEncrypt(userViewModel.Password);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    Password = Password,
                    UserCnName = userViewModel.UserCnName,
                    OrganId = userViewModel.OrganId,
                    HostId = userViewModel.HostId,
                    Type = "2",
                    IsAdminUser = Convert.ToInt32(userViewModel.IsAdminUser),
                    Roles = new List<ApplicationRole>(),
                    IsActive = true,
                    Status = "1",
                    CreateDate = DateTime.Now
                };
                var adminresult = UserManager.Create(user);

                //Add User to the selected Roles 
                if (adminresult == 1)
                {
                    if (selectedRoles != null)
                    {
                        var result = UserManager.AddToRoles(user, selectedRoles);
                        if (result != 1)
                        {
                            InitDrop();
                            return View();
                        }
                    }
                }
                else
                {
                    InitDrop();
                    return View();
                }
                return RedirectToAction("Index");
            }
            InitDrop();
            return View();
        }

        /// <summary>
        /// 初始化下拉菜单
        /// </summary>
        private void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            //Get the list of Roles
            var rolequery = dbcontent.Roles.Where(t => t.HostID == hostId);
            ViewBag.RoleId = new SelectList(rolequery.ToList(), "Name", "Name");
            ViewBag.HostId = new SelectList(dbcontent.Hosts.ToList(), "HostID", "Name");
            ViewBag.OrganId = new SelectList(dbcontent.Organs.Where(t => t.HostID == GlobalContext.Current.UserHost.HostID).ToList(), "OrganID", "Name");
            //ViewBag.Category = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "UserCategory").ToList(), "KeyValue", "Contents");

            List<SelectListItem> items = new List<SelectListItem>();
            items.Insert(0, new SelectListItem { Value = "0", Text = "普通用户" });
            items.Insert(0, new SelectListItem { Value = "1", Text = "管理员" });
            items.Insert(0, new SelectListItem { Value = "2", Text = "超级管理员" });
            ViewBag.UserAdmin = items;
        }

        //
        // GET: /Users/Edit/1
        public ActionResult Edit(string id)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = UserManager.GetRoles(user.Id);
            var rolequery = dbcontent.Roles.Where(t => t.HostID == hostId);
            IEnumerable<SelectListItem> rolesList = rolequery.ToList().Select(x => new SelectListItem()
            {
                Selected = userRoles.Contains(x.Name),
                Text = x.Name,
                Value = x.Name
            });
            InitDrop();

            if (user.HostId != hostId)
                ViewBag.OrganId = new SelectList(dbcontent.Organs.Where(t => t.HostID == user.HostId).ToList(), "OrganID", "Name");

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                HostId = user.HostId,
                HostName = dbcontent.Hosts.Where(t => t.HostID == user.HostId).FirstOrDefault().Name,
                UserName = user.UserName,
                Email = user.Email,
                OrganId = user.OrganId,
                UserCnName = user.UserCnName,
                IsAdminUser = user.IsAdminUser.ToString(),
                RolesList = rolesList
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel editUser, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                //UserName 检查重复
                int u = UserManager.CheckByName(editUser.Id, editUser.UserName, editUser.HostId);
                if (u != 0)
                {
                    InitDrop();
                    ModelState.AddModelError("UserName", "用户名已存在.");
                    return View();
                }

                // email重复验证 
                int u2 = UserManager.CheckByEmail(editUser.Id, editUser.Email);
                if (u2 != 0)
                {
                    InitDrop();
                    ModelState.AddModelError("Email", "邮件地址已存在.");
                    return View();
                }

                // 设置密码
                if (!string.IsNullOrEmpty(editUser.Password))
                {
                    string Password = Security.ToEncrypt(editUser.Password);
                    user.Password = Password;
                }

                user.UserName = editUser.UserName;
                user.Email = editUser.Email;
                user.UserCnName = editUser.UserCnName;
                user.OrganId = editUser.OrganId;
                user.Type = "2";
                user.IsAdminUser = Convert.ToInt32(editUser.IsAdminUser);

                var userRoles = UserManager.GetRoles(user.Id);
                selectedRoles = selectedRoles ?? new string[] { };
                // 新加角色保存
                var result = UserManager.AddToRoles(user, selectedRoles.Except(userRoles).ToArray<string>());
                if (result != 1)
                {
                    InitDrop();
                    ModelState.AddModelError("", "角色设置错误.");
                    return View();
                }
                // 舍弃角色保存
                result = UserManager.RemoveFromRoles(user, userRoles.Except(selectedRoles).ToArray<string>());
                if (result != 1)
                {
                    InitDrop();
                    ModelState.AddModelError("", "角色设置错误.");
                    return View();
                }

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "表单字段验证错误.");
            InitDrop();
            return View();
        }

        //
        // GET: /Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = UserManager.FindById(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = UserManager.Delete(user);
                if (result != 1)
                {
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        /// 终端取得美容师列表
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="type">0 所有顾问和美容师，其他正常</param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppListWorkers(int hostId, int branchId, string type)
        {
            var query = dbcontent.Users.Where(a => a.OrganId == branchId && a.Status == "1");
            if (type == "0")
            {
                query = query.Where(t => t.Type == "1" || t.Type == "3");
            }
            else
            {
                query = query.Where(t => t.Type == type);
            }
            var list = (from temp in query.ToList()
                        select new SelectListItem
                        {
                            Text = temp.Id,
                            Value = temp.UserCnName
                        }).ToList();
            return Json(list);
        }
    }
}
