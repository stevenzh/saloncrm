using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Identity;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 回访
    /// </summary>
    [Authorize]
    public class FeedbackController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: Feedback
        /// <summary>
        /// 回访
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(FeedbackQModel qmodel)
        {
            CustomPrincipal cu = (CustomPrincipal)User;
            if (cu.Type == "1")   // 门店用户锁死
            {
                qmodel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
                qmodel.BeauticianId = GlobalContext.Current.UserInfo.Id;
            }
            else if (cu.Type == "3")
            {
                qmodel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
                qmodel.SalesmanId = GlobalContext.Current.UserInfo.Id;
            }
            else if (cu.Type == "4")
            {
                qmodel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
            }

            ViewData["Name"] = qmodel.Name;
            ViewData["BranchId"] = qmodel.BranchId;
            ViewData["CardNo"] = qmodel.CardNo;
            ViewData["Mobile"] = qmodel.Mobile;
            ViewData["FeedbackDate"] = qmodel.FeedbackDate;
            ViewData["Purpose"] = qmodel.Purpose;
            ViewData["SalesmanId"] = qmodel.SalesmanId;
            ViewData["BeauticianId"] = qmodel.BeauticianId;

            qmodel.FeedbackList = GetFeedbackList(qmodel);
            InitDrop();

            return View(qmodel);
        }
        /// <summary>
        /// 回访
        /// </summary>
        /// <returns></returns>
        public ActionResult FeedbackList(FeedbackQModel qmodel)
        {
            ViewData["Name"] = qmodel.Name;
            ViewData["BranchId"] = qmodel.BranchId;
            ViewData["CardNo"] = qmodel.CardNo;
            ViewData["Mobile"] = qmodel.Mobile;
            ViewData["FeedbackDate"] = qmodel.FeedbackDate;
            ViewData["Purpose"] = qmodel.Purpose;
            ViewData["SalesmanId"] = qmodel.SalesmanId;
            ViewData["BeauticianId"] = qmodel.BeauticianId;

            return PartialView("FeedbackList", GetFeedbackList(qmodel));
        }
        /// <summary>
        /// 客户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CrisisIndex(FeedbackQModel qmodel)
        {
            CustomPrincipal cu = (CustomPrincipal)User;
            if (cu.Type == "1")   // 门店用户锁死
            {
                qmodel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
                qmodel.BeauticianId = GlobalContext.Current.UserInfo.Id;
            }
            else if (cu.Type == "3")
            {
                qmodel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
                qmodel.SalesmanId = GlobalContext.Current.UserInfo.Id;
            }
            else if (cu.Type == "4")
            {
                qmodel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
            }

            ViewData["Name"] = qmodel.Name;
            ViewData["BranchId"] = qmodel.BranchId;
            ViewData["CardNo"] = qmodel.CardNo;
            ViewData["Mobile"] = qmodel.Mobile;
            ViewData["FeedbackDate"] = qmodel.FeedbackDate;
            ViewData["Purpose"] = qmodel.Purpose;
            ViewData["SalesmanId"] = qmodel.SalesmanId;
            ViewData["BeauticianId"] = qmodel.BeauticianId;

            qmodel.FeedbackList = GetCrisisList(qmodel);
            InitDrop();

            return View(qmodel);
        }
        /// <summary>
        /// 客户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CrisisList(FeedbackQModel qmodel)
        {
            ViewData["Name"] = qmodel.Name;
            ViewData["BranchId"] = qmodel.BranchId;
            ViewData["CardNo"] = qmodel.CardNo;
            ViewData["Mobile"] = qmodel.Mobile;
            ViewData["FeedbackDate"] = qmodel.FeedbackDate;
            ViewData["Purpose"] = qmodel.Purpose;
            ViewData["SalesmanId"] = qmodel.SalesmanId;
            ViewData["BeauticianId"] = qmodel.BeauticianId;

            return PartialView("CrisisList", GetCrisisList(qmodel));
        }

        //// GET: MemberAdmin/Details/5
        //public ActionResult Details(int id)
        //{
        //    string userId = GlobalContext.Current.UserInfo.Id;
        //    MemberViewModel mb = (from d in dbcontent.Members.Where(a => a.MemberID == id)
        //                          select new MemberViewModel
        //                          {
        //                              MemberID = d.MemberID,
        //                              Name = d.Name,
        //                              JoinDate = d.JoinDate,
        //                              JoinBranch = d.JoinBranch,
        //                              Level = d.Level,
        //                              Type = d.Type,
        //                              Source = d.Source,
        //                              Sex = d.Sex,
        //                              Vocation = d.Vocation,
        //                              MobileNumber = d.MobileNumber,
        //                              Amt = d.Amt,
        //                              CardNo = d.CardNo,
        //                          }).First();
        //    var manage = new MemberAdminController();
        //    mb.Feedbacks = manage.GetFeedbacks(id);
        //    mb.ExpenseBooks = manage.GetExpenseBooks(id);
        //    mb.UsableProjects = manage.GetUsedProjects(id);


        //    mb.Feedback = new Feedback { MemberId = id, CallUserId = userId };
        //    ViewData["MemberID"] = id;
        //    ViewBag.FeedbackType = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "FeedbackType").ToList(), "KeyValue", "Contents");
        //    ViewBag.FeedbackWay = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "FeedbackWay").ToList(), "KeyValue", "Contents");
        //    return View(mb);
        //}

        // POST: Feedback/Create
        [HttpPost]
        public ActionResult Create(MemberViewModel model)
        {
            try
            {
                Feedback entity = new Feedback
                {
                    HostId = GlobalContext.Current.UserHost.HostID,
                    MemberId = model.Feedback.MemberId,
                    Purpose = model.Feedback.Purpose,
                    CallUserId = model.Feedback.CallUserId,
                    Centent = model.Feedback.Centent,
                    LinkWay = model.Feedback.LinkWay,
                    CreatedDate = DateTime.Now
                };

                dbcontent.Feedbacks.Add(entity);
                dbcontent.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditModesUpdatePartial(FeedbackViewModel product, string Name1, int BranchId1, string CardNo1, string Mobile1, DateTime? FeedbackDate1, string Purpose1)
        {
            FeedbackQModel qmodel = new FeedbackQModel
            {
                Name = Name1,
                BranchId = BranchId1,
                CardNo = CardNo1,
                Mobile = Mobile1,
                FeedbackDate = FeedbackDate1,
                Purpose = Purpose1
            };
            ViewData["Name"] = qmodel.Name;
            ViewData["BranchId"] = qmodel.BranchId;
            ViewData["CardNo"] = qmodel.CardNo;
            ViewData["Mobile"] = qmodel.Mobile;
            ViewData["FeedbackDate"] = qmodel.FeedbackDate;
            ViewData["Purpose"] = qmodel.Purpose;
            ViewData["SalesmanId"] = qmodel.SalesmanId;
            ViewData["BeauticianId"] = qmodel.BeauticianId;

            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.Members.Where(t => t.MemberID == product.MemberId).FirstOrDefault();
                    m.Feedback = product.Purpose;
                    m.FeedbackDate = product.NextDate;
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("CrisisList", GetCrisisList(qmodel));
        }

        /// <summary>
        /// 回访列表
        /// </summary>
        /// <returns></returns>
        private IList<FeedbackViewModel> GetFeedbackList(FeedbackQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Feedbacks.Where(t => t.HostId == hostId);
            if (qmodel.FeedbackDate != null)
                query = query.Where(t => t.CreatedDate == qmodel.FeedbackDate);
            if (!string.IsNullOrEmpty(qmodel.Purpose))
                query = query.Where(t => t.Purpose == qmodel.Purpose);

            var mem = dbcontent.Members.Where(t => t.HostID == hostId);
            if (qmodel.BranchId != default(int))
                mem = mem.Where(t => t.JoinBranch == qmodel.BranchId);
            if (!String.IsNullOrEmpty(qmodel.Name))
                mem = mem.Where(t => t.Name.Contains(qmodel.Name));
            if (!String.IsNullOrEmpty(qmodel.CardNo))
                mem = mem.Where(t => t.CardNo == qmodel.CardNo);
            if (!String.IsNullOrEmpty(qmodel.Mobile))
                mem = mem.Where(t => t.MobileNumber == qmodel.Mobile);
            if (!String.IsNullOrEmpty(qmodel.SalesmanId))
                mem = mem.Where(a => a.SalesmanId.Equals(qmodel.SalesmanId));
            if (!String.IsNullOrEmpty(qmodel.BeauticianId))
                mem = mem.Where(a => a.BeauticianId.Equals(qmodel.BeauticianId));

            IList<FeedbackViewModel> logs = (from vm in mem
                                             join vf in query on vm.MemberID equals vf.MemberId into ___
                                             from vf in ___.DefaultIfEmpty()
                                             select new FeedbackViewModel
                                             {
                                                 MemberId = vm.MemberID,
                                                 MemberName = vm.Name,
                                                 CardNo = vm.CardNo,
                                                 MobileNumber = vm.MobileNumber,
                                                 StatusValue = dbcontent.Dictionaries.Where(a => a.KeyValue == vm.Status && a.HostId == hostId && a.Identifier == "MemberStatus").FirstOrDefault().Contents,
                                                 CreatedDate = vf.CreatedDate,
                                                 FeedbackDate = vm.FeedbackDate,
                                                 NextDate = vf.NextDate,
                                                 Purpose = dbcontent.Dictionaries.Where(a => a.KeyValue == vf.Purpose && a.Identifier == "FeedbackType").FirstOrDefault().Contents,
                                                 CallUserId = vf.CallUserId,
                                                 CallUserName = dbcontent.Users.Where(t => t.Id == vf.CallUserId).FirstOrDefault().UserCnName,
                                                 Centent = vf.Centent
                                             }).ToList();
            return logs;
        }

        /// <summary>
        /// 客户列表
        /// </summary>
        /// <returns></returns>
        private IList<FeedbackViewModel> GetCrisisList(FeedbackQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Members.Where(t => t.HostID == hostId);

            if (qmodel.BranchId != default(int))
                query = query.Where(t => t.JoinBranch == qmodel.BranchId);
            if (!String.IsNullOrEmpty(qmodel.Name))
                query = query.Where(t => t.Name.Contains(qmodel.Name));
            if (!String.IsNullOrEmpty(qmodel.CardNo))
                query = query.Where(t => t.CardNo == qmodel.CardNo);
            if (!String.IsNullOrEmpty(qmodel.Mobile))
                query = query.Where(t => t.MobileNumber == qmodel.Mobile);
            if (!String.IsNullOrEmpty(qmodel.SalesmanId))
                query = query.Where(a => a.SalesmanId.Equals(qmodel.SalesmanId));
            if (!String.IsNullOrEmpty(qmodel.BeauticianId))
                query = query.Where(a => a.BeauticianId.Equals(qmodel.BeauticianId));

            if (qmodel.FeedbackDate != null)
                query = query.Where(t => t.FeedbackDate == qmodel.FeedbackDate);
            if (!string.IsNullOrEmpty(qmodel.Purpose))
                query = query.Where(t => t.Feedback == qmodel.Purpose);

            IList<FeedbackViewModel> logs = (from vm in query
                                             join br in dbcontent.Organs on vm.JoinBranch equals br.OrganID
                                             select new FeedbackViewModel
                                             {
                                                 MemberId = vm.MemberID,
                                                 BranchName = br.Name,
                                                 MemberName = vm.Name,
                                                 CardNo = vm.CardNo,
                                                 MobileNumber = vm.MobileNumber,
                                                 CreatedDate = vm.CreatedDate,
                                                 LevelValue = dbcontent.Dictionaries.Where(a => a.KeyValue == vm.Level && a.HostId == hostId && a.Identifier == "MemberLevel").FirstOrDefault().Contents,
                                                 Status = vm.Status,
                                                 StatusValue = dbcontent.Dictionaries.Where(a => a.KeyValue == vm.Status && a.HostId == hostId && a.Identifier == "MemberStatus").FirstOrDefault().Contents,
                                                 NextDate = vm.FeedbackDate,
                                                 Purpose = vm.Feedback
                                             }).ToList();
            foreach (FeedbackViewModel dd in logs)
            {
                var l = dbcontent.Feedbacks.Where(t => t.MemberId == dd.MemberId).OrderByDescending(t => t.CreatedDate);
                if (l.Count() > 0)
                {
                    dd.FeedbackDate = l.FirstOrDefault().CreatedDate;
                    dd.Result = l.FirstOrDefault().Result;
                }
            }
            return logs;
        }

        /// <summary>
        /// 初始化下拉菜单
        /// </summary>
        private void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            List<SelectListItem> items = new SelectList(dbcontent.Organs.Where(t => t.HostID == hostId).ToList(), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;

            List<SelectListItem> items2 = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "FeedbackType").ToList(), "KeyValue", "Contents").ToList();
            items2.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.FeedbackType = items2;

            // 顾问
            List<SelectListItem> items4 = new SelectList(dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type == "3" && t.Status == "1").ToList(), "Id", "UserCnName").ToList();
            items4.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.Salesman = items4;
            // 美容师
            List<SelectListItem> items5 = new SelectList(dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type == "1" && t.Status == "1").ToList(), "Id", "UserCnName").ToList();
            items5.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.Beautician = items5;
        }
    }

}