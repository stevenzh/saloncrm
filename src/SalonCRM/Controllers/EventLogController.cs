using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Web;
using EntityFramework.Extensions;
using SalonCRM.Reports;

namespace SalonCRM.Controllers
{
    [Authorize]
    public class EventLogController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: EventLog
        public ActionResult Index(EventLogQModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            if (viewModel.HostID == default(int))
            {
                viewModel.HostID = hostId;
            }
            if (User.IsInRole("超级管理员"))
            {
                ViewBag.HostList = new SelectList(dbcontent.Hosts.ToList(), "HostID", "Name");
            }
            if (viewModel.StartDate == default(DateTime))
                viewModel.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (viewModel.EndDate == default(DateTime))
                viewModel.EndDate = DateTime.Today;

            ViewData["HostID"] = viewModel.HostID;
            ViewData["TypeID"] = viewModel.TypeID;
            ViewData["StartDate"] = viewModel.StartDate;
            ViewData["EndDate"] = viewModel.EndDate;

            InitDrop();
            viewModel.LogList = GetList(viewModel);
            return View(viewModel);
        }

        public ActionResult LogList(EventLogQModel viewModel)
        {
            ViewData["HostID"] = viewModel.HostID;
            ViewData["TypeID"] = viewModel.TypeID;
            ViewData["StartDate"] = viewModel.StartDate;
            ViewData["EndDate"] = viewModel.EndDate;
            InitDrop();
            return PartialView("LogList", GetList(viewModel));
        }

        private List<EventLogModel> GetList(EventLogQModel viewModel)
        {
            var query = dbcontent.EventLogs.AsQueryable();
            if (viewModel.HostID != default(int))
            {
                query = query.Where(t => t.HostId == viewModel.HostID);
            }
            if (viewModel.TypeID != default(int))
            {
                query = query.Where(t => t.TypeId == viewModel.TypeID);
            }
            if (viewModel.StartDate != default(DateTime))
                query = query.Where(t => t.CreatedDate > viewModel.StartDate);
            if (viewModel.EndDate != default(DateTime))
            {
                var d = viewModel.EndDate.AddDays(1);
                query = query.Where(t => t.CreatedDate < d);
            }

            var mb = (from v in query
                      join h in dbcontent.Hosts on v.HostId equals h.HostID
                      join o in dbcontent.Organs on v.BranchId equals o.OrganID
                      join e in dbcontent.Users on v.UserId equals e.Id
                      select new EventLogModel
                      {
                          LogId = v.LogId,
                          BranchId = v.BranchId,
                          BranchName = o.Name,
                          Content = v.Content,
                          CreatedDate = v.CreatedDate,
                          HostId = v.HostId,
                          HostName = h.Name,
                          Level = v.Level,
                          MemberId = v.MemberId,
                          Shell = v.Shell,
                          TypeId = v.TypeId,
                          UserId = v.UserId,
                          UserName = e.UserCnName,
                          ClientId = v.ClientId,
                          Member = (v.MemberId == null ? null : dbcontent.Members.Where(t => t.MemberID == v.MemberId).FirstOrDefault())
                      }).OrderByDescending(t => t.CreatedDate).ToList();
            return mb;
        }


        // GET: EvengLog/Details/5
        public ActionResult Details(long id)
        {
            ViewData["EventLogID"] = id;
            EventLogQModel model = new EventLogQModel();
            model.EventLog = dbcontent.EventLogs.Where(t => t.LogId == id).FirstOrDefault();
            model.RecordList = GetRecordList(id);
            model.CardList = dbcontent.MemberCards.Where(t => t.LogId == id).ToList();

            return View(model);
        }

