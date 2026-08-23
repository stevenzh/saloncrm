using System;
using System.Linq;
using System.Web.Mvc;
using Common.Logging;
using SalonCRM.Models;
using SalonCRM.Identity;
using SalonCRM.Manager;
using SalonCRM.Web;
using System.Collections.Generic;

namespace SalonCRM.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        ILog logger = LogManager.GetLogger("HomeController");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Index(int ID = 0)
        {
            try
            {
                var bag = (HostContainerBag)this.RouteData.Values["tenant"];
                int hostId = bag.HostID;
                HomeIndexModel model = new HomeIndexModel();
                if (ID == 0)
                {
                    model.BranchID = GlobalContext.Current.UserDepartment.OrganID;
                }
                else
                {
                    model.BranchID = ID;
                }

                // 当前用户添加到Session
                if (GlobalContext.Current.UserInfo != null)
                {
                    string currentUserId = User.Identity.Name;
                    if (!String.IsNullOrEmpty(currentUserId))
                    {
                        CustomPrincipal cu = (CustomPrincipal)User;
                        ApplicationUser currentUser = dbcontent.Users.FirstOrDefault(x => x.Id == cu.UserId);
                        GlobalContext.Current.UserInfo = currentUser;
                        Organ department = dbcontent.Organs.FirstOrDefault(a => a.OrganID == currentUser.OrganId);
                        GlobalContext.Current.UserDepartment = department;
                        GlobalContext.Current.UserHost = department.Host;
                        hostId = currentUser.HostId;
                    }
                }
                //logger.Info("Current HostID:" + hostId);

                List<SelectListItem> items = new SelectList(dbcontent.Organs.Where(t => t.HostID == hostId).ToList(), "OrganID", "Name").ToList();
                ViewBag.OrganId = items;

                // 本月业绩指标
                //var n = dbcontent.Objectives.Where(t => t.Level == 1 && t.OrganId == model.BranchID && t.Year == DateTime.Now.Year && t.Month == DateTime.Now.Month).FirstOrDefault();
                //model.MonthObjective = n == null ? 0 : n.Accounts; // 月度目标

                // 本月业绩
                //var p = dbcontent.AccountRecords.Where(t => t.Type == "1" || t.Type == "2").Where(t => t.HostID == hostId && t.CreatedDate.Year == DateTime.Now.Year && t.CreatedDate.Month == DateTime.Now.Month);
                //model.MonthsIncome = p.Count() > 0 ? p.Sum(t => t.InAmount) : 0;

                // 本月销售
                //var s = dbcontent.MemberProjects.Where(t => t.HostID == hostId && t.CreatedDate.Year == DateTime.Now.Year && t.CreatedDate.Month == DateTime.Now.Month);
                //model.MonthSales = s.Count() > 0 ? s.Sum(t => t.Amount) : 0;

                // 本月消耗
                //var m = dbcontent.Books.Where(t => t.HostID == hostId && t.CreatedDate.Year == DateTime.Now.Year && t.CreatedDate.Month == DateTime.Now.Month);
                //model.MonthsService = m.Count() > 0 ? m.Sum(t => t.Amount) : 0;

                // 今日预约 到店客户
                model.MonthWvie = dbcontent.Appointments.Where(t => t.BranchId == model.BranchID && t.BookDate.Year == DateTime.Today.Year && t.BookDate.Month == DateTime.Today.Month && t.BookDate.Day == DateTime.Today.Day && t.BookStatus != "0").Count(); //完成
                var nextday = DateTime.Today.AddDays(1);

                // 明天预约人数
                model.NextDayAp = dbcontent.Appointments.Where(t => t.BranchId == model.BranchID && t.BookDate.Year == nextday.Year && t.BookDate.Month == nextday.Month && t.BookDate.Day == nextday.Day).Count();

                // 今天新会员
                model.NewMember = dbcontent.Members.Where(t => t.JoinBranch == model.BranchID && t.CreatedDate.Year == DateTime.Now.Year && t.CreatedDate.Month == DateTime.Now.Month && t.CreatedDate.Day == DateTime.Now.Day).Count();

                // 今日回访数量
                //model.TodayFeedback = dbcontent.Feedbacks.Where(t => t.BranchId == model.BranchID && t.CreatedDate.Year == DateTime.Today.Year && t.CreatedDate.Month == DateTime.Today.Month && t.CreatedDate.Day == DateTime.Now.Day).Count(); // 今日回访

                //// 当月消耗项目量
                //var e = dbcontent.BookProjects.Where(t => t.Book.BranchId == branchId && t.Book.CreatedDate.Year == DateTime.Now.Year && t.Book.CreatedDate.Month == DateTime.Now.Month);
                //model.MonthProject = e.Count() > 0 ? e.Sum(t => t.Quantity) : 0;

                //// 当天消耗项目量
                //var q = dbcontent.BookProjects.Where(t => t.Book.BranchId == branchId && t.Book.CreatedDate.Year == DateTime.Today.Year && t.Book.CreatedDate.Month == DateTime.Now.Month && t.Book.CreatedDate.Day == DateTime.Now.Day);
                //model.DayProject = q.Count() > 0 ? q.Sum(t => t.Quantity) : 0;

                // 当天储值卡购买
                var qc = dbcontent.MemberCards.Where(t => t.BranchID == model.BranchID && t.Type == "0" && t.CreatedDate.Year == DateTime.Today.Year && t.CreatedDate.Month == DateTime.Now.Month && t.CreatedDate.Day == DateTime.Now.Day);
                model.DayCardAmount = qc.Count() > 0 ? qc.Sum(t => t.Amount) : 0;

                // 当天储值卡购买
                var qd = dbcontent.MemberCards.Where(t => t.BranchID == model.BranchID && t.CreatedDate.Year == DateTime.Today.Year && t.CreatedDate.Month == DateTime.Now.Month && t.CreatedDate.Day == DateTime.Now.Day).Where(t => t.Type == "4" || t.Type == "5" || t.Type == "8");
                model.DayZhCardAmount = qd.Count() > 0 ? qd.Sum(t => t.Amount) : 0;

                // 客户总人数
                model.MemberCount = dbcontent.Members.Where(t => t.JoinBranch == model.BranchID && t.Type == "L02").Count();

                // 今天的统计 消费 销售 服务
                model.DayStat = StatManager.GetDailyList(model.BranchID, DateTime.Today, DateTime.Today).FirstOrDefault();

                var deStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                model.MonthStat = StatManager.GetMonthStat(model.BranchID, deStart, DateTime.Now);
                // 最近一个月过生日的客户
                model.BirtAlert = CommonManager.GetBirth(model.BranchID);
                // 今天的预约
                model.Appointment = dbcontent.Appointments.Where(t => t.BranchId == model.BranchID && t.BookDate.Year == DateTime.Today.Year && t.BookDate.Month == DateTime.Today.Month && t.BookDate.Day == DateTime.Now.Day).ToList();
                //model.MemberType = CommonManager.GetMemberType(branchId);
                model.MemberStatus = CommonManager.GetMemberStatus(hostId, model.BranchID);

                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error("", ex);
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Page()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /// <summary>
        /// 运营/客户 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Member()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            GlobalContext.Current.SiteNavNext = "SNN1";

            ViewBag.StatusList = (from v in dbcontent.Members.Where(t => t.HostID == hostId)
                                  group v by v.Status into vv
                                  select new KeyNameCountModel
                                  {
                                      Key = vv.Key,
                                      Name = dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberStatus" && t.KeyValue == vv.Key).FirstOrDefault().Contents,
                                      Count = vv.Count()
                                  }).ToList();

            ViewBag.LevelList = (from v in dbcontent.Members.Where(t => t.HostID == hostId)
                                 group v by v.Level into vv
                                 select new KeyNameCountModel
                                 {
                                     Key = vv.Key,
                                     Name = dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberLevel" && t.KeyValue == vv.Key).FirstOrDefault().Contents,
                                     Count = vv.Count()
                                 }).ToList();

            ViewBag.NewMemberCount = dbcontent.Members.Where(t => t.HostID == hostId && t.Type == "L02" && t.IsNew == 1).Count();
            ViewBag.OldMemberCount = dbcontent.Members.Where(t => t.HostID == hostId && t.Type == "L02" && t.IsNew == 0).Count();

            return View();
        }

        /// <summary>
        /// 运营/客户 客户临店次数
        /// </summary>
        /// <param name="qmodel"></param>
        /// <returns></returns>
        public ActionResult MemberStat(MemberQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            CustomPrincipal cu = (CustomPrincipal)User;
            if (cu.Type != "2")
            {
                qmodel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
            }

            if (qmodel.StartDate == default(DateTime)) qmodel.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (qmodel.EndDate == default(DateTime)) qmodel.EndDate = DateTime.Now;
            var q1 = dbcontent.Books.Where(t => t.HostID == hostId && t.CreatedDate > qmodel.StartDate && t.CreatedDate < qmodel.EndDate);
            var q2 = dbcontent.AccountRecords.Where(t => t.HostID == hostId && t.CreatedDate > qmodel.StartDate && t.CreatedDate < qmodel.EndDate);
            if (qmodel.BranchId != 0)
            {
                q1 = q1.Where(t => t.BranchId == qmodel.BranchId);
                q2 = q2.Where(t => t.BranchId == qmodel.BranchId);
            }
            // 人头数
            var query = q1.Select(t => t.MemberID).Union(q2.Select(t => t.MemberID)).Distinct();

            // 临店次数
            var query1 = q1.Select(t => new { member = t.MemberID, year = t.CreatedDate.Year, month = t.CreatedDate.Month, day = t.CreatedDate.Day })
                          .Union(q2.Select(t => new { member = t.MemberID, year = t.CreatedDate.Year, month = t.CreatedDate.Month, day = t.CreatedDate.Day }))
                          .Distinct();

            var statusList1 = (from v in dbcontent.Members.Where(t => t.HostID == hostId && t.Type == "L02")
                               join dd in query1 on v.MemberID equals dd.member
                               group v by v.Status into vv
                               select new KeyNameCountModel
                               {
                                   Key = vv.Key,
                                   Name = dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberStatus" && t.KeyValue == vv.Key).FirstOrDefault().Contents,
                                   Count = vv.Count()
                               }).ToList();

            var statusList = (from v in dbcontent.Members.Where(t => t.HostID == hostId && t.Type == "L02")
                              join dd in query on v.MemberID equals dd
                              group v by v.Status into vv
                              select new KeyNameCountModel
                              {
                                  Key = vv.Key,
                                  Name = dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberStatus" && t.KeyValue == vv.Key).FirstOrDefault().Contents,
                                  Count = vv.Count()
                              }).ToList();

            var levelList1 = (from v in dbcontent.Members.Where(t => t.HostID == hostId)
                              join dd in query1 on v.MemberID equals dd.member
                              group v by v.Level into vv
                              select new KeyNameCountModel
                              {
                                  Key = vv.Key,
                                  Name = dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberLevel" && t.KeyValue == vv.Key).FirstOrDefault().Contents,
                                  Count = vv.Count()
                              }).ToList();

            var levelList = (from v in dbcontent.Members.Where(t => t.HostID == hostId)
                             join dd in query on v.MemberID equals dd
                             group v by v.Level into vv
                             select new KeyNameCountModel
                             {
                                 Key = vv.Key,
                                 Name = dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberLevel" && t.KeyValue == vv.Key).FirstOrDefault().Contents,
                                 Count = vv.Count()
                             }).ToList();

            ViewBag.StatusList = (from aa in statusList
                                  join bb in statusList1 on aa.Key equals bb.Key
                                  select new KeyNameCountModel { Key = aa.Key, Name = aa.Name, Count = aa.Count, Count2 = bb.Count }).ToList();
            ViewBag.LevelList = (from aa in levelList
                                 join bb in levelList1 on aa.Key equals bb.Key
                                 select new KeyNameCountModel { Key = aa.Key, Name = aa.Name, Count = aa.Count, Count2 = bb.Count }).ToList();
            var noNumber = (from v in dbcontent.Members.Where(t => t.HostID == hostId && t.Type == "L01")
                            join dd in query on v.MemberID equals dd
                            select v).Count();
            var noNumber1 = (from v in dbcontent.Members.Where(t => t.HostID == hostId && t.Type == "L01")
                             join dd in query1 on v.MemberID equals dd.member
                             select v).Count();
            ViewBag.NoMember = noNumber;
            ViewBag.NoMember1 = noNumber1;

            ViewBag.AllMember = statusList.Sum(t => t.Count) + noNumber;

            var item = new SelectList(dbcontent.Organs.Where(t => t.HostID == hostId).ToList(), "OrganID", "Name").ToList();
            item.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganList = item;

            return View(qmodel);
        }

        /// <summary>
        /// 运营/员工 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Employee()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            ViewBag.BranchCount = dbcontent.Organs.Where(t => t.HostID == hostId && t.IsVaild == 1).Count();
            ViewBag.SalesCount = dbcontent.Users.Where(t => t.HostId == hostId && t.Type == "3" && t.Status == "1").Count();
            ViewBag.BeauticianCount = dbcontent.Users.Where(t => t.HostId == hostId && t.Type == "1" && t.Status == "1").Count();


            GlobalContext.Current.SiteNavNext = "SNN2";
            return View();
        }

        /// <summary>
        /// 运营/项目 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Project()
        {
            GlobalContext.Current.SiteNavNext = "SNN3";
            return View();
        }

        /// <summary>
        /// 运营/综合统计 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Stat()
        {
            GlobalContext.Current.SiteNavNext = "SNN4";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Files()
        {
            return View();
        }

        public ActionResult Today()
        {
            return View();
        }
    }
}
