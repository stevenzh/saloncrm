using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalonCRM.Manager;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Identity;

namespace SalonCRM.Controllers.Stat
{
    /// <summary>
    /// 员工统计
    /// </summary>
    public class WorkerStatController : Controller
    {
        // GET: WorkerStat
        public ActionResult Index(WorkerQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            CustomPrincipal cu = (CustomPrincipal)User;
            if (cu.Type != "2")
            {
                qmodel.BranchID = GlobalContext.Current.UserDepartment.OrganID;
            }

            if (qmodel.StartDate == default(DateTime))
                qmodel.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (qmodel.EndDate == default(DateTime))
                qmodel.EndDate = DateTime.Today;

            InitDrop();
            qmodel.HostID = hostId;
            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["WorkerType"] = qmodel.WorkerType;
            ViewData["WorkerName"] = qmodel.WorkerName;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["MemberType"] = qmodel.MemberType;
            ViewData["MemberNewType"] = qmodel.MemberNewType;
            ViewData["Sort"] = qmodel.Sort;

            qmodel.WorkerList = StatManager.WorkServiceRnking(qmodel);
            return View(qmodel);
        }

        public ActionResult PageList(WorkerQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            qmodel.HostID = hostId;
            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["WorkerType"] = qmodel.WorkerType;
            ViewData["WorkerName"] = qmodel.WorkerName;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["MemberType"] = qmodel.MemberType;
            ViewData["MemberNewType"] = qmodel.MemberNewType;
            ViewData["Sort"] = qmodel.Sort;

            var mb = StatManager.WorkServiceRnking(qmodel);
            return PartialView("PageList", mb);
        }

        //public ActionResult Detail(WorkerQModel model )
        //{
        //    return View();
        //}

        public void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            List<SelectListItem> items = new SelectList(CommonManager.GetBranchs(hostId), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;

        }
    }
}