        public ActionResult RecordList(long EventLogID)
        {
            ViewData["EventLogID"] = EventLogID;
            return PartialView("RecordList", GetRecordList(EventLogID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<AccountRecordModel> GetRecordList(long id)
        {
            IList<DictionaryExt> payList = new List<DictionaryExt>();
            payList.Add(new DictionaryExt("1", "现金", 0));
            payList.Add(new DictionaryExt("2", "刷卡", 1));
            payList.Add(new DictionaryExt("3", "转账", 1));
            payList.Add(new DictionaryExt("4", "储值卡", 0));


            var RecordList = (from ll in dbcontent.AccountRecords.Where(t => t.EventLogId == id)
                              join m in dbcontent.Members on ll.MemberID equals m.MemberID
                              join cc in dbcontent.MemberCards on ll.MemberCardId equals cc.MemberCardId
                              select new AccountRecordModel
                              {
                                  RecordID = ll.RecordID,
                                  Balance = ll.Balance,
                                  BookID = ll.BookID,
                                  Branch = ll.Branch,
                                  BranchId = ll.BranchId,
                                  ClientID = ll.ClientID,
                                  InAmount = ll.InAmount,
                                  OutAmount = ll.OutAmount,
                                  Debt = ll.Debt,
                                  PaymentType = ll.PaymentType,
                                  CreatedDate = ll.CreatedDate,
                                  Member = ll.Member,
                                  BeauticianId = ll.BeauticianID,
                                  SaleId = ll.SaleID,
                                  Salesman = dbcontent.Users.Where(t => t.Id == ll.SaleID).FirstOrDefault(),
                                  Splits = ll.Splits,
                                  Type = ll.Type,
                                  TypeValue = dbcontent.Dictionaries.Where(a => a.KeyValue == ll.Type && a.Identifier == "AccountRecordType").FirstOrDefault().Contents,
                                  Remark = ll.Remark,
                                  SalesType = ll.SalesType,
                                  IsVaild = ll.IsVaild,
                                  MemberCard = new MemberCardModel
                                  {
                                      MemberCardId = cc.MemberCardId,
                                      Title = cc.Title,
                                      TypeValue = dbcontent.Dictionaries.Where(a => a.KeyValue == cc.Type && a.Identifier == "MemberCardType").FirstOrDefault().Contents,
                                  },
                              }).OrderByDescending(t => t.RecordID).ToList();

            foreach (var bp in RecordList)
            {
                bp.SalesmanStr = String.Join(",", bp.Splits.Select(t => t.User.UserCnName).ToArray());
                if (!string.IsNullOrEmpty(bp.PaymentType))
                    bp.PaymentType = payList.Where(t => t.Key == bp.PaymentType).FirstOrDefault().Value;
            }

            return RecordList;
        }

        public ActionResult CardList(long EventLogID)
        {
            ViewData["EventLogID"] = EventLogID;
            var list = dbcontent.MemberCards.Where(t => t.LogId == EventLogID).ToList();
            InitDrop2();
            return PartialView("CardList", list);
        }
        public ActionResult ProjectList(long MemberCardId)
        {
            ViewData["MemberCardId"] = MemberCardId;
            var list = dbcontent.MemberProjects.Where(t => t.MemberCardId == MemberCardId).ToList();
            InitDrop3();
            return PartialView("ProjectList", list);
        }
        public ActionResult ProjectList1(long MemberCardId)
        {
            ViewData["MemberCardId"] = MemberCardId;
            var list = dbcontent.MemberCardProjects.Where(t => t.MemberCardId == MemberCardId).ToList();
            InitDrop3();
            return PartialView("ProjectList1", list);
        }

        private void InitDrop()
        {
            List<SelectListItem> items = new SelectList(dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "EventType").ToList(), "KeyValue", "Contents").ToList();
            ViewBag.EventTypeList = items;


            List<SelectListItem> items1 = new SelectList(dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "EventType").ToList(), "KeyValue", "Contents").ToList();
            items1.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.EventTypeSelectList = items1;
        }
        private void InitDrop2()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            List<SelectListItem> items = new SelectList(dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "MemberCardType").ToList(), "KeyValue", "Contents").ToList();
            ViewBag.CardTypeList = items;

