using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Transactions;
using EntityFramework.Extensions;
using Common.Logging;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Identity;
using SalonCRM.Manager;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 消耗操作
    /// 美容服务完成后提交
    /// </summary>
    [CustomAuthorize]
    public class BookController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        ILog logger = LogManager.GetLogger("BookController");

        // GET: Book
        public ActionResult Index(BookQModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            CustomPrincipal cu = (CustomPrincipal)User;

            List<SelectListItem> items = new SelectList(dbcontent.Organs.Where(t => t.HostID == hostId).ToList(), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;
            if (cu.Type == "1")   // 门店用户锁死
            {
                viewModel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
                viewModel.BeauticianId = GlobalContext.Current.UserInfo.Id;
            }
            else if (cu.Type == "3")
            {
                viewModel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
                viewModel.SalesId = GlobalContext.Current.UserInfo.Id;
            }
            else if (cu.Type == "4")
            {
                viewModel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
            }

            if (viewModel.StartDate == default(DateTime))
                viewModel.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (viewModel.EndDate == default(DateTime))
                viewModel.EndDate = DateTime.Today;

            // 顾问
            List<SelectListItem> items4 = new SelectList(dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type == "3" && t.Status == "1").ToList(), "Id", "UserCnName").ToList();
            items4.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.Salesman = items4;
            // 美容师
            List<SelectListItem> items5 = new SelectList(dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type == "1" && t.Status == "1").ToList(), "Id", "UserCnName").ToList();
            items5.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.Beautician = items5;

            viewModel.BookList = GetBookList(viewModel);
            return View(viewModel);
        }

        public ActionResult BookList(BookQModel viewModel)
        {
            ViewData["CardNo"] = viewModel.CardNo;
            ViewData["BranchId"] = viewModel.BranchId;
            ViewData["StartDate"] = viewModel.StartDate;
            ViewData["EndDate"] = viewModel.EndDate;
            ViewData["SalesId"] = viewModel.SalesId;
            ViewData["MemberName"] = viewModel.MemberName;
            ViewData["Category"] = viewModel.Category;
            ViewData["BeauticianId"] = viewModel.BeauticianId;

            var mb = GetBookList(viewModel);
            return PartialView("BookList", mb);
        }

        public List<BookModel> GetBookList(BookQModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Books.Where(t => t.HostID == hostId);
            if (!string.IsNullOrEmpty(viewModel.BeauticianId))
                query = dbcontent.BookProjectSplits.Where(t => t.UserID == viewModel.BeauticianId && t.BookProject.Book.HostID == hostId).Select(t => t.BookProject.Book);
            if (viewModel.BranchId != 0)
                query = query.Where(t => t.BranchId == viewModel.BranchId);
            if (!string.IsNullOrEmpty(viewModel.SalesId))
                query = query.Where(t => t.SalesmanID == viewModel.SalesId);
            if (!string.IsNullOrEmpty(viewModel.MemberName))
                query = query.Where(t => t.Member.Name.Contains(viewModel.MemberName));
            if (!string.IsNullOrEmpty(viewModel.CardNo))
                query = query.Where(t => t.Member.CardNo == viewModel.CardNo);
            if (viewModel.StartDate != default(DateTime))
                query = query.Where(t => t.CreatedDate > viewModel.StartDate);
            if (viewModel.EndDate != default(DateTime))
            {
                var d = viewModel.EndDate.AddDays(1);
                query = query.Where(t => t.CreatedDate < d);
            }

            var list = (from bb in query.OrderByDescending(t => t.CreatedDate)
                        select new BookModel
                        {
                            CreatedDate = bb.CreatedDate,
                            CreatedBy = bb.CreatedBy,
                            ClientID = bb.ClientID,
                            BookID = bb.BookID,
                            Amount = bb.Amount,
                            BranchId = bb.BranchId,
                            HostID = bb.HostID,
                            MemberID = bb.MemberID,
                            SalesmanID = bb.SalesmanID,
                            Satisfaction = bb.Satisfaction,
                            State = bb.State,
                            Remark = bb.Remark,
                            BookProjects = bb.BookProjects,
                            Member = bb.Member,
                            Branch = dbcontent.Organs.Where(t => t.OrganID == bb.BranchId).FirstOrDefault(),
                            StateValue = dbcontent.Dictionaries.Where(a => a.KeyValue == bb.State && a.Identifier == "BookState").FirstOrDefault().Contents,
                            Salesman = dbcontent.Users.Where(t => t.Id == bb.SalesmanID).FirstOrDefault()
                        }).ToList();

            return list;
        }

        // GET: Book/Details/5
        public ActionResult Details(long id)
        {
            var entity = (from bb in dbcontent.Books.Where(t => t.BookID == id)
                          select new BookModel
                          {
                              CreatedDate = bb.CreatedDate,
                              CreatedBy = bb.CreatedBy,
                              ClientID = bb.ClientID,
                              BookID = bb.BookID,
                              Amount = bb.Amount,
                              BranchId = bb.BranchId,
                              HostID = bb.HostID,
                              MemberID = bb.MemberID,
                              SalesmanID = bb.SalesmanID,
                              Salesman = dbcontent.Users.Where(t => t.Id == bb.SalesmanID).FirstOrDefault(),
                              Satisfaction = bb.Satisfaction,
                              State = bb.State,
                              Remark = bb.Remark,
                              BookProjects = bb.BookProjects,
                              Member = bb.Member,
                              Branch = dbcontent.Organs.Where(t => t.OrganID == bb.BranchId).FirstOrDefault(),
                              StateValue = dbcontent.Dictionaries.Where(a => a.KeyValue == bb.State && a.Identifier == "BookState").FirstOrDefault().Contents,
                          }).FirstOrDefault();

            foreach (var bp in entity.BookProjects)
            {
                bp.BeauticianId = String.Join(",", bp.UserSplits.Select(t => t.User.UserCnName).ToArray());
            }
            entity.Member.MemberProjects = entity.Member.MemberProjects.Where(t => t.LastCount > 0).ToList();

            if (entity.HostID == 11)
            {
                XtraReport2 report = new XtraReport2();
                BookReportModel model = new BookReportModel();
                model.BookList = new List<BookModel>();
                model.BookList.Add(entity);
                report.DataSource = model;

                return View("Details60MBBC", report);
            }
            else
            {
                XtraReport60 report = new XtraReport60();
                report.DataSource = entity;
                return View("Details60", report);
            }
        }

        // GET: Book/Edit/5
        public ActionResult Edit(long id)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var entity = (from bb in dbcontent.Books.Where(t => t.BookID == id)
                          select new BookModel
                          {
                              CreatedDate = bb.CreatedDate,
                              CreatedBy = bb.CreatedBy,
                              ClientID = bb.ClientID,
                              BookID = bb.BookID,
                              Amount = bb.Amount,
                              BranchId = bb.BranchId,
                              HostID = bb.HostID,
                              MemberID = bb.MemberID,
                              SalesmanID = bb.SalesmanID,
                              Satisfaction = bb.Satisfaction,
                              State = bb.State,
                              Remark = bb.Remark,
                              BookProjects = bb.BookProjects,
                              Member = bb.Member,
                              Branch = dbcontent.Organs.Where(t => t.OrganID == bb.BranchId).FirstOrDefault(),
                              StateValue = dbcontent.Dictionaries.Where(a => a.KeyValue == bb.State && a.Identifier == "BookState").FirstOrDefault().Contents,
                          }).FirstOrDefault();

            ViewData["BookID"] = id;
            // 顾问
            List<SelectListItem> items4 = new SelectList(dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type == "3" && t.Status == "1").ToList(), "Id", "UserCnName").ToList();
            items4.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.Salesman = items4;

            return View(entity);
        }

        // POST: Book/Edit/5
        [HttpPost]
        public ActionResult Edit(BookModel model)
        {
            try
            {
                var entity = dbcontent.Books.Where(t => t.BookID == model.BookID).FirstOrDefault();
                if (entity != null)
                {
                    entity.SalesmanID = model.SalesmanID;
                    entity.Amount = model.Amount;
                    entity.Remark = model.Remark;
                    dbcontent.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ProjectList(BookQModel viewModel)
        {
            ViewData["BookID"] = viewModel.BookID;

            var mb = dbcontent.BookProjects.Where(t => t.BookID == viewModel.BookID).ToList();
            return PartialView("ProjectList", mb);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProjectPartial(BookProject model)
        {
            ViewData["BookID"] = model.BookID;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.BookProjects.Where(t => t.BookProjectID == model.BookProjectID).FirstOrDefault();
                    m.Amount = model.Amount;
                    m.Quantity = model.Quantity;
                    m.HandicraftFee = model.HandicraftFee;
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop();
            var list = dbcontent.BookProjects.Where(t => t.BookID == model.BookID).ToList();
            return PartialView("ProjectList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteProjectPartial(BookProject model)
        {
            ViewData["BookID"] = model.BookProjectID;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.BookProjects.Where(t => t.BookProjectID == model.BookProjectID).FirstOrDefault();
                    if (m != null)
                    {
                        dbcontent.BookProjects.Remove(m);
                        dbcontent.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop();
            var list = dbcontent.BookProjects.Where(t => t.BookID == model.BookID).ToList();
            return PartialView("ProjectList", list);
        }

        #region 美容师设置
        public ActionResult SplitList(long BookProjectID)
        {
            ViewData["BookProjectID"] = BookProjectID;
            InitDrop();
            var list = dbcontent.BookProjectSplits.Where(t => t.BookProjectID == BookProjectID).ToList();
            return PartialView("SplitList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewSplitPartial(BookProjectSplit model)
        {
            ViewData["BookProjectID"] = model.BookProjectID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = new BookProjectSplit
                    {
                        BookProjectID = model.BookProjectID,
                        UserID = model.UserID,
                        Percentage = model.Percentage,
                        Amount = model.Amount,
                        ModifiedBy = userId,
                        ModifiedTime = DateTime.Now,
                        Position = model.Position
                    };

                    dbcontent.BookProjectSplits.Add(m);
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop();
            var list = dbcontent.BookProjectSplits.Where(t => t.BookProjectID == model.BookProjectID).ToList();
            return PartialView("SplitList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateSplitPartial(BookProjectSplit model)
        {
            ViewData["BookProjectID"] = model.BookProjectID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.BookProjectSplits.Where(t => t.SplitID == model.SplitID).FirstOrDefault();
                    m.Percentage = model.Percentage;
                    m.UserID = model.UserID;
                    m.Amount = model.Amount;
                    m.ModifiedBy = userId;
                    m.ModifiedTime = DateTime.Now;
                    m.Position = model.Position;
                    m.HandicraftFee = model.HandicraftFee;
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop();
            var list = dbcontent.BookProjectSplits.Where(t => t.BookProjectID == model.BookProjectID).ToList();
            return PartialView("SplitList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteSplitPartial(BookProjectSplit model)
        {
            ViewData["BookProjectID"] = model.BookProjectID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.BookProjectSplits.Where(t => t.SplitID == model.SplitID).FirstOrDefault();
                    if (m != null)
                    {
                        dbcontent.BookProjectSplits.Remove(m);
                        dbcontent.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop();
            var list = dbcontent.BookProjectSplits.Where(t => t.BookProjectID == model.BookProjectID).ToList();
            return PartialView("SplitList", list);
        }
        #endregion

        private void InitDrop()
        {
            ViewBag.Salesman = dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type != "2" && t.Status == "1").ToList();

            List<SelectListItem> l = new List<SelectListItem> {
                 new SelectListItem { Text="顾问", Value="1" },
                 new SelectListItem { Text ="美容师",Value="2"},
                 new SelectListItem { Text ="助理美容师",Value="3"}
            };
            ViewBag.Position = l;
        }

        [AllowAnonymous]
        public ActionResult AjaxCancel(string bookId)
        {
            var result = new
            {
                code = 2,
                message = "消耗单不存在."
            };

            long bid = Convert.ToInt32(bookId);
            var book = dbcontent.Books.Where(t => t.BookID == bid).FirstOrDefault();
            if (book != null)
            {
                if (book.State == "20")
                {
                    using (var dbtran = new TransactionScope(TransactionScopeOption.Required))
                    {
                        dbcontent.AccountRecords.Where(t => t.EventLogId == book.LogId).Update(t => new AccountRecord { IsVaild = 0 });
                        // 项目返还
                        foreach (var item in book.BookProjects)
                        {
                            var mp = dbcontent.MemberProjects.Where(t => t.MemberProjectId == item.MemberProjectId).FirstOrDefault();
                            var mc = dbcontent.MemberCards.Where(t => t.MemberCardId == item.MemberCardId).FirstOrDefault();
                            mp.UsedTime = mp.UsedTime - item.Quantity;
                            mp.LastCount = mp.LastCount + item.Quantity;
                            mc.UsedTime = mc.UsedTime - item.Quantity;
                            mc.LastCount = mc.LastCount + item.Quantity;
                        }
                        book.State = "30";

                        dbcontent.SaveChanges();
                        dbtran.Complete();
                    }
                }
                else
                {
                    book.State = "30";
                }
                dbcontent.SaveChanges();

                result = new
                {
                    code = 0,
                    message = "SUCCESS"
                };
            }

            return Json(result);
        }

        #region 终端使用

        /// <summary>
        /// 消费订单添加
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="memberId"></param>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <param name="books"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppCreate(int hostId, int branchId, int memberId, string clientId, string userId, string bookId, string SalesId, string books)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];

            var result = new
            {
                code = 1,
                message = string.Empty
            };

            try
            {
                using (var dbtran = new TransactionScope(TransactionScopeOption.Required))
                {
                    string[] st = books.Split(';');  // 多项目消费拆分
                    long currentBookId = default(long);
                    string ProjectNames = "";
                    EventLog eventLog = null;

                    if (!string.IsNullOrEmpty(bookId))
                    {
                        currentBookId = Convert.ToInt64(bookId);
                    }
                    else
                    {
                        // 操作任务
                        var log = new EventLog
                        {
                            HostId = hostId,
                            BranchId = branchId,
                            MemberId = memberId,
                            TypeId = 10,
                            UserId = userId,
                            ClientId = clientId,
                            CreatedDate = DateTime.Now,
                            Level = 5
                        };
                        eventLog = dbcontent.EventLogs.Add(log);
                        dbcontent.SaveChanges();

                        // 新增消耗记录
                        var book = new Book
                        {
                            HostID = hostId,
                            BranchId = branchId,
                            MemberID = memberId,
                            LogId = eventLog.LogId,
                            Amount = 0,
                            ClientID = clientId,
                            CreatedBy = userId,
                            State = "0",
                            CreatedDate = DateTime.Now,
                            SalesmanID = SalesId
                        };

                        dbcontent.Books.Add(book);
                        dbcontent.SaveChanges();
                        currentBookId = book.BookID;
                    }

                    decimal amount = 0;  // 消费总额

                    // 订单项目列表
                    foreach (string dd in st)
                    {
                        if (!string.IsNullOrEmpty(dd))
                        {
                            // 消费项目
                            string[] di = dd.Split(','); // [项目ID, [卡扣：false，现金：true]，数量，[美容师]，金额, MemberProjectId, MemberCardId,BookProjectId, IsEntity, PaymentType]
                            decimal pamount = 0;
                            int pjid = Convert.ToInt32(di[0]);
                            Project pj = dbcontent.Projects.Where(t => t.ProjectID == pjid).FirstOrDefault();
                            ProjectNames += pj.Name + ",";
                            long cardid = default(long);
                            decimal hf = pj.HandicraftFee * Convert.ToInt32(di[2]);

                            if (di[1] == "false")     // 卡扣疗程
                            {
                                long id = Convert.ToInt64(di[5]);
                                MemberProject mpp = dbcontent.MemberProjects.Where(a => a.MemberProjectId == id).FirstOrDefault();
                                cardid = mpp.MemberCardId.Value;
                                if (mpp.Type == "1")  // 赠送项目 手工费 使用最低费用
                                {
                                    hf = pj.LowHandicraftFee * Convert.ToInt32(di[2]);
                                }

                                if (mpp.Type == "1")
                                    pamount = Convert.ToDecimal(di[4]);
                                else
                                    pamount = mpp.UnitPrice * Convert.ToInt32(di[2]);
                            }
                            else                    // 现付购买
                            {
                                pamount = Convert.ToDecimal(di[4]);
                            }

                            long giveId = default(long);
                            int paymentType = Convert.ToInt32(di[9]);

                            // 服务人员
                            List<BookProjectSplit> us = new List<BookProjectSplit>();
                            string[] wo = di[3].Split('|');
                            int workernum = 0;
                            foreach (string works in wo)
                            {
                                if (!string.IsNullOrEmpty(works))
                                {
                                    workernum++;
                                }
                            }

                            foreach (string works in wo)
                            {
                                if (!string.IsNullOrEmpty(works))
                                {
                                    ApplicationUser u = dbcontent.Users.Where(a => a.Id == works).FirstOrDefault();
                                    if (u != null)
                                    {
                                        us.Add(new BookProjectSplit
                                        {
                                            UserID = works,
                                            Position = "2",
                                            Percentage = 1M / workernum,
                                            Amount = pamount / workernum,
                                            ModifiedTime = DateTime.Now,
                                            ModifiedBy = userId,
                                            HandicraftFee = hf / workernum
                                        });
                                    }
                                }
                            }

                            #region 现付项目
                            if (di[1] == "true")
                            {
                                if (paymentType == 4)  // 储值卡
                                {
                                    string[] cardtype = { "", "" };

                                    // 选择卡片
                                    cardtype = di[6].Split(':');
                                    cardid = Convert.ToInt64(cardtype[0]);  // 客户端选择
                                }
                                else
                                {
                                    // 现金通道
                                    var card = dbcontent.MemberCards.Where(t => t.MemberID == memberId && t.Type == "9").FirstOrDefault();
                                    if (card == null)
                                    {
                                        // 添加现金流通卡
                                        MemberCard card1 = new MemberCard
                                        {
                                            MemberID = memberId,
                                            HostID = hostId,
                                            BranchID = branchId,
                                            LogId = eventLog.LogId,
                                            Amount = 0,
                                            ActualPrice = 0,
                                            Amt = 0,
                                            Status = 1,
                                            Title = "现金通道",
                                            Type = "9",
                                            ClientID = clientId,
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = userId
                                        };
                                        dbcontent.MemberCards.Add(card1);
                                        dbcontent.SaveChanges();

                                        cardid = card1.MemberCardId;
                                    }
                                    else
                                    {
                                        cardid = card.MemberCardId;
                                    }
                                }
                            }

                            #endregion

                            amount += pamount; //累加订单

                            // 保存记录
                            if (!string.IsNullOrEmpty(di[7]))
                            {
                                long bpid = Convert.ToInt64(di[7]);
                                var bp = dbcontent.BookProjects.Where(t => t.BookProjectID == bpid).FirstOrDefault();
                                if (bp != null)
                                {
                                    bp.ProjectID = Convert.ToInt32(di[0]);
                                    bp.BookID = currentBookId;
                                    bp.MemberCardId = cardid;
                                    bp.Quantity = Convert.ToInt32(di[2]);
                                    bp.Amount = pamount;
                                    bp.Type = Convert.ToInt32(di[9]);
                                    bp.HandicraftFee = hf;

                                    foreach (var usp in bp.UserSplits)
                                    {
                                        if (us.Where(t => t.UserID.Contains(usp.UserID)).Count() == 0)
                                        {
                                            // 删除
                                            dbcontent.BookProjectSplits.Remove(bp.UserSplits.Where(t => t.UserID == usp.UserID).First());
                                        }
                                    }
                                    foreach (var usd in us)
                                    {
                                        if (bp.UserSplits.Where(t => t.UserID == usd.UserID).Count() == 0)
                                        {
                                            // 添加新的美容师
                                            bp.UserSplits.Add(new BookProjectSplit { UserID = usd.UserID, Position = "2", HandicraftFee = hf / workernum });
                                        }
                                    }

                                    // 卡扣/现消
                                    if (di[1] == "false")
                                    {
                                        bp.MemberProjectId = Convert.ToInt64(di[5]);
                                        if (giveId != default(long))
                                            bp.MemberGiveId = giveId;
                                    }

                                    dbcontent.SaveChanges();
                                }
                            }
                            else
                            {
                                BookProject bp = new BookProject()
                                {
                                    ProjectID = Convert.ToInt32(di[0]),
                                    BookID = currentBookId,
                                    MemberCardId = cardid,
                                    Quantity = Convert.ToInt32(di[2]),
                                    Amount = pamount,
                                    Type = Convert.ToInt32(di[9]),
                                    HandicraftFee = hf,
                                    UserSplits = us
                                };
                                // 卡扣/现消
                                if (di[1] == "false")
                                {
                                    bp.MemberProjectId = Convert.ToInt64(di[5]);
                                    if (giveId != default(long))
                                        bp.MemberGiveId = giveId;
                                }

                                dbcontent.BookProjects.Add(bp);
                            }
                        }
                    }

                    // 更新预约状态
                    Appointment ap = dbcontent.Appointments.Where(a => a.MemberID == memberId && a.BookDate.Year == DateTime.Today.Year
                                              && a.BookDate.Month == DateTime.Today.Month && a.BookDate.Day == DateTime.Today.Day).FirstOrDefault();
                    if (ap != null)
                    {
                        ap.BookId = currentBookId;
                        ap.BookStatus = "1";
                    }

                    // 更新新保存的订单
                    Book nb = dbcontent.Books.Where(a => a.BookID == currentBookId).FirstOrDefault();
                    nb.Amount = amount;

                    dbcontent.SaveChanges();
                    dbtran.Complete();

                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                logger.Error("消费失败", ex);
                result = new
                {
                    code = 2,
                    message = ex.Message
                };
                return Json(result);
            }
        }

        /// <summary>
        /// 消费订单支付
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="memberId"></param>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <param name="books"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppSettlement(int hostId, int branchId, int memberId, string clientId, string userId, string bookId, string SalesId, string books)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];

            var result = new
            {
                code = 1,
                message = string.Empty
            };

            try
            {
                using (var dbtran = new TransactionScope(TransactionScopeOption.Required))
                {
                    string[] st = books.Split(';');
                    Book currentBook = null;
                    EventLog eventLog = null;

                    if (!string.IsNullOrEmpty(bookId))
                    {
                        var currentBookId = Convert.ToInt64(bookId);
                        currentBook = dbcontent.Books.Where(t => t.BookID == currentBookId).FirstOrDefault();
                        currentBook.SalesmanID = SalesId;
                        eventLog = dbcontent.EventLogs.Where(t => t.LogId == currentBook.LogId).FirstOrDefault();
                    }
                    else
                    {
                        // 操作任务
                        var log = new EventLog
                        {
                            HostId = hostId,
                            BranchId = branchId,
                            MemberId = memberId,
                            TypeId = 10,
                            UserId = userId,
                            ClientId = clientId,
                            CreatedDate = DateTime.Now,
                            Level = 5
                        };
                        eventLog = dbcontent.EventLogs.Add(log);
                        dbcontent.SaveChanges();

                        // 新增消耗记录
                        var book = new Book
                        {
                            HostID = hostId,
                            BranchId = branchId,
                            MemberID = memberId,
                            LogId = eventLog.LogId,
                            Amount = 0,
                            ClientID = clientId,
                            CreatedBy = userId,
                            State = "0",
                            CreatedDate = DateTime.Now,
                            SalesmanID = SalesId
                        };

                        currentBook = dbcontent.Books.Add(book);
                        dbcontent.SaveChanges();
                    }

                    decimal amount = 0;  // 消费总额

                    // 客户信息
                    Member mb = dbcontent.Members.Where(a => a.MemberID == memberId).FirstOrDefault();
                    string ProjectNames = "";

                    // 订单项目列表
                    foreach (string dd in st)
                    {
                        if (!string.IsNullOrEmpty(dd))
                        {
                            // 消费项目
                            string[] di = dd.Split(','); // [项目ID, [卡扣：false，现付：true]，数量，[美容师]，金额, MemberProjectId, MemberCardId, BookProjectId, IsEntity, PaymentType]
                            decimal pamount = 0;
                            int pjid = Convert.ToInt32(di[0]);
                            Project pj = dbcontent.Projects.Where(t => t.ProjectID == pjid).FirstOrDefault();
                            ProjectNames += pj.Name + ",";
                            decimal hf = pj.HandicraftFee * Convert.ToInt32(di[2]);

                            if (di[1] == "false")  // 卡扣疗程
                            {
                                long id = Convert.ToInt64(di[5]);
                                MemberProject mpp = dbcontent.MemberProjects.Where(a => a.MemberProjectId == id).FirstOrDefault();
                                if (mpp.Type == "1")
                                {
                                    pamount = Convert.ToDecimal(di[4]);
                                    hf = pj.LowHandicraftFee * Convert.ToInt32(di[2]);
                                }
                                else
                                    pamount = mpp.UnitPrice * Convert.ToInt32(di[2]);
                            }
                            else                   // 现付购买
                            {
                                pamount = Convert.ToDecimal(di[4]);
                            }
                            long giveId = default(long);
                            int paymentType = Convert.ToInt32(di[9]);

                            #region  服务人员

                            List<BookProjectSplit> us = new List<BookProjectSplit>();
                            List<AccountRecordSplit> rs = new List<AccountRecordSplit>();
                            List<AccountRecordSplit> fq = new List<AccountRecordSplit>();

                            // 添加顾问
                            //us.Add(new BookProjectSplit
                            //{
                            //    UserID = SalesId,
                            //    Position = "1",
                            //    Percentage = 1M,
                            //    Amount = pamount,
                            //    ModifiedTime = DateTime.Now,
                            //    ModifiedBy = userId
                            //});
                            //rs.Add(new AccountRecordSplit
                            //{
                            //    UserID = SalesId,
                            //    Position = "1",
                            //    Percentage = 1M,
                            //    Amount = pamount,
                            //    ModifiedTime = DateTime.Now,
                            //    ModifiedBy = userId
                            //});
                            //fq.Add(new AccountRecordSplit
                            //{
                            //    UserID = SalesId,
                            //    Position = "1",
                            //    Percentage = 1M,
                            //    Amount = pamount,
                            //    ModifiedTime = DateTime.Now,
                            //    ModifiedBy = userId
                            //});


                            string[] wo = di[3].Split('|');
                            int workernum = 0;
                            foreach (string works in wo)
                            {
                                if (!string.IsNullOrEmpty(works))
                                {
                                    workernum++;
                                }
                            }

                            foreach (string works in wo)
                            {
                                if (!string.IsNullOrEmpty(works))
                                {
                                    ApplicationUser u = dbcontent.Users.Where(a => a.Id == works).FirstOrDefault();
                                    if (u != null)
                                    {
                                        us.Add(new BookProjectSplit
                                        {
                                            UserID = works,
                                            Position = "2",
                                            Percentage = 1M / workernum,
                                            Amount = pamount / workernum,
                                            ModifiedTime = DateTime.Now,
                                            ModifiedBy = userId,
                                            HandicraftFee = hf / workernum
                                        });

                                        //if (rs.Where(t => t.UserID.Contains(works)).Count() == 0)
                                        //{
                                        // 去掉重复
                                        rs.Add(new AccountRecordSplit
                                        {
                                            UserID = works,
                                            Position = "2",
                                            Percentage = 1M / workernum,
                                            Amount = pamount / workernum,
                                            ModifiedTime = DateTime.Now,
                                            ModifiedBy = userId
                                        });
                                        fq.Add(new AccountRecordSplit
                                        {
                                            UserID = works,
                                            Position = "2",
                                            Percentage = 1M / workernum,
                                            Amount = pamount / workernum,
                                            ModifiedTime = DateTime.Now,
                                            ModifiedBy = userId
                                        });
                                        //}
                                    }
                                }
                            }

                            #endregion

                            long cardid = default(long);
                            string cardtype = "";

                            #region 现付项目
                            if (di[1] == "true")
                            {
                                int DebtFlag = 0;
                                decimal ActualPrice = pamount;

                                if (paymentType == 4)  // 储值卡
                                {
                                    //选择卡片
                                    if (!string.IsNullOrEmpty(di[6]))
                                    {
                                        string[] cardstring = di[6].Split(':');
                                        cardtype = cardstring[1];
                                        cardid = Convert.ToInt64(cardstring[0]);  // 客户端选择
                                    }
                                }
                                else
                                {
                                    // 现金通道
                                    var card = dbcontent.MemberCards.Where(t => t.MemberID == memberId && t.Type == "9").FirstOrDefault();
                                    if (card == null)
                                    {
                                        // 添加现金流通卡
                                        MemberCard card1 = new MemberCard
                                        {
                                            MemberID = memberId,
                                            HostID = hostId,
                                            BranchID = branchId,
                                            Amount = 0,
                                            ActualPrice = 0,
                                            Amt = 0,
                                            Status = 1,
                                            Title = "现金通道",
                                            Type = "9",
                                            ClientID = clientId,
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = userId
                                        };
                                        dbcontent.MemberCards.Add(card1);
                                        dbcontent.SaveChanges();

                                        cardid = card1.MemberCardId;
                                        cardtype = "9";
                                    }
                                    else
                                    {
                                        cardid = card.MemberCardId;
                                        cardtype = card.Type;
                                    }
                                }

                                var mcard = dbcontent.MemberCards.Where(t => t.MemberCardId == cardid).FirstOrDefault();

                                if (mcard != null)
                                {
                                    // 0:储值卡 1:疗程卡 2: 单次卡 3:留客卡 4: 综合限时卡 5:综合限次卡 6:拓客增值卡 7:拓客优惠卡 8:综合限时限次卡
                                    if (cardtype == "5" || cardtype == "8")
                                    {
                                        mcard.UsedTime = mcard.UsedTime + Convert.ToInt32(di[2]);
                                        mcard.LastCount = mcard.LastCount - Convert.ToInt32(di[2]);
                                    }
                                    // 现金 支付
                                    else if (cardtype == "9")
                                    {
                                        // 现金充值
                                        var log = new AccountRecord
                                        {
                                            MemberCardId = cardid,
                                            BranchId = branchId,
                                            HostID = mb.HostID,
                                            MemberID = memberId,
                                            EventLogId = eventLog.LogId,
                                            Type = "1",
                                            PaymentType = Convert.ToString(paymentType),
                                            Remark = "现金消费",
                                            InAmount = pamount,
                                            CreatedDate = DateTime.Now,
                                            ClientID = clientId,
                                            CreatedBy = userId,
                                            Splits = fq,
                                            Balance = mcard.Amt,
                                            SaleID = mb.SalesmanId,
                                            IsVaild = 1
                                        };

                                        dbcontent.AccountRecords.Add(log);
                                    }
                                    else
                                    {
                                        if (pamount > mcard.Amt)
                                        {
                                            DebtFlag = 1;
                                            ActualPrice = mcard.Amt;
                                        }

                                        if (cardtype == "0" || cardtype == "6")  // 储值卡
                                        {
                                            mcard.Amt -= ActualPrice;
                                            mb.Amt -= ActualPrice;
                                        }
                                    }
                                }


                                if (cardtype != "5" && cardtype != "4" && cardtype != "8")  // 限次卡不记录购买
                                {
                                    // 消耗记录
                                    var log = new AccountRecord
                                    {
                                        MemberCardId = cardid,
                                        BranchId = branchId,
                                        HostID = mb.HostID,
                                        MemberID = memberId,
                                        EventLogId = eventLog.LogId,
                                        Type = "3",
                                        SalesType = (cardtype == "9" ? 2 : 1),
                                        PaymentType = "1",
                                        Remark = "现金消费",
                                        OutAmount = pamount,
                                        CreatedDate = DateTime.Now,
                                        ClientID = clientId,
                                        CreatedBy = userId,
                                        Splits = rs,
                                        Balance = mcard.Amt,
                                        SaleID = mb.SalesmanId,
                                        IsVaild = 1
                                    };
                                    dbcontent.AccountRecords.Add(log);
                                    dbcontent.SaveChanges();

                                    #region 保存到购买项目表

                                    var mp = new MemberProject
                                    {
                                        MemberCardId = cardid,
                                        BranchId = branchId,
                                        HostID = mb.HostID,
                                        MemberID = memberId,
                                        LogId = eventLog.LogId,
                                        AccountRecordID = log.RecordID,
                                        ProjectID = Convert.ToInt32(di[0]),
                                        Remark = "",
                                        Amount = pamount,
                                        BookTime = Convert.ToInt32(di[2]),
                                        CreatedDate = DateTime.Now,
                                        LastCount = 0,
                                        UsedTime = Convert.ToInt32(di[2]),
                                        ClientId = clientId,
                                        CreatedBy = userId,
                                        Type = "0",
                                        ActualPrice = ActualPrice,
                                        DebtFlag = DebtFlag,   // 欠款移至卡
                                        status = 0
                                    };
                                    mp.UnitPrice = mp.Amount / mp.BookTime;
                                    dbcontent.MemberProjects.Add(mp);
                                    #endregion

                                    dbcontent.SaveChanges();
                                    di[5] = mp.MemberProjectId.ToString(); // 新的ID
                                }
                            }
                            #endregion

                            #region 卡扣项目
                            else
                            {
                                long id = Convert.ToInt64(di[5]);
                                MemberProject mp = dbcontent.MemberProjects.Where(a => a.MemberProjectId == id).FirstOrDefault();
                                MemberCard mc = dbcontent.MemberCards.Where(a => a.MemberCardId == mp.MemberCardId).FirstOrDefault();

                                // 数量不够要报错
                                if (mp.LastCount < Convert.ToInt32(di[2]))
                                {
                                    throw new Exception("消费项目数量不足");
                                    //result = new
                                    //{
                                    //    code = 2,
                                    //    message = "消费项目数量不足."
                                    //};
                                    //return Json(result);
                                }

                                mp.UsedTime = mp.UsedTime + Convert.ToInt32(di[2]);  // 加次
                                mp.LastCount = mp.LastCount - Convert.ToInt32(di[2]);  //减次
                                mc.UsedTime = mc.UsedTime + Convert.ToInt32(di[2]);
                                mc.LastCount = mc.LastCount - Convert.ToInt32(di[2]);
                                if (mp.GiveId != null) giveId = mp.GiveId.Value;
                                cardid = mp.MemberCardId.Value;
                            }
                            #endregion

                            amount += pamount; //累加订单

                            BookProject bp = null;
                            // 保存记录
                            if (!string.IsNullOrEmpty(di[7]))
                            {
                                long bpid = Convert.ToInt64(di[7]);
                                bp = dbcontent.BookProjects.Where(t => t.BookProjectID == bpid).FirstOrDefault();
                                if (bp != null)
                                {
                                    bp.ProjectID = Convert.ToInt32(di[0]);
                                    bp.BookID = currentBook.BookID;
                                    bp.MemberCardId = cardid;
                                    bp.Quantity = Convert.ToInt32(di[2]);
                                    bp.Amount = pamount;
                                    bp.Type = Convert.ToInt32(di[9]);
                                    bp.HandicraftFee = hf;

                                    foreach (var usp in bp.UserSplits)
                                    {
                                        if (us.Where(t => t.UserID.Contains(usp.UserID)).Count() == 0)
                                        {
                                            // 删除
                                            dbcontent.BookProjectSplits.Remove(bp.UserSplits.Where(t => t.UserID == usp.UserID).First());
                                        }
                                    }
                                    foreach (var usd in us)
                                    {
                                        if (bp.UserSplits.Where(t => t.UserID == usd.UserID).Count() == 0)
                                        {
                                            // 添加新的美容师
                                            bp.UserSplits.Add(new BookProjectSplit
                                            {
                                                UserID = usd.UserID,
                                                Position = usd.Position,
                                                Percentage = usd.Percentage,
                                                Amount = usd.Amount,
                                                ModifiedBy = usd.UserID,
                                                ModifiedTime = DateTime.Now,
                                                HandicraftFee = hf / workernum
                                            });
                                        }
                                    }

                                    // 卡扣/现消
                                    if (di[5] != "undefined" && di[5] != "null")
                                    {
                                        bp.MemberProjectId = Convert.ToInt64(di[5]);
                                        if (giveId != default(long))
                                        {
                                            bp.MemberGiveId = giveId;
                                            var give = dbcontent.MemberGives.Where(t => t.GiveId == giveId).FirstOrDefault();
                                            if (give != null)
                                            {
                                                give.LastCount = give.LastCount - Convert.ToInt32(di[2]);
                                                give.UsedTime = give.UsedTime + Convert.ToInt32(di[2]);
                                            }
                                        }
                                    }

                                    dbcontent.SaveChanges();
                                }
                            }
                            else
                            {
                                bp = new BookProject()
                                {
                                    ProjectID = Convert.ToInt32(di[0]),
                                    BookID = currentBook.BookID,
                                    MemberCardId = cardid,
                                    Quantity = Convert.ToInt32(di[2]),
                                    Amount = pamount,
                                    Type = Convert.ToInt32(di[9]),
                                    HandicraftFee = hf,
                                    UserSplits = us
                                };

                                // 卡扣/现消
                                if (di[5] != "undefined" && di[5] != "null")
                                {
                                    bp.MemberProjectId = Convert.ToInt64(di[5]);
                                    if (giveId != default(long))
                                    {
                                        bp.MemberGiveId = giveId;
                                        var give = dbcontent.MemberGives.Where(t => t.GiveId == giveId).FirstOrDefault();
                                        if (give != null)
                                        {
                                            give.LastCount = give.LastCount - Convert.ToInt32(di[2]);
                                            give.UsedTime = give.UsedTime + Convert.ToInt32(di[2]);
                                        }
                                    }
                                }

                                dbcontent.BookProjects.Add(bp);
                                dbcontent.SaveChanges();
                            }

                            // 货品数据保存
                            var pgs = dbcontent.ProjectGoods.Where(t => t.ProjectID == pjid).ToList();
                            foreach (var pg in pgs)
                            {
                                var bgs = new BookGoods
                                {
                                    BookID = currentBook.BookID,
                                    GoodsID = pg.GoodsID,
                                    BookProjectID = bp.BookProjectID,
                                    Quantity = pg.Quantity * Convert.ToInt32(di[2]),
                                    ProjectID = pg.ProjectID
                                };
                                dbcontent.BookGoods.Add(bgs);
                            }
                        }
                    }

                    // 更新预约状态
                    Appointment ap = dbcontent.Appointments.Where(a => a.MemberID == memberId && a.BookDate.Year == DateTime.Today.Year
                                            && a.BookDate.Month == DateTime.Today.Month && a.BookDate.Day == DateTime.Today.Day).FirstOrDefault();
                    if (ap != null)
                    {
                        ap.BookId = currentBook.BookID;
                        ap.BookStatus = "1";
                    }

                    // 更新新保存的订单
                    currentBook.Amount = amount;
                    currentBook.State = "20";

                    dbcontent.SaveChanges();
                    dbtran.Complete();

                    // 微信客户提醒
                    if (!string.IsNullOrEmpty(mb.OpenID))
                    {
                        var accessToken = AccessTokenContainer.TryGetAccessToken(bag.AppId, bag.Secret);
                        var testData = new
                        {
                            productType = new TemplateDataItem("服务项目"),
                            name = new TemplateDataItem(ProjectNames),
                            accountType = new TemplateDataItem("会员卡号"),
                            account = new TemplateDataItem(mb.CardNo),
                            time = new TemplateDataItem(currentBook.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss")),
                            remark = new TemplateDataItem("点击查看服务详情")
                        };
                        string url = "http://cn.mdss.hk/wap/book/" + currentBook.BookID;
                        var result1 = TemplateApi.SendTemplateMessage(accessToken, mb.OpenID, bag.TmplMsg_Service, "#FF0000", url, testData);
                    }
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                logger.Error("消费失败", ex);
                result = new
                {
                    code = 2,
                    message = ex.Message
                };
                return Json(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BookId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppBookInfo(long BookId)
        {
            try
            {

                var vv = (from _ in dbcontent.Books.Where(t => t.BookID == BookId)
                          select new BookModel
                          {
                              BookID = _.BookID,
                              Member = _.Member,
                              MemberName = _.Member.Name,
                              MemberCardNo = _.Member.CardNo,
                              SalesmanID = _.SalesmanID,
                              CreatedDate = _.CreatedDate,
                              BookProjects = _.BookProjects,
                              State = _.State,
                              StateValue = dbcontent.Dictionaries.Where(a => a.KeyValue == _.State && a.Identifier == "BookState").FirstOrDefault().Contents,
                              PayTime = _.PayTime
                          }).FirstOrDefault();

                var book = new
                {
                    Id = vv.BookID,
                    BookDate = vv.CreatedDate,
                    Projects = vv.SalesmanID,
                    State = vv.State,
                    StateValue = vv.StateValue,
                    PayTime = vv.PayTime,
                    SalesmanID = vv.SalesmanID
                };

                var projects = (from _ in vv.BookProjects
                                select new
                                {
                                    BookProjectId = _.BookProjectID,
                                    ProjectId = _.ProjectID,
                                    ProjectName = _.Project.Name,
                                    Catetory = _.Project.Category,
                                    Brand = _.Project.Brand,
                                    ExtCategory = _.Project.ExtCategory,
                                    Quantity = _.Quantity,
                                    Beauties = _.UserSplits.Select(t => t.User.Id),
                                    Workers = _.UserSplits.Select(t => t.User.UserCnName),
                                    Amount = _.Amount,
                                    MemberProjectId = _.MemberProjectId,
                                    MemberCardId = _.MemberCardId,
                                    MemberCardType = (_.MemberCardId == null ? "" : dbcontent.MemberCards.Where(t => t.MemberCardId == _.MemberCardId).FirstOrDefault().Type),
                                    Type = _.Type
                                }).ToList();

                var result = new
                {
                    book = book,
                    projects = projects
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 当天订单
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="cardNo"></param>
        /// <param name="status"></param>
        /// <param name="q"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppDayList(int hostId, int branchId, string cardNo, string status, string q, string userId, string date)
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
                var app = dbcontent.Books.AsQueryable();
                if (!String.IsNullOrEmpty(status))
                    app = app.Where(a => a.State.Equals(status));
                if (!String.IsNullOrEmpty(q))
                    query = query.Where(a => a.Name.Contains(q) || a.MobileNumber.Contains(q) || a.CardNo.Contains(q));

                var list = (from _ in app
                            join m in query on _.MemberID equals m.MemberID
                            where _.HostID == hostId && _.BranchId == branchId
                                  && _.CreatedDate.Year == d.Year && _.CreatedDate.Month == d.Month && _.CreatedDate.Day == d.Day
                            select new BookModel
                            {
                                BookID = _.BookID,
                                Member = _.Member,
                                MemberName = m.Name,
                                MemberCardNo = m.CardNo,
                                CreatedDate = _.CreatedDate,
                                BookProjects = _.BookProjects,
                                State = _.State,
                                StateValue = dbcontent.Dictionaries.Where(a => a.KeyValue == _.State && a.Identifier == "BookState").FirstOrDefault().Contents,
                                PayTime = _.PayTime
                            }).ToList();
                var count = list.Count();

                var ll = (from v in list
                          select new
                          {
                              Id = v.BookID,
                              ProjectName = String.Join(",", v.BookProjects.Select(t => t.Project.Name).ToArray()),
                              MemberName = v.Member.Name,
                              MemberId = v.Member.MemberID,
                              CardNo = v.Member.CardNo,
                              BookDate = v.CreatedDate,
                              Workers = String.Join(",", v.BookProjects.First().UserSplits.Select(t => t.User.UserCnName).ToArray()),
                              State = v.State,
                              StateValue = v.StateValue,
                              PayTime = v.PayTime
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

        #endregion

    }
}
