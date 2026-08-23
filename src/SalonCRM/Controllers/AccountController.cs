using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Identity;
using SalonCRM.Tools;
using SalonCRM.Manager;
using Common.Logging;

namespace SalonCRM.Controllers
{

    /// <summary>
    /// 账户操作
    /// </summary>
    public class AccountController : Controller
    {
        ApplicationDbContext Context = new ApplicationDbContext();
        ILog logger = LogManager.GetLogger("AccountController");

        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "", string path = "")
        {
            string host = Consts.GetTestHost(Request.Url.Host);
            var platFrom = Context.Hosts.Where(t => t.Url == host).FirstOrDefault();
            ViewBag.ReturnUrl = returnUrl;

            LoginViewModel model = new LoginViewModel() { HostCode = platFrom.HostCode, UserName = "", Password = "" };
            GlobalContext.Current.SiteNav = path;
            if (path == "SN1")
            {
                GlobalContext.Current.SiteNavName = "运营系统";
                GlobalContext.Current.SiteNavNext = "SNN4";
            }
            if (path == "SN2") GlobalContext.Current.SiteNavName = "经营系统";
            if (path == "SN3") GlobalContext.Current.SiteNavName = "财务系统";
            return View(model);
        }

        /// <summary>
        /// 登陆验证
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                string Password = Security.ToEncrypt(model.Password);
                var user = (from u in Context.Users.Where(u => u.UserName == model.UserName && u.Password == Password && u.Status == "1")
                            join h in Context.Hosts.Where(t => t.HostCode == model.HostCode && t.IsVaild == 1) on u.HostId equals h.HostID
                            select u).FirstOrDefault();
                if (user != null)
                {
                    var roles = user.Roles.Select(m => m.Name).ToList();
                    if (user.IsAdminUser > 0) roles.Add("管理员");
                    if (user.IsAdminUser > 1) roles.Add("超级管理员");

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.HostId = user.HostId;
                    serializeModel.UserId = user.Id;
                    serializeModel.UserCnName = user.UserCnName;
                    serializeModel.IsAdminUser = user.IsAdminUser;
                    serializeModel.Type = user.Type;
                    serializeModel.roles = roles.ToArray();

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             user.UserName,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(15),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    // 保存到Session
                    GlobalContext.Current.UserInfo = user;
                    Host host = Context.Hosts.FirstOrDefault(a => a.HostID == user.HostId);
                    Organ department = Context.Organs.FirstOrDefault(a => a.OrganID == user.OrganId);
                    GlobalContext.Current.UserDepartment = department;
                    GlobalContext.Current.UserHost = host;
                    if (user.IsAdminUser > 0)
                        GlobalContext.Current.LoginUserFunctions = UserManager.GetAllMenus();
                    else
                        GlobalContext.Current.LoginUserFunctions = UserManager.GetMenus(user.Id);
                    // 加载用户功能 用于菜单


                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "用户名或密码错误");
            }

            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            if (GlobalContext.Current.SiteNav == "SN2")
                return RedirectToAction("Today", "Home");
            else
                return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Page", "Home", null);
        }

        /// <summary>
        /// 网页登录（测试用）
        /// </summary>
        /// <param name="host"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AjaxLogin(string host, string username, string password)
        {
            var result = new
            {
                code = 2,
                message = "用户名或密码错误."
            };

            string passwd = Security.ToEncrypt(password);
            var user = (from _ in Context.Users.Where(t => t.UserName == username && t.Password == passwd && t.Status == "1")  // 在职
                        join h in Context.Hosts.Where(t => t.HostCode == host && t.IsVaild == 1) on _.HostId equals h.HostID
                        join b in Context.Organs on _.OrganId equals b.OrganID
                        select new LoginResult
                        {
                            code = 1,
                            Id = _.Id,
                            HostId = _.HostId,
                            HostCode = h.HostCode,
                            OrganId = _.OrganId,
                            BranchName = b.Name,
                            Type = _.Type,
                            PercentageLock = "0",      // 占比
                            MajorPercentage = "0.6",   // 顾问占比
                            MajorBeauticianPercentage = "0.4",   // 美容师占比
                            BeauticianPercentage = "0"           // 辅助美容师占比
                        }).FirstOrDefault();
            if (user != null)
            {
                var pf = Context.HostProfiles.Where(t => t.HostID == user.HostId).ToDictionary(t => t.PropertyText, t => t.PropertyValue);
                user.MajorPercentage = pf.ContainsKey("MajorPercentage") ? pf["MajorPercentage"] : "";   // 顾问业绩占比
                user.BeauticianPercentage = pf.ContainsKey("BeauticianPercentage") ? pf["BeauticianPercentage"] : ""; // 辅助美容师业绩占比
                user.MajorBeauticianPercentage = pf.ContainsKey("MajorBeauticianPercentage") ? pf["MajorBeauticianPercentage"] : "";  // 主要美容师占比
                user.PercentageLock = pf.ContainsKey("Setting_PercentageLock") ? pf["Setting_PercentageLock"] : "";  // 主要美容师占比

                return Json(user);
            }

            return Json(result);
        }

        /// <summary>
        /// 终端登录
        /// </summary>
        /// <param name="host">商户CODE</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="uuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppLogin(string host, string username, string password, string uuid, string model)
        {
            var result = new
            {
                code = 2,
                message = string.Empty
            };
            string errmsg = "";
            string passwd = Security.ToEncrypt(password);
            var user = (from _ in Context.Users.Where(t => t.UserName == username && t.Password == passwd && t.Status == "1")
                        join h in Context.Hosts.Where(t => t.HostCode == host && t.IsVaild == 1) on _.HostId equals h.HostID
                        join b in Context.Organs on _.OrganId equals b.OrganID
                        select new LoginResult
                        {
                            code = 1,   // 返回值
                            Id = _.Id,
                            HostId = _.HostId,
                            HostCode = h.HostCode,
                            OrganId = _.OrganId,
                            BranchName = b.Name,
                            Type = _.Type,
                            ClientNum = h.ClientNum,
                            PercentageLock = "0",      // 占比
                            MajorPercentage = "0.6",   // 顾问占比
                            MajorBeauticianPercentage = "0.4",   // 美容师占比
                            BeauticianPercentage = "0"           // 辅助美容师占比
                        }).FirstOrDefault();
            if (user != null)
            {
                //var c = Context.Clients.Where(t => t.HostID == user.HostId && t.OrganID == user.OrganId && t.MobileGUID == uuid).FirstOrDefault();
                //if (c == null)
                //{
                //    // 现有设备
                //    int count = Context.Clients.Where(t => t.HostID == user.HostId && t.IsVaild == "1").Count();
                //    if (user.ClientNum > count)
                //    {
                //        // 添加设备
                //        Client client = new Client();
                //        client.HostID = user.HostId;
                //        client.OrganID = user.OrganId;
                //        client.IsVaild = "1";
                //        client.MobileGUID = uuid;
                //        client.MobileModel = model;
                //        client.UserId = user.Id;
                //        client.LastSignIn = DateTime.Now;
                //        Context.Clients.Add(client);
                //    }
                //    else
                //    {
                //        errmsg = "登录失败-终端数量限制";
                //    }
                //}
                //else
                //{
                //    c.LastSignIn = DateTime.Now;
                //}

                var pf = Context.HostProfiles.Where(t => t.HostID == user.HostId).ToDictionary(t => t.PropertyText, t => t.PropertyValue);
                user.MajorPercentage = pf.ContainsKey("MajorPercentage") ? pf["MajorPercentage"] : "";   // 顾问业绩占比
                user.BeauticianPercentage = pf.ContainsKey("BeauticianPercentage") ? pf["BeauticianPercentage"] : ""; // 辅助美容师业绩占比
                user.MajorBeauticianPercentage = pf.ContainsKey("MajorBeauticianPercentage") ? pf["MajorBeauticianPercentage"] : "";  // 主要美容师占比
                user.PercentageLock = pf.ContainsKey("Setting_PercentageLock") ? pf["Setting_PercentageLock"] : "";  // 主要美容师占比


                // 登陆日志
                var log = new EventLog
                {
                    BranchId = user.OrganId,
                    HostId = user.HostId,
                    TypeId = 6,
                    Level = 5,
                    UserId = user.Id,
                    Content = "终端登录 uuid:" + uuid + ",设备说明:" + model + errmsg,
                    CreatedDate = DateTime.Now
                };
                Context.EventLogs.Add(log);
                Context.SaveChanges();

                if (string.IsNullOrEmpty(errmsg))
                {
                    return Json(user);
                }
                else
                {
                    result = new
                    {
                        code = 2,
                        message = errmsg
                    };
                }
            }
            else
            {
                result = new
                {
                    code = 2,
                    message = "用户名或密码错误."
                };
            }

            return Json(result);
        }

    }

    class LoginResult
    {
        public int code { get; set; }
        public string Id { get; set; }
        public int HostId { get; set; }
        public string HostCode { get; set; }
        public int OrganId { get; set; }
        public string BranchName { get; set; }
        public int? ClientNum { get; set; }
        public string Type { get; set; }
        public string PercentageLock { get; set; }   // 占比
        public string MajorPercentage { get; set; }   // 顾问占比
        public string MajorBeauticianPercentage { get; set; } // 美容师占比
        public string BeauticianPercentage { get; set; }
    }
}