            List<SelectListItem> items1 = new SelectList(dbcontent.CardTmpls.Where(t => t.HostID == hostId && t.IsVaild == 1).ToList(), "TmplID", "Title").ToList();
            items1.Insert(0, new SelectListItem { Text = "无", Value = "" });
            ViewBag.CardTmplList = items1;
        }
        private void InitDrop3()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            List<SelectListItem> items = new SelectList(dbcontent.Projects.Where(t => t.HostID == hostId).ToList(), "ProjectID", "Name").ToList();
            ViewBag.ProjectDataList = items;
        }


        // POST: Order/Delete/5
        [HttpPost]
        public ActionResult Delete(EventLogQModel model)
        {
            try
            {
                var log = dbcontent.EventLogs.Where(t => t.LogId == model.EventLog.LogId).FirstOrDefault();
                if (log.TypeId == 8)  // 购卡
                {
                    // 卡
                    var card = dbcontent.MemberCards.Where(t => t.LogId == log.LogId).FirstOrDefault();
                    card.Status = 0;

                    // 交易记录
                    var records = dbcontent.AccountRecords.Where(t => t.EventLogId == log.LogId).ToList();
                    foreach (var rc in records)
                    {
                        if (rc.Type == "4")
                        {
                            var fromCard = dbcontent.MemberCards.Where(t => t.MemberCardId == rc.MemberCardId).FirstOrDefault();
                            fromCard.Amt = fromCard.Amt + rc.OutAmount;
                        }
                        rc.IsVaild = 0;
                    }


                    var mps = dbcontent.MemberProjects.Where(t => t.LogId == log.LogId).Update(t => new MemberProject { IsVaild = 0 });

                    dbcontent.SaveChanges();

                    // 转账的负载
                }
                else if (log.TypeId == 9)  // 购买项目
                {
                    var records = dbcontent.AccountRecords.Where(t => t.EventLogId == log.LogId).ToList();
                    foreach (var rc in records)
                    {
                        var fromCard = dbcontent.MemberCards.Where(t => t.MemberCardId == rc.MemberCardId).FirstOrDefault();
                        fromCard.Amt = fromCard.Amt + rc.OutAmount;
                        rc.IsVaild = 0;
                    }

                    var mps = dbcontent.MemberProjects.Where(t => t.LogId == log.LogId).Update(t => new MemberProject { IsVaild = 0 });
                    dbcontent.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Print(long id)
        {
            EventLogQModel model = new EventLogQModel();
            model.EventLog = dbcontent.EventLogs.Where(t => t.LogId == id).FirstOrDefault();
            model.RecordList = GetRecordList(id).Where(t => t.Type == "2" || t.Type == "3" || t.Type == "4").ToList();
            model.CardList = dbcontent.MemberCards.Where(t => t.LogId == id).ToList();
            model.Member = dbcontent.Members.Where(t => t.MemberID == model.EventLog.MemberId).FirstOrDefault();
            model.Branch = dbcontent.Organs.Where(t => t.OrganID == model.EventLog.BranchId).FirstOrDefault();
            model.Member.MemberProjects = model.Member.MemberProjects.Where(t => t.LastCount > 0).ToList();

            if (model.EventLog.HostId == 11)
            {
                EventLogReport1 report = new EventLogReport1();
                report.DataSource = model;

                return View("PrintMBBC", report);
            }
            else
            {
                EventLogReport report = new EventLogReport();
                report.DataSource = model;
                return View("Details60", report);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="type"></param>
        /// <param name="date">YYYY-MM-DD</param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppDayList(int hostId, int branchId, string userId, string type, string date)
        {
            try
            {
                DateTime d = DateTime.Today;
                if (!string.IsNullOrEmpty(date))
                {
                    DateTime.TryParse(date, out d);
                }
                var entity = dbcontent.Users.Where(t => t.Id == userId).FirstOrDefault();
                var query = dbcontent.Members.Where(a => a.HostID == hostId);
                if (entity != null && (entity.Type == "3" || entity.Type == "1"))    //美容师 或 顾问 
                {
                    query = query.Where(t => t.SalesmanId == userId);
                }
                var app = dbcontent.EventLogs.AsQueryable();
                if (string.IsNullOrEmpty(type))
                    app = app.Where(a => a.TypeId == 8 || a.TypeId == 9);

                var list = (from _ in app
                            join m in query on _.MemberId equals m.MemberID
                            where _.HostId == hostId && _.BranchId == branchId
                                  && _.CreatedDate.Year == d.Year && _.CreatedDate.Month == d.Month && _.CreatedDate.Day == d.Day
                            select new EventLogModel
                            {
                                LogId = _.LogId,
                                Member = m,
                                MemberName = m.Name,
                                CreatedDate = _.CreatedDate,
                                TypeId = _.TypeId,
                                Content = _.Content
                            }).ToList();
                var count = list.Count();


                foreach (var item in list)
                {
                    var sales = (from ar in dbcontent.AccountRecords.Where(t => t.EventLogId == item.LogId)
                                 join mm in dbcontent.Users on ar.SaleID equals mm.Id
                                 select mm).FirstOrDefault();
                    if (sales != null)
                        item.Sales = sales.UserCnName;
                }

                var ll = (from v in list
                          select new
                          {
                              Id = v.LogId,
                              MemberName = v.Member.Name,
                              MemberId = v.Member.MemberID,
                              TypeID = v.TypeId,
                              Content = v.Content,
                              Sales = v.Sales,
                              CreatedDate = v.CreatedDate
                          }).ToList();

                var result = new
                {
                    count = count,
                    list = ll
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}