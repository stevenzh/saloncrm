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
    /// <summary>
    /// 员工管理
    /// </summary>
    [Authorize(Roles = "管理员")]
    public class WorkerController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        //
        // GET: /Users/
        public ActionResult Index(UserAdminViewModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            viewModel.UserList = GetWorkerList(viewModel);
            ViewData["UserName"] = viewModel.UserName;
            ViewData["BranchId"] = viewModel.BranchId;
            ViewData["DStatus"] = viewModel.Status;
            ViewData["DType"] = viewModel.Type;
            List<SelectListItem> items = new SelectList(dbcontent.Organs.Where(t => t.HostID == hostId).ToList(), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;

            List<SelectListItem> items1 = new List<SelectListItem>();
            items1.Add(new SelectListItem { Value = "0", Text = "--请选择--" });
            items1.Add(new SelectListItem { Value = "1", Text = "在职" });
            items1.Add(new SelectListItem { Value = "2", Text = "离职" });
            ViewBag.StatusList = items1;

            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem { Value = "", Text = "--请选择--" });
            items2.Add(new SelectListItem { Value = "1", Text = "美容师" });
            items2.Add(new SelectListItem { Value = "3", Text = "美容顾问" });
            items2.Add(new SelectListItem { Value = "4", Text = "店长" });
            ViewBag.TypeList = items2;

            return View(viewModel);
        }

        public ActionResult WorkerList(UserAdminViewModel viewModel)
        {
            ViewData["UserName"] = viewModel.UserName;
            ViewData["BranchId"] = viewModel.BranchId;
            ViewData["DStatus"] = viewModel.Status;
            ViewData["DType"] = viewModel.Type;
            List<EditUserViewModel> UserList = GetWorkerList(viewModel);
            return PartialView("WorkerList", UserList);
        }

        private List<EditUserViewModel> GetWorkerList(UserAdminViewModel vmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Users.Where(a => a.HostId == hostId && a.Type != "2");
            if (!string.IsNullOrEmpty(vmodel.UserName))
                query = query.Where(t => t.UserName == vmodel.UserName);
            if (vmodel.BranchId != default(int))
                query = query.Where(t => t.OrganId == vmodel.BranchId);
            if (!string.IsNullOrEmpty(vmodel.Status))
                query = query.Where(t => t.Status == vmodel.Status);
            if (!string.IsNullOrEmpty(vmodel.Type))
                query = query.Where(t => t.Type == vmodel.Type);

            return (from d in query.OrderBy(a => a.Id)
                    select new EditUserViewModel
                    {
                        Id = d.Id,
                        UserName = d.UserName,
                        Email = d.Email,
                        OrganId = d.OrganId,
                        OrganName = dbcontent.Organs.Where(t => t.OrganID == d.OrganId).FirstOrDefault().Name,
                        UserCnName = d.UserCnName,
                        Status = d.Status,
                        StatusValue = dbcontent.Dictionaries.Where(t => t.Identifier == "UserStatus" && t.KeyValue == d.Status).FirstOrDefault().Contents,
                        JoinDate = d.JoinDate,
                        Type = d.Type,
                        TypeValue = (d.Type == "1" ? "美容师" : (d.Type == "3" ? "顾问" : "店长"))
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
            WorkerViewModel model = new WorkerViewModel();
            model.HostId = GlobalContext.Current.UserHost.HostID;
            model.Password = "888888";
            return View(model);
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public ActionResult Create(WorkerViewModel userViewModel, params string[] selectedRoles)
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

                //// 重复验证 email,host+username
                //if (!string.IsNullOrEmpty(userViewModel.Email))
                //{
                //    InitDrop();
                //    var u2 = UserManager.FindByEmail(userViewModel.Email);
                //    if (u2 != null)
                //    {
                //        ModelState.AddModelError("Email", "邮件地址已存在.");
                //        return View();
                //    }
                //}

                string Password = Security.ToEncrypt(userViewModel.Password);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userViewModel.UserName,   // 员工登录用户名
                    Email = userViewModel.Email,
                    Password = Password,
                    UserCnName = userViewModel.UserCnName,
                    OrganId = userViewModel.OrganId,
                    HostId = userViewModel.HostId,
                    Position = userViewModel.Position,
                    MobileNumber = userViewModel.MobileNumber,
                    JoinDate = userViewModel.JoinDate,
                    ResignDate = userViewModel.ResignDate,
                    IsMajorOrgan = userViewModel.IsMajorOrgan,   // 管理员工 总部账号
                    Type = userViewModel.Type,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                    Status = "1",
                    Roles = new List<ApplicationRole>()
                };
                var adminresult = UserManager.Create(user);

                //Add User to the selected Roles 
                if (adminresult == 1)
                {
                    var result = UserManager.AddToRole(user, "美容师");
                    if (result != 1)
                    {
                        InitDrop();
                        return View();
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
            var rolequery = dbcontent.Roles.Where(t => t.HostID == hostId && t.IsMajor == false);
            ViewBag.RoleId = new SelectList(rolequery.ToList(), "Name", "Name");
            ViewBag.HostList = new SelectList(dbcontent.Hosts.ToList(), "HostID", "Name");
            ViewBag.OrganList = new SelectList(dbcontent.Organs.Where(t => t.HostID == GlobalContext.Current.UserHost.HostID).ToList(), "OrganID", "Name");

            List<SelectListItem> items1 = new List<SelectListItem>();
            items1.Add(new SelectListItem { Value = "1", Text = "在职" });
            items1.Add(new SelectListItem { Value = "2", Text = "离职" });
            ViewBag.StatusList = items1;

            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem { Value = "1", Text = "美容师" });
            items2.Add(new SelectListItem { Value = "3", Text = "顾问" });
            items2.Add(new SelectListItem { Value = "4", Text = "店长" });
            ViewBag.Types = items2;
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
            var rolequery = dbcontent.Roles.Where(t => t.HostID == hostId && t.IsMajor == false);
            IEnumerable<SelectListItem> rolesList = rolequery.ToList().Select(x => new SelectListItem()
            {
                Selected = userRoles.Contains(x.Name),
                Text = x.Name,
                Value = x.Name
            });
            InitDrop();
            return View(new WorkerViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                OrganId = user.OrganId,
                UserCnName = user.UserCnName,
                Position = user.Position,
                MobileNumber = user.MobileNumber,
                IsMajorOrgan = user.IsMajorOrgan,
                Status = user.Status,
                JoinDate = user.JoinDate,
                ResignDate = user.ResignDate,
                Type = user.Type,
                RolesList = rolesList
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkerViewModel editUser, params string[] selectedRoles)
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
                //int u2 = UserManager.CheckByEmail(editUser.Id, editUser.Email);
                //if (u2 != 0)
                //{
                //    InitDrop();
                //    ModelState.AddModelError("Email", "邮件地址已存在.");
                //    return View();
                //}

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
                user.Type = editUser.Type;
                user.JoinDate = editUser.JoinDate;
                user.ResignDate = editUser.ResignDate;
                user.MobileNumber = editUser.MobileNumber;
                user.IsMajorOrgan = editUser.IsMajorOrgan;
                user.Position = editUser.Position;
                user.Status = editUser.Status;

                dbcontent.SaveChanges();

                var userRoles = user.Roles.Select(t => t.Name);
                selectedRoles = selectedRoles ?? new string[] { };
                var result = UserManager.AddToRoles(user, selectedRoles.Except(userRoles).ToArray<string>());
                if (result != 1)
                {
                    InitDrop();
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
            ModelState.AddModelError("", "字段验证错误.");
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

    }
}
