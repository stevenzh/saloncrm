using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Common.Logging;
using EntityFramework.Extensions;
using SalonCRM.Identity;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Manager;

namespace SalonCRM.Controllers
{
    [CustomAuthorize]
    public class AccountRecordController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        ILog logger = LogManager.GetLogger("AccountRecordController");

        // GET: AccountRecord
        public ActionResult Index(AccountRecordQModel viewModel)
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
                viewModel.SalesmanId = GlobalContext.Current.UserInfo.Id;
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

            var items6 = new SelectList(CommonManager.GetDictionaries("MemberCardType"), "KeyValue", "Contents").ToList();
            items6.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.CardTypeList = items6;

            var items7 = new SelectList(CommonManager.GetDictionaries("AccountRecordType"), "KeyValue", "Contents").ToList();
            items7.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.TypeList = items7;

            viewModel.RecordList = GetRecordList(viewModel);
            return View(viewModel);
        }

        public ActionResult RecordList(AccountRecordQModel viewModel)
        {
            ViewData["BranchId"] = viewModel.BranchId;
            ViewData["StartDate"] = viewModel.StartDate;
            ViewData["EndDate"] = viewModel.EndDate;
            ViewData["SalesmanId"] = viewModel.SalesmanId;
            ViewData["BeauticianId"] = viewModel.BeauticianId;
            ViewData["Type"] = viewModel.Type;
            ViewData["CardType"] = viewModel.CardType;
            ViewData["CardNo"] = viewModel.CardNo;

            var RecordList = GetRecordList(viewModel);
            return PartialView("RecordList", RecordList);
        }

        public List<AccountRecordModel> GetRecordList(AccountRecordQModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.AccountRecords.Where(t => t.HostID == hostId);
            if (viewModel.BranchId != default(int))
                query = query.Where(t => t.BranchId == viewModel.BranchId);
            if (viewModel.StartDate != default(DateTime))
                query = query.Where(t => t.CreatedDate > viewModel.StartDate);
            if (viewModel.EndDate != default(DateTime))
            {
                var d = viewModel.EndDate.AddDays(1);
                query = query.Where(t => t.CreatedDate < d);
            }
            if (!string.IsNullOrEmpty(viewModel.Type))
            {
                List<string> tt = new List<string>();
                if (viewModel.Type.IndexOf(',') > 0)
                {
                    foreach (var vv in viewModel.Type.Split(',').ToArray())
                    {
                        if (!string.IsNullOrEmpty(vv))
                            tt.Add(vv);
                    }
                    query = query.Where(t => tt.Contains(t.Type));
                }
                else
                {
                    query = query.Where(t => t.Type == viewModel.Type);
                }
            }
            var ccar = dbcontent.MemberCards.Where(t => t.HostID == hostId);
            if (!String.IsNullOrEmpty(viewModel.CardType))
                ccar = ccar.Where(t => t.Type == viewModel.CardType);

            var mem = dbcontent.Members.Where(t => t.HostID == hostId);
            if (!String.IsNullOrEmpty(viewModel.SalesmanId))
                mem = mem.Where(a => a.SalesmanId.Equals(viewModel.SalesmanId));
            if (!String.IsNullOrEmpty(viewModel.BeauticianId))
                mem = mem.Where(a => a.BeauticianId.Equals(viewModel.BeauticianId));
            if (!String.IsNullOrEmpty(viewModel.CardNo))
                mem = mem.Where(a => a.CardNo.Equals(viewModel.CardNo));

            var RecordList = (from ll in query
                              join m in mem on ll.MemberID equals m.MemberID
                              join cc in ccar on ll.MemberCardId equals cc.MemberCardId
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
            }

            return RecordList;
        }


        // GET: AccountRecord/Details/5
        public ActionResult Details(long id)
        {
            var entity = (from bb in dbcontent.AccountRecords.Where(t => t.RecordID == id)
                          select new AccountRecordModel
                          {
                              RecordID = bb.RecordID,
                              CreatedDate = bb.CreatedDate,
                              CreatedBy = bb.CreatedBy,
                              ClientID = bb.ClientID,
                              BookID = bb.BookID,
                              InAmount = bb.InAmount,
                              OutAmount = bb.OutAmount,
                              BranchId = bb.BranchId,
                              HostID = bb.HostID,
                              MemberCardId = bb.MemberCardId,
                              MemberID = bb.MemberID,
                              SaleId = bb.SaleID,
                              Remark = bb.Remark,
                              Splits = bb.Splits,
                              Member = bb.Member,
                              Branch = dbcontent.Organs.Where(t => t.OrganID == bb.BranchId).FirstOrDefault(),
                              Type = bb.Type,
                              TypeValue = dbcontent.Dictionaries.Where(a => a.KeyValue == bb.Type && a.Identifier == "AccountRecordType").FirstOrDefault().Contents
                          }).FirstOrDefault();
            if (entity.Type == "3")
            {
                entity.MemberProjects = dbcontent.MemberProjects.Where(t => t.AccountRecordID == id).ToList();
            }

            ViewBag.Salesman = dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type != "2" && t.Status == "1").ToList();
            ViewData["RecordID"] = id;
            InitDrop();
            return View(entity);
        }


        [HttpPost]
        public ActionResult BatchDelete(string records)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };

            var list = records.Split(',').ToArray();
            foreach (var id in list)
            {
                if (string.IsNullOrEmpty(id)) continue;
                long bookid = long.Parse(id);

                var row = dbcontent.AccountRecords.Where(t => t.RecordID == bookid).Delete();
            }
            dbcontent.SaveChanges();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProjectList(long RecordID)
        {
            ViewData["RecordID"] = RecordID;
            var list = dbcontent.MemberProjects.Where(t => t.AccountRecordID == RecordID).ToList();
            return PartialView("ProjectList", list);
        }

        public ActionResult SplitList(long RecordID)
        {
            ViewData["RecordID"] = RecordID;
            InitDrop();
            var list = dbcontent.AccountRecordSplits.Where(t => t.RecordID == RecordID).ToList();
            return PartialView("SplitList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewSplitPartial(AccountRecordSplit model)
        {
            ViewData["RecordID"] = model.RecordID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = new AccountRecordSplit
                    {
                        RecordID = model.RecordID,
                        UserID = model.UserID,
                        Percentage = model.Percentage,
                        Amount = model.Amount,
                        ModifiedBy = userId,
                        ModifiedTime = DateTime.Now
                    };

                    dbcontent.AccountRecordSplits.Add(m);
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
            var list = dbcontent.AccountRecordSplits.Where(t => t.RecordID == model.RecordID).ToList();
            return PartialView("SplitList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateSplitPartial(AccountRecordSplit model)
        {
            ViewData["RecordID"] = model.RecordID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.AccountRecordSplits.Where(t => t.SplitID == model.SplitID).FirstOrDefault();
                    m.Percentage = model.Percentage;
                    m.UserID = model.UserID;
                    m.Amount = model.Amount;
                    m.ModifiedBy = userId;
                    m.ModifiedTime = DateTime.Now;
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
            var list = dbcontent.AccountRecordSplits.Where(t => t.RecordID == model.RecordID).ToList();
            return PartialView("SplitList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteSplitPartial(AccountRecordSplit model)
        {
            ViewData["RecordID"] = model.RecordID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.AccountRecordSplits.Where(t => t.SplitID == model.SplitID).FirstOrDefault();

                    if (m != null)
                    {
                        dbcontent.AccountRecordSplits.Remove(m);
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
            var list = dbcontent.AccountRecordSplits.Where(t => t.RecordID == model.RecordID).ToList();
            return PartialView("SplitList", list);
        }
        private void InitDrop()
        {
            ViewBag.Salesman = dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type != "2" && t.Status == "1").ToList();

            List<SelectListItem> pp = new List<SelectListItem> {
                 new SelectListItem { Text="顾问", Value="1" },
                 new SelectListItem { Text ="美容师",Value="2"},
                 new SelectListItem { Text ="助理美容师",Value="3"}
            };
            ViewBag.Position = pp;
        }
    }
}