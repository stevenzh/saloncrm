using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 系统设置【列表维护、会员分类、指标设置、卡管理】
    /// </summary>
     [Authorize(Roles = "管理员")]
    public class SystemController : Controller
    {
        // GET: System
        public ActionResult Index()
        {
            return View();
        }
    }
}