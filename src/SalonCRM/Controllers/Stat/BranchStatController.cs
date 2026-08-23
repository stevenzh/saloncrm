using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Manager;
using Common.Logging;

namespace SalonCRM.Controllers.Stat
{
    /// <summary>
    /// 门店排名
    /// </summary>
    public class BranchStatController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        ILog logger = LogManager.GetLogger("BranchStatController");

        // GET: BranchStat
        public ActionResult Index(BranchQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            qmodel.HostID = hostId;
            //初始化
            if (qmodel.StartDate == default(DateTime))
                qmodel.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (qmodel.EndDate == default(DateTime))
                qmodel.EndDate = DateTime.Today;


            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;

            qmodel.StatList = StatManager.GetBranchRankList(qmodel);
            return View(qmodel);
        }

        public ActionResult StatList(BranchQModel qmodel)
        {
            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;

            var mb = StatManager.GetBranchRankList(qmodel);
            return PartialView("StatList", mb);
        }
        public ActionResult Details(BranchQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;


            List<SelectListItem> items = new SelectList(dbcontent.Organs.Where(t => t.HostID == hostId).ToList(), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;

            // 业绩
            AccountRecordQModel model = new AccountRecordQModel { BranchId = qmodel.BranchID, StartDate = qmodel.StartDate, EndDate = qmodel.EndDate, Type = "1,2" };
            qmodel.IncomeList = new AccountRecordController().GetRecordList(model);

            // 卡扣
            AccountRecordQModel model3 = new AccountRecordQModel { BranchId = qmodel.BranchID, StartDate = qmodel.StartDate, EndDate = qmodel.EndDate, Type = "3" };
            qmodel.OutcomeList = new AccountRecordController().GetRecordList(model3);

            // 消耗
            BookQModel model2 = new BookQModel { BranchId = qmodel.BranchID, StartDate = qmodel.StartDate, EndDate= qmodel.EndDate };
            qmodel.ServiceList= new BookController().GetBookList(model2);

            return View(qmodel);
        }

        public ActionResult IncomeList(AccountRecordQModel viewModel)
        {
            ViewData["BranchId"] = viewModel.BranchId;
            ViewData["StartDate"] = viewModel.StartDate;
            ViewData["EndDate"] = viewModel.EndDate;
            //ViewData["Type"] = viewModel.Type;

            var RecordList = new AccountRecordController().GetRecordList(viewModel);
            return PartialView("IncomeList", RecordList);
        }

        public ActionResult OutcomeList(AccountRecordQModel viewModel)
        {
            ViewData["BranchId"] = viewModel.BranchId;
            ViewData["StartDate"] = viewModel.StartDate;
            ViewData["EndDate"] = viewModel.EndDate;
            //ViewData["Type"] = viewModel.Type;

            var RecordList = new AccountRecordController().GetRecordList(viewModel);
            return PartialView("OutcomeList", RecordList);
        }
    }
}