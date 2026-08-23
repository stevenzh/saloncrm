using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
using SalonCRM.Models;
using SalonBiz.Models.Stat;
using SalonCRM.Web;
using SalonCRM.Manager;
using SalonCRM.Identity;

namespace SalonCRM.Controllers.Stat
{
    public class CardStatController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        ILog logger = LogManager.GetLogger("CardStatController");
        // GET: CardStat
        public ActionResult Index(CardQModel qmodel)
        {
            qmodel.HostID = GlobalContext.Current.UserHost.HostID;
            CustomPrincipal cu = (CustomPrincipal)User;
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


            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["CardType"] = qmodel.CardType;

            qmodel.StatList = StatManager.GetCardStatList(qmodel);
            return View(qmodel);
        }


        public ActionResult StatList(CardQModel qmodel)
        {
            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["CardType"] = qmodel.CardType;

            var mb = StatManager.GetCardStatList(qmodel);
            return PartialView("StatList", mb);
        }

        public ActionResult Details(CardQModel qmodel)
        {
            qmodel.HostID = GlobalContext.Current.UserHost.HostID;
            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["CardType"] = qmodel.CardType;

            InitDrop();

            // 业绩
            qmodel.CardList = StatManager.GetCardList(qmodel);

            return View(qmodel);
        }

        public ActionResult CardList(CardQModel qmodel)
        {
            qmodel.HostID = GlobalContext.Current.UserHost.HostID;
            ViewData["BranchId"] = qmodel.BranchID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;
            ViewData["CardType"] = qmodel.CardType;
            InitDrop();

            var RecordList = StatManager.GetCardList(qmodel);
            return PartialView("CardList", RecordList);
        }


        public void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            List<SelectListItem> items = new SelectList(CommonManager.GetBranchs(hostId), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;

            var items6 = new SelectList(CommonManager.GetDictionaries("MemberCardType"), "KeyValue", "Contents").ToList();
            items6.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.CardTypeList = items6;

            List<SelectListItem> items2 = new SelectList(dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "MemberCardType").ToList(), "KeyValue", "Contents").ToList();
            ViewBag.CardTypeList = items2;

            List<SelectListItem> items1 = new SelectList(dbcontent.CardTmpls.Where(t => t.HostID == hostId && t.IsVaild == 1).ToList(), "TmplID", "Title").ToList();
            items1.Insert(0, new SelectListItem { Text = "无", Value = "" });
            ViewBag.CardTmplList = items1;
        }

    }
}