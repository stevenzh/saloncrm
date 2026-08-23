using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SalonCRM.Manager;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Identity;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 项目相关 统计
    /// </summary>
    public class ProjectStatController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        /// <summary>
        /// 项目销售消耗统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(ProjectQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            CustomPrincipal cu = (CustomPrincipal)User;

            List<SelectListItem> items1 = new SelectList(CommonManager.GetDictionaries("ProjectCategory"), "KeyValue", "Contents").ToList();
            items1.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.CategoryList = items1;

            List<SelectListItem> items2 = new SelectList(CommonManager.GetDictionaries(hostId, "ProjectBrand"), "KeyValue", "Contents").ToList();
            items2.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.BrandList = items2;


            List<SelectListItem> items3 = new SelectList(CommonManager.GetDictionaries("ProjectExtCategory"), "KeyValue", "Contents").ToList();
            items3.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.ExtCategoryList = items3;

            if (cu.Type != "2")
            {
                qmodel.BranchID = GlobalContext.Current.UserDepartment.OrganID;
            }

            InitDrop();

            //初始化
            if (qmodel.StartDate == default(DateTime))
                qmodel.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (qmodel.EndDate == default(DateTime))
                qmodel.EndDate = DateTime.Today;
            //if (qmodel.BranchID == 0) 
            //   qmodel.BranchID = GlobalContext.Current.UserDepartment.OrganID;
            qmodel.HostID = hostId;
            qmodel.StatList = StatManager.GetProjectList(qmodel);

            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["BrandCode"] = qmodel.BrandCode;
            ViewData["Category"] = qmodel.Category;
            ViewData["ExtCategory"] = qmodel.ExtCategory;
            ViewData["ProjectName"] = qmodel.ProjectName;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["CardType"] = qmodel.CardType;

            return View(qmodel);
        }

        public ActionResult PageList(ProjectQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            qmodel.HostID = hostId;

            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["BrandCode"] = qmodel.BrandCode;
            ViewData["Category"] = qmodel.Category;
            ViewData["ExtCategory"] = qmodel.ExtCategory;
            ViewData["ProjectName"] = qmodel.ProjectName;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["CardType"] = qmodel.CardType;

            var mb = StatManager.GetProjectList(qmodel);
            return PartialView("PageList", mb);
        }

        /// <summary>
        /// 项目销售消耗明细
        /// </summary>
        /// <returns></returns>
        public ActionResult Details(ProjectQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            qmodel.HostID = hostId;

            ViewData["ProjectID"] = qmodel.ProjectID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["CardType"] = qmodel.CardType;

            qmodel.ProjectName = dbcontent.Projects.Where(t => t.ProjectID == qmodel.ProjectID).FirstOrDefault().Name;
            // 销售 
            qmodel.SalesList = StatManager.GetProjectSelesList(qmodel);
            // 消耗
            qmodel.ServiceList = StatManager.GetProjectServiceList(qmodel);

            InitDrop();
            return View(qmodel);
        }

        public ActionResult ServiceList(ProjectQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            qmodel.HostID = hostId;

            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["ProjectID"] = qmodel.ProjectName;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["CardType"] = qmodel.CardType;

            var mb = StatManager.GetProjectServiceList(qmodel);
            return PartialView("ServiceList", mb);
        }

        public ActionResult SalesList(ProjectQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            qmodel.HostID = hostId;

            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["ProjectID"] = qmodel.ProjectName;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["CardType"] = qmodel.CardType;

            var mb = StatManager.GetProjectSelesList(qmodel);
            return PartialView("SalesList", mb);
        }

        public void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            List<SelectListItem> items = new SelectList(CommonManager.GetBranchs(hostId), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;


            List<SelectListItem> items4 = new SelectList(CommonManager.GetDictionaries("MemberCardType"), "KeyValue", "Contents").ToList();
            items4.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.CardTypeList = items4;
        }
    }
}