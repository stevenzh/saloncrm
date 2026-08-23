using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Manager;
using SalonCRM.Web;

namespace SalonCRM.Controllers.Stat
{
    /// <summary>
    /// 货品统计
    /// </summary>
    public class GoodsStatController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: GoodsStat
        public ActionResult Index(GoodsQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            qmodel.HostID = hostId;
            //初始化
            if (qmodel.StartDate == default(DateTime))
                qmodel.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (qmodel.EndDate == default(DateTime))
                qmodel.EndDate = DateTime.Today;

            InitDrop();

            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["BrandCode"] = qmodel.BrandCode;
            ViewData["Category"] = qmodel.Category;
            ViewData["ProjectID"] = qmodel.ProjectID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;

            qmodel.GoodsList = StatManager.GetGoods(qmodel);
            return View(qmodel);
        }

        public ActionResult GoodsList(GoodsQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            qmodel.HostID = hostId;
            ViewData["BranchID"] = qmodel.BranchID;
            ViewData["BrandCode"] = qmodel.BrandCode;
            ViewData["Category"] = qmodel.Category;
            ViewData["ProjectID"] = qmodel.ProjectID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;

            InitDrop();
            var mb = StatManager.GetGoods(qmodel);
            return PartialView("PageList", mb);
        }

        public ActionResult ProjectList(GoodsQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            qmodel.HostID = hostId;
            ViewData["GoodsID"] = qmodel.GoodsID;
            ViewData["StartDate"] = qmodel.StartDate;
            ViewData["EndDate"] = qmodel.EndDate;

            var d = qmodel.EndDate.AddDays(1);

            var list = (from bg in dbcontent.BookGoods.Where(t => t.GoodsID == qmodel.GoodsID && t.BookProject.Book.State == "20"
                       && t.BookProject.Book.CreatedDate > qmodel.StartDate && t.BookProject.Book.CreatedDate < d)
                        select new GoodStatProjectModel
                        {
                            BookGoodsID = bg.BookGoodsID,
                            CreatedDate = bg.BookProject.Book.CreatedDate,
                            MemberName = bg.BookProject.Book.Member.Name,
                            Num = bg.Quantity,
                            ProjectName = bg.BookProject.Project.Name,
                            Quantity = bg.BookProject.Quantity,
                            Unit = bg.Goods.Unit
                        }).Union(
                from mpg in dbcontent.MemberProjectGoods.Where(t => t.GoodsID == qmodel.GoodsID
                      && t.MemberProject.CreatedDate > qmodel.StartDate && t.MemberProject.CreatedDate < d)
                select new GoodStatProjectModel
                {
                    BookGoodsID = mpg.MemberProjectGoodsID,
                    CreatedDate = mpg.MemberProject.CreatedDate,
                    MemberName = mpg.MemberProject.Member.Name,
                    Num = mpg.Quantity,
                    ProjectName = mpg.MemberProject.Project.Name,
                    Quantity = mpg.MemberProject.BookTime,
                    Unit = mpg.Goods.Unit
                }).ToList();

            return PartialView("ProjectList", list);
        }


        public void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            List<SelectListItem> items = new SelectList(CommonManager.GetBranchs(hostId), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;


            List<SelectListItem> items1 = new SelectList(CommonManager.GetDictionaries("ProjectCategory"), "KeyValue", "Contents").ToList();
            items1.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.CategoryList = items1;

            List<SelectListItem> items2 = new SelectList(CommonManager.GetDictionaries(hostId, "ProjectBrand"), "KeyValue", "Contents").ToList();
            items2.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.BrandList = items2;


        }
    }
}