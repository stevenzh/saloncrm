using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Web;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 指标设置
    /// </summary>
    [Authorize(Roles = "管理员")]
    public class ObjectiveController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: Objective
        public ActionResult Index(ObjectiveQModel qmodel)
        {
            int branchId = GlobalContext.Current.UserDepartment.OrganID;

            List<SelectListItem> items = new SelectList(dbcontent.Organs.Where(t => t.HostID == GlobalContext.Current.UserHost.HostID).ToList(), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;

            List<SelectListItem> years = new List<SelectListItem>();
            List<SelectListItem> months = new List<SelectListItem>();
            for (var y = 2017; y <= DateTime.Now.Year; y++)
                years.Add(new SelectListItem { Value = y.ToString(), Text = y.ToString() });

            for (var y = 1; y < 13; y++)
                months.Add(new SelectListItem { Value = y.ToString(), Text = y.ToString() });
            ViewBag.Years = years;
            ViewBag.Months = months;

            if (qmodel.DYear == default(int))
            {
                qmodel.DYear = DateTime.Now.Year;
                qmodel.DMonth = DateTime.Now.Month;
            }

            ViewData["BranchId"] = qmodel.BranchId;
            ViewData["DYear"] = qmodel.DYear;
            ViewData["DMonth"] = qmodel.DMonth;
            qmodel.ObjectiveList = GetList(qmodel.BranchId, qmodel.DYear, qmodel.DMonth);
            return View(qmodel);
        }

        public ActionResult TreePartial(int BranchId, int DYear, int DMonth)
        {
            ViewData["BranchId"] = BranchId;
            ViewData["DYear"] = DYear;
            ViewData["DMonth"] = DMonth;
            return PartialView("TreePartial", GetList(BranchId, DYear, DMonth));
        }

        private List<ObjectiveViewModel> GetList(int branchId, int year, int month)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var bt = dbcontent.Organs.Where(a => a.HostID == hostId);
            var bm = dbcontent.Users.Where(a => a.HostId == hostId).Where(a => a.Type == "1" || a.Type == "3");
            if (branchId != default(int))
            {
                bt = bt.Where(t => t.OrganID == branchId);
                bm = bm.Where(t => t.OrganId == branchId);
            }
            var result = (from a in bt
                          join b in dbcontent.Objectives.Where(t => t.Year == year && t.Month == month && t.Level == 1) on a.OrganID equals b.OrganId into result1
                          from f in result1.DefaultIfEmpty()
                          select new ObjectiveViewModel
                          {
                              ObjectiveId = f.ObjectiveId,
                              ID = a.OrganID.ToString(),
                              OrganId = a.OrganID,
                              OrganName = a.Name,
                              Level = 1,
                              Year = year,
                              Month = month,
                              Accounts = f.Accounts,
                              TopObjective = f.TopObjective,
                              SalesObjective = f.SalesObjective,
                              ServiceObjective = f.ServiceObjective,
                              ParentID = null
                          }).Union(
              from a in bm
              join b in dbcontent.Objectives.Where(t => t.Year == year && t.Month == month && t.Level == 3) on a.Id equals b.UserId into result1
              from f in result1.DefaultIfEmpty()
              select new ObjectiveViewModel
              {
                  ObjectiveId = f.ObjectiveId,
                  ID = a.Id,
                  OrganId = a.OrganId,
                  OrganName = a.UserCnName,
                  Level = 3,
                  Year = year,
                  Month = month,
                  Accounts = f.Accounts,
                  TopObjective = f.TopObjective,
                  SalesObjective = f.SalesObjective,
                  ServiceObjective = f.ServiceObjective,
                  ParentID = a.OrganId.ToString()
              }).ToList();

            return result;
        }


        [HttpPost]
        public ActionResult UpdateObjectivePartial(ObjectiveViewModel model, int BranchId, int DYear, int DMonth)
        {

            if (string.IsNullOrEmpty(model.ParentID))
            {
                int organId = Convert.ToInt32(model.ID);
                // 门店
                var entity = dbcontent.Objectives.Where(t => t.Year == model.Year && t.Month == model.Month && t.OrganId == organId && t.Level == 1).FirstOrDefault();

                if (entity != null)
                {
                    entity.Accounts = model.Accounts.Value;
                    entity.TopObjective = model.TopObjective.Value;
                    entity.ServiceObjective = model.ServiceObjective.Value;
                    entity.SalesObjective = model.SalesObjective.Value;
                }
                else
                {
                    Objective ob = new Objective();
                    ob.Year = model.Year;
                    ob.Month = model.Month;
                    ob.Level = 1;
                    ob.OrganId = Convert.ToInt32(model.ID);
                    ob.Accounts = model.Accounts.Value;
                    ob.TopObjective = model.TopObjective.Value;
                    ob.SalesObjective = model.SalesObjective.Value;
                    ob.ServiceObjective = model.ServiceObjective.Value;
                    dbcontent.Objectives.Add(ob);
                }
            }
            else
            {
                // 服务人员
                var entity = dbcontent.Objectives.Where(t => t.Year == model.Year && t.Month == model.Month && t.UserId == model.ID).FirstOrDefault();

                if (entity != null)
                {
                    entity.Accounts = model.Accounts.Value;
                    entity.TopObjective = model.TopObjective.Value;
                    entity.SalesObjective = model.SalesObjective.Value;
                    entity.ServiceObjective = model.ServiceObjective.Value;
                }
                else
                {
                    Objective ob = new Objective();
                    ob.Year = model.Year;
                    ob.Month = model.Month;
                    ob.Level = 3;
                    ob.OrganId = Convert.ToInt32(model.ParentID);
                    ob.UserId = model.ID;
                    ob.Accounts = model.Accounts.Value;
                    ob.TopObjective = model.TopObjective.Value;
                    ob.SalesObjective = model.SalesObjective.Value;
                    ob.ServiceObjective = model.ServiceObjective.Value;
                    dbcontent.Objectives.Add(ob);
                }
            }

            dbcontent.SaveChanges();

            ViewData["BranchId"] = BranchId;
            ViewData["DYear"] = DYear;
            ViewData["DMonth"] = DMonth;
            return PartialView("TreePartial", GetList(BranchId, model.Year, model.Month));
        }

    }
}
