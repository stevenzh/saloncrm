using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Drawing.Printing;
using System.Transactions;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using EntityFramework.Extensions;
using Common.Logging;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.Web;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs;
using SalonCRM.Models;
using SalonCRM.Models.Post;
using SalonCRM.Web;
using SalonCRM.Web.MVC;
using SalonCRM.Manager;
using SalonCRM.Identity;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 会员管理
    /// </summary>
    [Authorize]
    public class MemberAdminController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        ILog logger = LogManager.GetLogger("MemberAdminController");
        MemberManager service = new MemberManager();

        #region 会员管理

        // GET: MemberAdmin
        public ActionResult Index(MemberQModel viewModel, string message)
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

            ViewBag.Message = message;

            // 客户类型
            List<SelectListItem> items1 = new SelectList(dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberStatus").ToList(), "KeyValue", "Contents").ToList();
            items1.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.MemberStatus = items1;
            // 客户类型
            List<SelectListItem> items2 = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "MemberType").ToList(), "KeyValue", "Contents").ToList();
            items2.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.MemberType = items2;
            // 客户等级
            List<SelectListItem> items3 = new SelectList(dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberLevel").ToList(), "KeyValue", "Contents").ToList();
            items3.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.MemberLevel = items3;
            // 顾问
            List<SelectListItem> items4 = new SelectList(dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type == "3" && t.Status == "1").ToList(), "Id", "UserCnName").ToList();
            items4.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.Salesman = items4;
            // 美容师
            List<SelectListItem> items5 = new SelectList(dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type == "1" && t.Status == "1").ToList(), "Id", "UserCnName").ToList();
            items5.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.Beautician = items5;
            List<SelectListItem> items6 = new List<SelectListItem>();
            items6.Add(new SelectListItem { Value = "-1", Text = "--请选择--" });
            items6.Add(new SelectListItem { Value = "1", Text = "新客户" });
            items6.Add(new SelectListItem { Value = "0", Text = "老客户" });
            ViewBag.NewList = items6;

            ViewData["Name"] = viewModel.Name;
            ViewData["BranchId"] = viewModel.BranchId;
            ViewData["Level"] = viewModel.Level;
            ViewData["Type"] = viewModel.Type;
            ViewData["Status"] = viewModel.Status;
            ViewData["CardNo"] = viewModel.CardNo;
            ViewData["Mobile"] = viewModel.Mobile;
            ViewData["SalesmanId"] = viewModel.SalesmanId;
            ViewData["BeauticianId"] = viewModel.BeauticianId;
            ViewData["IsNew"] = viewModel.IsNew;

            viewModel.MemberList = GetMemberList(viewModel);
            return View(viewModel);
        }
        private List<MemberViewModel> GetMemberList(MemberQModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Members.Where(a => a.HostID == hostId);
            if (viewModel.BranchId != default(int))
                query = query.Where(a => a.JoinBranch == viewModel.BranchId);
            if (!String.IsNullOrEmpty(viewModel.CardNo))
                query = query.Where(a => a.CardNo.Equals(viewModel.CardNo));
            if (!String.IsNullOrEmpty(viewModel.Name))
                query = query.Where(a => a.Name.Contains(viewModel.Name));
            if (!String.IsNullOrEmpty(viewModel.Mobile))
                query = query.Where(a => a.MobileNumber.Contains(viewModel.Mobile));
            if (!String.IsNullOrEmpty(viewModel.Level))
                query = query.Where(a => a.Level.Equals(viewModel.Level));
            if (!String.IsNullOrEmpty(viewModel.Type))
                query = query.Where(a => a.Type.Equals(viewModel.Type));
            if (!String.IsNullOrEmpty(viewModel.Status))
                query = query.Where(a => a.Status.Equals(viewModel.Status));
            if (!String.IsNullOrEmpty(viewModel.SalesmanId))
                query = query.Where(a => a.SalesmanId.Equals(viewModel.SalesmanId));
            if (!String.IsNullOrEmpty(viewModel.BeauticianId))
                query = query.Where(a => a.BeauticianId.Equals(viewModel.BeauticianId));
            if (viewModel.IsNew != -1)
                query = query.Where(a => a.IsNew == viewModel.IsNew);

            List<MemberViewModel> MemberList = (from vm in query
                                                select new MemberViewModel
                                                {
                                                    MemberID = vm.MemberID,
                                                    Name = vm.Name,
                                                    CardNo = vm.CardNo,
                                                    MobileNumber = vm.MobileNumber,
                                                    JoinDate = vm.JoinDate,
                                                    Birthday = vm.Birthday,
                                                    TypeValue = dbcontent.Dictionaries.Where(a => a.KeyValue == vm.Type && a.Identifier == "MemberType").FirstOrDefault().Contents,
                                                    JoinBranchStr = dbcontent.Organs.Where(a => a.OrganID == vm.JoinBranch).FirstOrDefault().Name,
                                                    Amt = vm.Amt,
                                                    Points = vm.Points,
                                                    SexValue = dbcontent.Dictionaries.Where(a => a.KeyValue == vm.Sex && a.Identifier == "MemberGender").FirstOrDefault().Contents,
                                                    LevelValue = dbcontent.Dictionaries.Where(a => a.KeyValue == vm.Level && a.HostId == hostId && a.Identifier == "MemberLevel").FirstOrDefault().Contents,
                                                    StatusValue = dbcontent.Dictionaries.Where(a => a.KeyValue == vm.Status && a.HostId == hostId && a.Identifier == "MemberStatus").FirstOrDefault().Contents,
                                                    CreatedDate = vm.CreatedDate,
                                                    OpenID = vm.OpenID,
                                                    wxMember = dbcontent.WxMembers.Where(t => t.OpenID == vm.OpenID).FirstOrDefault(),
                                                    SalesmanId = vm.SalesmanId,
                                                    Salesman = dbcontent.Users.Where(t => t.Id == vm.SalesmanId).FirstOrDefault(),
                                                    BeauticianId = vm.BeauticianId,
                                                    Beautician = dbcontent.Users.Where(t => t.Id == vm.BeauticianId).FirstOrDefault(),
                                                    IsNew = vm.IsNew
                                                }).OrderBy(a => a.MemberID).ToList();
            return MemberList;
        }
        /// <summary>
        /// 客户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberList(MemberQModel viewModel)
        {
            ViewData["Name"] = viewModel.Name;
            ViewData["Name"] = viewModel.Name;
            ViewData["BranchId"] = viewModel.BranchId;
            ViewData["Level"] = viewModel.Level;
            ViewData["Type"] = viewModel.Type;
            ViewData["Status"] = viewModel.Status;
            ViewData["CardNo"] = viewModel.CardNo;
            ViewData["Mobile"] = viewModel.Mobile;
            ViewData["SalesmanId"] = viewModel.SalesmanId;
            ViewData["BeauticianId"] = viewModel.BeauticianId;
            ViewData["IsNew"] = viewModel.IsNew;

            return PartialView("MemberList", GetMemberList(viewModel));
        }

        // GET: MemberAdmin/Details/5
        public ActionResult Details(int id)
        {
            string userId = GlobalContext.Current.UserInfo.Id;
            int hostId = GlobalContext.Current.UserHost.HostID;
            var entity = (from model in dbcontent.Members.Where(t => t.HostID == hostId && t.MemberID == id)
                          select new MemberViewModel
                          {
                              // 会员信息
                              MemberID = model.MemberID,
                              Source = dbcontent.Dictionaries.Where(a => a.KeyValue == model.Source && a.Identifier == "MemberSource").FirstOrDefault().Contents,
                              JoinBranch = model.JoinBranch,
                              JoinBranchStr = dbcontent.Organs.Where(t => t.OrganID == model.JoinBranch).FirstOrDefault().Name,
                              JoinDate = model.JoinDate,
                              CardNo = model.CardNo,
                              Amt = model.Amt,
                              Level = model.Level,
                              Points = model.Points,
                              Status = model.Status,
                              StatusValue = dbcontent.Dictionaries.Where(a => a.HostId == hostId && a.KeyValue == model.Status && a.Identifier == "MemberStatus").FirstOrDefault().Contents,
                              Type = model.Type,
                              BookTime = model.BookTime,
                              SalesmanId = model.SalesmanId,
                              Salesman = dbcontent.Users.Where(t => t.Id == model.SalesmanId).FirstOrDefault(),
                              BeauticianId = model.BeauticianId,
                              Beautician = dbcontent.Users.Where(t => t.Id == model.SalesmanId).FirstOrDefault(),

                              // 个人信息
                              Name = model.Name,
                              Sex = dbcontent.Dictionaries.Where(a => a.KeyValue == model.Sex && a.Identifier == "MemberGender").FirstOrDefault().Contents,
                              Birthday = model.Birthday,
                              Vocation = dbcontent.Dictionaries.Where(a => a.KeyValue == model.Vocation && a.Identifier == "MemberVocation").FirstOrDefault().Contents,
                              Position = model.Position,
                              MaritalStatus = model.MaritalStatus,
                              Conjugal = model.Conjugal,
                              WeddingDay = model.WeddingDay,
                              MobileNumber = model.MobileNumber,
                              Email = model.Email,
                              Address = model.Address,
                              TencentQQ = model.TencentQQ,
                              WebChat = model.WebChat,
                              Company = model.Company,
                              CompanyAddress = model.CompanyAddress,

                              Personality = model.Personality,
                              FacialDemand = model.FacialDemand,
                              BodyDemand = model.BodyDemand,
                              CustomerDemand = model.CustomerDemand,
                              ConsumptionHabit = model.ConsumptionHabit,
                              SkinConditions = model.SkinConditions,
                              SkinType = model.SkinType
                          }).FirstOrDefault();

            if (entity != null)
            {
                entity.Cards = (from _ in dbcontent.MemberCards
                                where _.MemberID == id && _.Status == 1
                                select new MemberCardModel
                                {
                                    MemberCardId = _.MemberCardId,
                                    MemberID = _.MemberID,
                                    Title = _.Title,
                                    Type = _.Type,
                                    Amt = _.Amt,
                                    UsedTime = _.UsedTime,
                                    BookTime = _.BookTime,
                                    LastCount = _.LastCount,
                                    Amount = _.Amount,
                                    Status = _.Status,
                                    ActualPrice = _.ActualPrice,
                                    CreatedDate = _.CreatedDate,
                                    ExpiryDate = _.ExpiryDate,
                                    Record = dbcontent.AccountRecords.Where(t => t.MemberCardId == _.MemberCardId).FirstOrDefault()
                                }).ToList();

                //entity.Feedbacks = service.GetFeedbacks(id);             // 客户回访
                entity.ExpenseProjects = service.GetExpenseProjects(id);   // 消耗项目
                entity.RechargeRecords = service.GetRechargeRecords(id);   // 充值记录
                entity.ExpenseRecords = service.GetExpenseRecords(id);     // 卡扣记录
                entity.UsableProjects = service.GetAllProjects(id);        // 可用项目

                entity.BookTime = (from ar in dbcontent.AccountRecords.Where(t => t.MemberID == id)
                                   select new { yy = ar.CreatedDate.Year, mm = ar.CreatedDate.Month, dd = ar.CreatedDate.Day })
                                   .Union(
                                    from ar in dbcontent.Books.Where(t => t.MemberID == id)
                                    select new { yy = ar.CreatedDate.Year, mm = ar.CreatedDate.Month, dd = ar.CreatedDate.Day })
                                   .Distinct().Count();

                //统计
                //今年卡扣额
                var p1 = dbcontent.AccountRecords.Where(t => t.Type == "3" && t.SalesType == 1 && t.CreatedDate.Year == DateTime.Now.Year && t.MemberID == id);
                entity.ConsumptionThisYear = p1.Count() > 0 ? p1.Sum(t => t.OutAmount) : 0;
                // 今年卡扣频次
                //var p2 = dbcontent.AccountRecords.Where(t => t.Type == 3 && t.CreatedDate.Year == DateTime.Now.Year && t.MemberID == id);
                entity.ConsumptionHzThisYear = p1.Count() > 0 ? p1.Count() : 0;
                //去年卡扣金额
                var p3 = dbcontent.AccountRecords.Where(t => t.Type == "3" && t.SalesType == 1 && t.CreatedDate.Year == DateTime.Now.Year - 1 && t.MemberID == id);
                entity.ConsumptionLastYear = p3.Count() > 0 ? p3.Sum(t => t.OutAmount) : 0;
                // 去年卡扣频次
                //var p4 = dbcontent.AccountRecords.Where(t => t.Type == 3 && t.CreatedDate.Year == DateTime.Now.Year - 1 && t.MemberID == id).Select(t => new { t.CreatedDate.Month, t.CreatedDate.Day }).Distinct();
                entity.ConsumptionHzLastYear = p3.Count() > 0 ? p3.Count() : 0;
                // 最后服务时间
                var p5 = dbcontent.Books.Where(t => t.MemberID == id).OrderByDescending(t => t.CreatedDate);
                entity.LastServiceDate = p5.Count() > 0 ? p5.First().CreatedDate : default(DateTime);
                var p6 = dbcontent.MemberProjects.Where(t => t.MemberID == id);
                entity.RemainedProject = p6.Count() > 0 ? p6.Sum(t => t.LastCount) : 0;

                //新增回访
                //entity.Feedback = new Feedback { MemberId = id, CallUserId = userId };
                ViewData["MemberID"] = id;
            }

            //ViewBag.FeedbackType = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "FeedbackType").ToList(), "KeyValue", "Contents");
            //ViewBag.FeedbackWay = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "FeedbackWay").ToList(), "KeyValue", "Contents");

            ViewBag.MemberType = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberType").ToSelectListFor(t => t.KeyValue, t => t.Contents);
            // 等级
            ViewBag.MemberLevel = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberLevel" && t.HostId == hostId).ToSelectListFor(t => t.KeyValue, t => t.Contents);
            // 状态
            ViewBag.MemberStatus = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberStatus" && t.HostId == hostId).ToSelectListFor(t => t.KeyValue, t => t.Contents);
            // 职业
            ViewBag.MemberVocation = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberVocation").ToSelectListFor(t => t.KeyValue, t => t.Contents);
            // 来源
            ViewBag.MemberSource = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberSource").ToSelectListFor(t => t.KeyValue, t => t.Contents);
            // 性别
            ViewBag.Genders = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberGender").ToSelectListFor(t => t.KeyValue, t => t.Contents);
            // 婚姻状况
            ViewBag.MaritalStatus = dbcontent.Dictionaries.Where(t => t.Identifier == "MaritalStatus").ToSelectListFor(t => t.KeyValue, t => t.Contents);

            // 皮肤类型
            ViewBag.SkinTypes = dbcontent.Dictionaries.Where(t => t.Identifier == "SkinType").ToSelectListFor(t => t.Contents, t => t.Contents);
            // 肌肤状况
            ViewBag.SkinConditions = dbcontent.Dictionaries.Where(t => t.Identifier == "SkinConditions").ToSelectListFor(t => t.Contents, t => t.Contents);
            // 面部需求
            ViewBag.FacialDemands = dbcontent.Dictionaries.Where(t => t.Identifier == "FacialDemand").ToSelectListFor(t => t.Contents, t => t.Contents);
            // 身体需求
            ViewBag.BodyDemands = dbcontent.Dictionaries.Where(t => t.Identifier == "BodyDemand").ToSelectListFor(t => t.Contents, t => t.Contents);
            // 客户需求
            ViewBag.CustomerDemands = dbcontent.Dictionaries.Where(t => t.Identifier == "CustomerDemand").ToSelectListFor(t => t.Contents, t => t.Contents);
            // 消费习惯
            ViewBag.ConsumptionHabits = dbcontent.Dictionaries.Where(t => t.Identifier == "ConsumptionHabit").ToSelectListFor(t => t.Contents, t => t.Contents);
            // 性格
            ViewBag.Personalitys = dbcontent.Dictionaries.Where(t => t.Identifier == "Personality").ToSelectListFor(t => t.Contents, t => t.Contents);

            return View(entity);
        }


        // GET: MemberAdmin/Create
        public ActionResult Create()
        {
            InitDrop();
            return View();
        }

        // POST: MemberAdmin/Create
        [HttpPost]
        public ActionResult Create(MemberViewModel model)
        {
            var hostId = GlobalContext.Current.UserHost.HostID;
            try
            {
                Member entity = new Member
                {
                    // 会员信息
                    HostID = hostId,
                    Passwd = (string.IsNullOrEmpty(model.Passwd) ? "888888" : model.Passwd),
                    Source = model.Source,
                    JoinBranch = Convert.ToInt32(model.JoinBranch),
                    JoinDate = model.JoinDate,
                    CardNo = model.CardNo,
                    Type = model.Type,
                    SalesmanId = model.SalesmanId,
                    BeauticianId = model.BeauticianId,

                    // 个人信息
                    Name = model.Name,
                    Sex = model.Sex,
                    Birthday = model.Birthday,
                    Vocation = model.Vocation,
                    Position = model.Position,
                    MaritalStatus = model.MaritalStatus,
                    Conjugal = model.Conjugal,
                    WeddingDay = model.WeddingDay,
                    MobileNumber = model.MobileNumber,
                    Email = model.Email,
                    Address = model.Address,
                    TencentQQ = model.TencentQQ,
                    WebChat = model.WebChat,
                    Company = model.Company,
                    CompanyAddress = model.CompanyAddress,
                    CreatedDate = DateTime.Now,
                    CreatedBy = GlobalContext.Current.UserInfo.Id,
                    IsNew = -1
                };

                if (model.PersonalityE != null)
                    entity.Personality = string.Join(",", model.PersonalityE);
                if (model.FacialDemandE != null)
                    entity.FacialDemand = string.Join(",", model.FacialDemandE);
                if (model.BodyDemandE != null)
                    entity.BodyDemand = string.Join(",", model.BodyDemandE);
                if (model.CustomerDemandE != null)
                    entity.CustomerDemand = string.Join(",", model.CustomerDemandE);
                if (model.ConsumptionHabitE != null)
                    entity.ConsumptionHabit = string.Join(",", model.ConsumptionHabitE);
                if (model.SkinConditionE != null)
                    entity.SkinConditions = string.Join(",", model.SkinConditionE);
                if (model.SkinTypeE != null)
                    entity.SkinType = string.Join(",", model.SkinTypeE);


                dbcontent.Members.Add(entity);
                dbcontent.SaveChanges();

                // 添加现金流通卡
                MemberCard card = new MemberCard
                {
                    MemberID = entity.MemberID,
                    HostID = hostId,
                    BranchID = model.JoinBranch.Value,
                    Amount = 0,
                    ActualPrice = 0,
                    Amt = 0,
                    Status = 1,
                    Title = "现金通道",
                    Type = "9",
                    CreatedDate = DateTime.Now,
                };
                dbcontent.MemberCards.Add(card);
                dbcontent.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error("客户保存失败", ex);
                InitDrop();
                ModelState.AddModelError("UserName", "客户保存失败.");
                return View();
            }
        }

        // GET: MemberAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            InitDrop();
            var hostId = GlobalContext.Current.UserHost.HostID;
            var model = (from entity in dbcontent.Members.Where(t => t.HostID == hostId && t.MemberID == id)
                         select new MemberViewModel
                         {
                             MemberID = entity.MemberID,
                             Source = entity.Source,
                             CardNo = entity.CardNo,
                             Type = entity.Type,
                             //Passwd = model.Passwd,
                             JoinBranch = entity.JoinBranch,
                             JoinDate = entity.JoinDate,
                             SalesmanId = entity.SalesmanId,
                             BeauticianId = entity.BeauticianId,
                             // 个人信息
                             Name = entity.Name,
                             Sex = entity.Sex,
                             Birthday = entity.Birthday,
                             Vocation = entity.Vocation,
                             Address = entity.Address,
                             Company = entity.Company,
                             CompanyAddress = entity.CompanyAddress,
                             Position = entity.Position,
                             MaritalStatus = entity.MaritalStatus,
                             Conjugal = entity.Conjugal,
                             WeddingDay = entity.WeddingDay,
                             MobileNumber = entity.MobileNumber,
                             WebChat = entity.WebChat,
                             TencentQQ = entity.TencentQQ,
                             Email = entity.Email
                         }).FirstOrDefault();

            if (model != null)
            {
                // 新增属性
                if (model.Personality != null)
                    model.PersonalityE = model.Personality.Split(',').ToArray();
                if (model.FacialDemand != null)
                    model.FacialDemandE = model.FacialDemand.Split(',').ToArray();
                if (model.BodyDemand != null)
                    model.BodyDemandE = model.BodyDemand.Split(',').ToArray();
                if (model.CustomerDemand != null)
                    model.CustomerDemandE = model.CustomerDemand.Split(',').ToArray();
                if (model.ConsumptionHabit != null)
                    model.ConsumptionHabitE = model.ConsumptionHabit.Split(',').ToArray();
                if (model.SkinConditions != null)
                    model.SkinConditionE = model.SkinConditions.Split(',').ToArray();
                if (model.SkinType != null)
                    model.SkinTypeE = model.SkinType.Split(',').ToArray();
            }

            return View(model);
        }

        // POST: MemberAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(MemberViewModel model)
        {
            try
            {
                Member entity = dbcontent.Members.Where(a => a.MemberID == model.MemberID).FirstOrDefault();
                entity.Source = model.Source;
                //entity.CardNo = entity.CardNo;
                entity.Passwd = model.Passwd;
                entity.Type = model.Type;
                entity.JoinBranch = model.JoinBranch;
                entity.JoinDate = model.JoinDate;
                entity.SalesmanId = model.SalesmanId;
                entity.BeauticianId = model.BeauticianId;
                //个人信息
                entity.Name = model.Name;
                entity.Sex = model.Sex;
                entity.Birthday = model.Birthday;
                entity.Vocation = model.Vocation;
                entity.Position = model.Position;
                entity.Address = model.Address;
                entity.MaritalStatus = model.MaritalStatus;
                entity.Conjugal = model.Conjugal;
                entity.WeddingDay = model.WeddingDay;
                entity.MobileNumber = model.MobileNumber;
                entity.Email = model.Email;
                entity.WebChat = model.WebChat;
                entity.TencentQQ = model.TencentQQ;
                entity.Company = model.Company;
                entity.CompanyAddress = model.CompanyAddress;

                // 新增属性
                if (model.PersonalityE != null)
                    entity.Personality = string.Join(",", model.PersonalityE);
                if (model.FacialDemandE != null)
                    entity.FacialDemand = string.Join(",", model.FacialDemandE);
                if (model.BodyDemandE != null)
                    entity.BodyDemand = string.Join(",", model.BodyDemandE);
                if (model.CustomerDemandE != null)
                    entity.CustomerDemand = string.Join(",", model.CustomerDemandE);
                if (model.ConsumptionHabitE != null)
                    entity.ConsumptionHabit = string.Join(",", model.ConsumptionHabitE);
                if (model.SkinConditionE != null)
                    entity.SkinConditions = string.Join(",", model.SkinConditionE);
                if (model.SkinTypeE != null)
                    entity.SkinType = string.Join(",", model.SkinType);

                dbcontent.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error("客户保存失败", ex);
                InitDrop();
                ModelState.AddModelError("UserName", "客户保存失败.");
                return View();
            }
        }

        /// <summary>
        /// 初始化下拉菜单
        /// </summary>
        private void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            // 门店
            ViewBag.OrganId = new SelectList(dbcontent.Organs.Where(t => t.HostID == GlobalContext.Current.UserHost.HostID && t.IsVaild == 1).ToList(), "OrganID", "Name");
            // 顾问
            ViewBag.Salesman = new SelectList(dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type == "3" && t.Status == "1").ToList(), "Id", "UserCnName");
            // 美容师
            ViewBag.Beautician = new SelectList(dbcontent.Users.Where(t => t.HostId == GlobalContext.Current.UserHost.HostID && t.Type == "1" && t.Status == "1").ToList(), "Id", "UserCnName");
            // 类型
            ViewBag.MemberType = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "MemberType").ToList(), "KeyValue", "Contents");
            // 等级
            ViewBag.MemberLevel = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "MemberLevel" && t.HostId == hostId).ToList(), "KeyValue", "Contents");
            // 状态
            ViewBag.MemberStatus = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "MemberStatus" && t.HostId == hostId).ToList(), "KeyValue", "Contents");
            // 职业
            ViewBag.MemberVocation = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "MemberVocation").ToList(), "KeyValue", "Contents");
            // 来源
            ViewBag.MemberSource = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "MemberSource").ToList(), "KeyValue", "Contents");
            // 性别
            ViewBag.Genders = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "MemberGender").ToList(), "KeyValue", "Contents");
            // 婚姻状况
            ViewBag.MaritalStatus = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "MaritalStatus").ToList(), "KeyValue", "Contents");

            // 皮肤类型
            ViewBag.SkinTypes = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "SkinType").ToList(), "Contents", "Contents");
            // 肌肤状况
            ViewBag.SkinConditions = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "SkinConditions").ToList(), "Contents", "Contents");
            // 面部需求
            ViewBag.FacialDemands = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "FacialDemand").ToList(), "Contents", "Contents");
            // 身体需求
            ViewBag.BodyDemands = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "BodyDemand").ToList(), "Contents", "Contents");
            // 客户需求
            ViewBag.CustomerDemands = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "CustomerDemand").ToList(), "Contents", "Contents");
            // 消费习惯
            ViewBag.ConsumptionHabits = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "ConsumptionHabit").ToList(), "Contents", "Contents");
            // 性格
            ViewBag.Personalitys = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "Personality").ToList(), "Contents", "Contents");

        }

        // GET: MemberAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MemberAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult BatchClean(string records)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };

            using (var dbtran = new TransactionScope(TransactionScopeOption.Required))
            {
                var list = records.Split(',').ToArray();
                foreach (var id in list)
                {
                    if (string.IsNullOrEmpty(id)) continue;
                    long memberid = long.Parse(id);

                    var row1 = dbcontent.AccountRecords.Where(t => t.MemberID == memberid).Delete();
                    var row2 = dbcontent.MemberCards.Where(t => t.MemberID == memberid).Delete();
                    var row3 = dbcontent.MemberGives.Where(t => t.MemberID == memberid).Delete();
                    var row4 = dbcontent.MemberProjects.Where(t => t.MemberID == memberid).Delete();
                    var row5 = dbcontent.Appointments.Where(t => t.MemberID == memberid).Delete();
                    var row6 = dbcontent.Books.Where(t => t.MemberID == memberid).Delete();
                    var row7 = dbcontent.PointBooks.Where(t => t.MemberId == memberid).Delete();
                    var row8 = dbcontent.RedeemProjects.Where(t => t.MemberId == memberid).Delete();
                    var row9 = dbcontent.Members.Where(t => t.MemberID == memberid).Update(t => new Member { Amt = 0, Points = 0 });

                    var log = new EventLog
                    {
                        HostId = GlobalContext.Current.UserHost.HostID,
                        BranchId = GlobalContext.Current.UserDepartment.OrganID,
                        CreatedDate = DateTime.Now,
                        MemberId = memberid,
                        Level = 5,
                        Content = "客户数据清理",
                        TypeId = 99,
                        UserId = GlobalContext.Current.UserInfo.Id
                    };
                    dbcontent.EventLogs.Add(log);
                }
                dbcontent.SaveChanges();
                dbtran.Complete();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 客户列表导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult XlsImport(FormCollection formValues)
        {
            string EventType = formValues["Type"];

            try
            {
                StringBuilder msg = new StringBuilder();
                int HostID = GlobalContext.Current.UserHost.HostID;
                var file = Request.Files["postedFile"];

                if (null != file && file.ContentLength > 0)
                {
                    var workBook = new HSSFWorkbook(file.InputStream);
                    HSSFSheet sheet = workBook.GetSheet("Sheet1");

                    //取行Excel的最大行数
                    int rowsCount = sheet.PhysicalNumberOfRows;
                    int colsCount = sheet.GetRow(0).PhysicalNumberOfCells;
                    int row = 0;
                    var allBranch = dbcontent.Organs.Where(t => t.HostID == HostID).ToList();
                    var allGender = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberGender").ToList();
                    var allVocation = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberVocation").ToList();
                    var allSource = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberSource").ToList();
                    var allSales = dbcontent.Users.Where(t => t.HostId == HostID && t.Type == "3").ToList();
                    var allType = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberType").ToList();
                    var allMeirong = dbcontent.Users.Where(t => t.HostId == HostID && t.Type == "1").ToList();
                    var allCardType = dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "MemberCardType").ToList();
                    var allPrj = dbcontent.Projects.Where(t => t.HostID == HostID).Select(t => new { KeyValue = t.ProjectID, Contents = t.Name }).ToList();

                    Member NewMember = null;
                    MemberCard NewMemberCard = null;
                    MemberCard CashCard = null;
                    string CardType = "0";

                    for (int rowIndex = 1; rowIndex < rowsCount; rowIndex++)
                    {
                        logger.Info("Import Row" + rowIndex);

                        string branch = (sheet.GetRow(rowIndex).GetCell(0) != null ? sheet.GetRow(rowIndex).GetCell(0).ToString().Trim() : "");    // 门店
                        string cardNo = (sheet.GetRow(rowIndex).GetCell(1) != null ? sheet.GetRow(rowIndex).GetCell(1).ToString().Trim() : "");    // 卡号
                        string name = (sheet.GetRow(rowIndex).GetCell(2) != null ? sheet.GetRow(rowIndex).GetCell(2).ToString().Trim() : "");      // 姓名
                        string mobile = (sheet.GetRow(rowIndex).GetCell(3) != null ? sheet.GetRow(rowIndex).GetCell(3).ToString().Trim() : "");      // 手机号
                        string sex = (sheet.GetRow(rowIndex).GetCell(4) != null ? sheet.GetRow(rowIndex).GetCell(4).ToString().Trim() : "");         // 性别
                        var birthCell = sheet.GetRow(rowIndex).GetCell(5);
                        string birth = (birthCell != null ? (birthCell.CellType == 0 ? birthCell.ToString() : "") : "");      // 生日
                        string address = (sheet.GetRow(rowIndex).GetCell(6) != null ? sheet.GetRow(rowIndex).GetCell(6).ToString().Trim() : "");     // 地址
                        string vocation = (sheet.GetRow(rowIndex).GetCell(7) != null ? sheet.GetRow(rowIndex).GetCell(7).ToString().Trim() : "");    // 职业
                        string source = (sheet.GetRow(rowIndex).GetCell(8) != null ? sheet.GetRow(rowIndex).GetCell(8).ToString().Trim() : "");     // 来源
                        string type = (sheet.GetRow(rowIndex).GetCell(9) != null ? sheet.GetRow(rowIndex).GetCell(9).ToString().Trim() : "");       // 类型 体验/正式
                        string sales = (sheet.GetRow(rowIndex).GetCell(10) != null ? sheet.GetRow(rowIndex).GetCell(10).ToString().Trim() : "");    // 顾问归属
                        string beautician = (sheet.GetRow(rowIndex).GetCell(11) != null ? sheet.GetRow(rowIndex).GetCell(11).ToString().Trim() : "");   // 美容师归属
                        string join = (sheet.GetRow(rowIndex).GetCell(12) != null ? sheet.GetRow(rowIndex).GetCell(12).ToString().Trim() : "");     // 入会时间
                        string point = (sheet.GetRow(rowIndex).GetCell(13) != null ? sheet.GetRow(rowIndex).GetCell(13).ToString().Trim() : "");     // 客户积分

                        var cardTitleCell = sheet.GetRow(rowIndex).GetCell(14);
                        string cardTitle = (cardTitleCell != null ? (cardTitleCell.CellType == 1 ? cardTitleCell.ToString() : cardTitleCell.StringCellValue) : "");  // 卡项标题
                        string cardType = (sheet.GetRow(rowIndex).GetCell(15) != null ? sheet.GetRow(rowIndex).GetCell(15).ToString().Trim() : "");     // 卡项类型
                        var cardAmountCell = sheet.GetRow(rowIndex).GetCell(16);
                        string cardAmount = (cardAmountCell != null ? (cardAmountCell.CellType == 1 ? cardAmountCell.ToString() : cardAmountCell.NumericCellValue.ToString()) : "");   // 购买金额
                        var cardTimeCell = sheet.GetRow(rowIndex).GetCell(17);
                        string cardTime = (cardTimeCell != null ? (cardTimeCell.CellType == 1 ? cardTimeCell.ToString() : cardTimeCell.NumericCellValue.ToString()) : "");
                        //string cardTime = (sheet.GetRow(rowIndex).GetCell(17) != null ? sheet.GetRow(rowIndex).GetCell(17).ToString().Trim() : "");    // 可用次数
                        string cardLimit = (sheet.GetRow(rowIndex).GetCell(18) != null ? sheet.GetRow(rowIndex).GetCell(18).ToString().Trim() : "");   // 有效时间
                        string cardStart = (sheet.GetRow(rowIndex).GetCell(19) != null ? sheet.GetRow(rowIndex).GetCell(19).ToString().Trim() : "");   // 购买时间

                        string prjName = (sheet.GetRow(rowIndex).GetCell(20) != null ? sheet.GetRow(rowIndex).GetCell(20).ToString().Trim() : "");    // 项目名称
                        string prjTime = (sheet.GetRow(rowIndex).GetCell(21) != null ? sheet.GetRow(rowIndex).GetCell(21).ToString().Trim() : "");     // 可用次数
                        var prjPriceCell = sheet.GetRow(rowIndex).GetCell(22);
                        string prjPrice = (prjPriceCell != null ? (prjPriceCell.CellType == 1 ? prjPriceCell.ToString() : prjPriceCell.NumericCellValue.ToString()) : "");    // 购买单价
                        string prjGive = (sheet.GetRow(rowIndex).GetCell(23) != null ? sheet.GetRow(rowIndex).GetCell(23).ToString().Trim() : "");     // 是否赠送


                        if (EventType == "ImportCheck")
                        {
                            // 一段
                            if (!string.IsNullOrEmpty(branch))
                            {
                                if (string.IsNullOrEmpty(name))
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，姓名为空；<br />");
                                    continue;
                                }
                                if (string.IsNullOrEmpty(cardNo))
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，卡号为空；<br />");
                                    continue;
                                }
                                if (string.IsNullOrEmpty(type))
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，类别为空；<br />");
                                    continue;
                                }

                                // 卡号重复检测
                                int r = dbcontent.Members.Where(t => t.CardNo == cardNo && t.HostID == HostID).Count();
                                if (r > 0)
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，卡号已存在；<br />");
                                    continue;
                                }
                                r = dbcontent.Members.Where(t => t.MobileNumber == mobile && t.HostID == HostID).Count();
                                if (r > 0)
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，手机号已存在；<br />");
                                    continue;
                                }
                                Organ c = allBranch.Where(t => t.Name == branch).FirstOrDefault();
                                if (c == null)
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，门店名称不匹配；<br />");
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(sex))
                                {
                                    Dictionary s = allGender.Where(t => t.Contents == sex).FirstOrDefault();
                                    if (s == null)
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，性别不匹配；<br />");
                                        continue;
                                    }
                                }
                                if (!string.IsNullOrEmpty(vocation))
                                {
                                    Dictionary b = allVocation.Where(t => t.Contents == vocation).FirstOrDefault();
                                    if (b == null)
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，职业不匹配；<br />");
                                        continue;
                                    }
                                    vocation = b.KeyValue;
                                }
                                if (!string.IsNullOrEmpty(source))
                                {
                                    Dictionary b = allSource.Where(t => t.Contents == source).FirstOrDefault();
                                    if (b == null)
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，来源不匹配；<br />");
                                        continue;
                                    }
                                    source = b.KeyValue;
                                }
                                Dictionary d = allType.Where(t => t.Contents == type).FirstOrDefault();
                                if (d == null)
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，类别不匹配；<br />");
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(sales))
                                {
                                    var v = allSales.Where(t => t.UserCnName == sales).FirstOrDefault();
                                    if (v == null)
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，输入的顾问不匹配；<br />");
                                        continue;
                                    }
                                }
                                //else
                                //{
                                //    msg.Append("第" + (rowIndex + 1) + @"行，顾问不能为空；<br />");
                                //    continue;
                                //}
                                if (!string.IsNullOrEmpty(beautician))
                                {
                                    var u = allMeirong.Where(t => t.UserCnName == beautician).FirstOrDefault();
                                    if (u == null)
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，输入的美容师不匹配；<br />");
                                        continue;
                                    }
                                }
                                //else
                                //{
                                //    msg.Append("第" + (rowIndex + 1) + @"行，美容师归属不能空；<br />");
                                //    continue;
                                //}
                                if (!string.IsNullOrEmpty(birth))
                                {
                                    DateTime jj;
                                    if (!DateTime.TryParseExact(birth, "M/d/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out jj))
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，生日不匹配；<br />");
                                        continue;
                                    }
                                }
                                if (!string.IsNullOrEmpty(join))
                                {
                                    DateTime jj;
                                    if (!DateTime.TryParseExact(join, "M/d/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out jj))
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，入会时间不匹配；<br />");
                                        continue;
                                    }
                                }

                                if (!string.IsNullOrEmpty(point))
                                {
                                    Regex rgx = new Regex("^[0-9]+$");
                                    if (!rgx.IsMatch(point))
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，客户积分不是整数；<br />");
                                        continue;
                                    }
                                }
                            }

                            // 二段
                            if (!string.IsNullOrEmpty(cardTitle))
                            {
                                Dictionary s = allCardType.Where(t => t.Contents == cardType).FirstOrDefault();
                                if (s == null)
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，卡项类型不匹配；<br />");
                                    continue;
                                }
                                else
                                {
                                    CardType = s.KeyValue;
                                }
                                if (string.IsNullOrEmpty(cardAmount))
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，卡项购买金额不能空；<br />");
                                    continue;
                                }
                                else
                                {
                                    Regex objNotNumberPattern = new Regex("^(-?\\d+)(\\.\\d+)?$");
                                    if (!objNotNumberPattern.IsMatch(cardAmount))
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，购卡金额不是数字；<br />");
                                        continue;
                                    }
                                }
                                if (!string.IsNullOrEmpty(cardLimit))
                                {
                                    DateTime jj;
                                    if (!DateTime.TryParseExact(cardLimit, "M/d/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out jj))
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，卡项有效时间格式不对；<br />");
                                        continue;
                                    }
                                }
                                if (!string.IsNullOrEmpty(cardStart))
                                {
                                    DateTime jj;
                                    if (!DateTime.TryParseExact(cardStart, "M/d/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out jj))
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，卡项购买时间格式不对；<br />");
                                        continue;
                                    }
                                }
                            }

                            // 三段
                            if (!string.IsNullOrEmpty(prjName))
                            {
                                var s = allPrj.Where(t => t.Contents == prjName).FirstOrDefault();
                                if (s == null)
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，项目名称不匹配；<br />");
                                    continue;
                                }
                                if (CardType != "4")
                                {
                                    if (string.IsNullOrEmpty(prjTime))
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，项目次数不能空；<br />");
                                        continue;
                                    }
                                    else
                                    {
                                        Regex rgx = new Regex("^[0-9]+$");
                                        if (!rgx.IsMatch(prjTime))
                                        {
                                            msg.Append("第" + (rowIndex + 1) + @"行，项目次数不是整数；<br />");
                                            continue;
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(prjPrice))
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，项目单价不能空；<br />");
                                    continue;
                                }
                                else
                                {
                                    Regex objNotNumberPattern = new Regex("^(-?\\d+)(\\.\\d+)?$");
                                    if (!objNotNumberPattern.IsMatch(prjPrice))
                                    {
                                        msg.Append("第" + (rowIndex + 1) + @"行，项目单价不是数字；<br />");
                                        continue;
                                    }
                                }
                            }
                        }
                        else if (EventType == "Import")
                        {
                            // 添加客户
                            if (!string.IsNullOrEmpty(cardNo))
                            {
                                Member mb = new Member
                                {
                                    HostID = HostID,
                                    CardNo = cardNo,
                                    Name = name,
                                    MobileNumber = mobile,
                                    Address = address,

                                    JoinBranch = allBranch.Where(t => t.Name == branch).FirstOrDefault().OrganID,
                                    Type = allType.Where(t => t.Contents == type).FirstOrDefault().KeyValue,
                                    //SalesmanId = allSales.Where(t => t.UserCnName == sales).FirstOrDefault().Id,
                                    //BeauticianId = allMeirong.Where(t => t.UserCnName == beautician).FirstOrDefault().Id,
                                    Amt = 0,
                                    BookTime = 0,
                                    Points = 0,
                                    IsNew = -1,
                                    CreatedDate = DateTime.Now,
                                    Passwd = "888888"
                                };
                                if (!string.IsNullOrEmpty(sex))
                                {
                                    mb.Sex = allGender.Where(t => t.Contents == sex).FirstOrDefault().KeyValue;
                                }
                                if (!string.IsNullOrEmpty(birth))
                                {
                                    mb.Birthday = DateTime.ParseExact(birth, "M/d/yy", CultureInfo.InvariantCulture);
                                }
                                if (!string.IsNullOrEmpty(join))
                                {
                                    mb.JoinDate = DateTime.ParseExact(join, "M/d/yy", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    mb.JoinDate = DateTime.Today;
                                }
                                if (!string.IsNullOrEmpty(sales))
                                {
                                    var v = allSales.Where(t => t.UserCnName == sales).FirstOrDefault();
                                    if (v != null)
                                    {
                                        mb.SalesmanId = v.Id;
                                    }
                                }
                                if (!string.IsNullOrEmpty(beautician))
                                {
                                    var u = allMeirong.Where(t => t.UserCnName == beautician).FirstOrDefault();
                                    if (u != null)
                                    {
                                        mb.BeauticianId = u.Id;
                                    }
                                }
                                if (!string.IsNullOrEmpty(point))
                                {
                                    mb.Points = Convert.ToInt32(point);
                                }
                                NewMember = dbcontent.Members.Add(mb);
                                dbcontent.SaveChanges();



                                // 添加现金流通卡
                                MemberCard card = new MemberCard
                                {
                                    MemberID = NewMember.MemberID,
                                    HostID = NewMember.HostID,
                                    BranchID = NewMember.JoinBranch.Value,
                                    Amount = 0,
                                    ActualPrice = 0,
                                    Amt = 0,
                                    Status = 1,
                                    Title = "现金通道",
                                    Type = "9",
                                    CreatedDate = DateTime.Now,
                                };
                                CashCard = dbcontent.MemberCards.Add(card);
                                dbcontent.SaveChanges();
                            }

                            // 同行有没有卡项数据
                            if (string.IsNullOrEmpty(cardTitle) == false && NewMember != null)
                            {
                                MemberCard card = new MemberCard
                                {
                                    Type = allCardType.Where(t => t.Contents == cardType).FirstOrDefault().KeyValue,
                                    Amount = Convert.ToDecimal(cardAmount),
                                    Status = 1,
                                    DebtFlag = 0,
                                    DebtStatus = 0,
                                    HostID = NewMember.HostID,
                                    BranchID = NewMember.JoinBranch.Value,
                                    ActualPrice = Convert.ToDecimal(cardAmount),
                                    MemberID = NewMember.MemberID,
                                    Title = cardTitle
                                };
                                if (!string.IsNullOrEmpty(cardTime.Trim()))
                                {
                                    card.BookTime = Convert.ToInt32(cardTime);
                                    card.UsedTime = 0;
                                    card.LastCount = Convert.ToInt32(cardTime);
                                }

                                var amt = Convert.ToDecimal(cardAmount);
                                if (amt < 0)
                                {
                                    // 欠款
                                    card.DebtFlag = 1;
                                    card.DebtStatus = 1;
                                    card.ActualPrice = 0;
                                    card.Amt = 0;
                                    card.Amount = -amt;
                                }
                                if (card.Type == "0" || card.Type == "6")
                                {
                                    // 无欠
                                    card.Amt = card.Amount;
                                }

                                if (!string.IsNullOrEmpty(cardStart))
                                {
                                    card.CreatedDate = DateTime.ParseExact(cardStart, "M/d/yy", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    card.CreatedDate = DateTime.Now;
                                }
                                if (!string.IsNullOrEmpty(cardLimit))
                                {
                                    card.ExpiryDate = DateTime.ParseExact(cardLimit, "M/d/yy", CultureInfo.InvariantCulture);
                                }
                                NewMemberCard = dbcontent.MemberCards.Add(card);
                                dbcontent.SaveChanges();
                            }

                            // 同行有没有卡项数据
                            var prj = allPrj.Where(t => t.Contents == prjName).FirstOrDefault();
                            if (!string.IsNullOrEmpty(prjName) && NewMemberCard != null && prj != null)
                            {
                                if ((new string[] { "4", "5", "8" }).Contains(NewMemberCard.Type))
                                {
                                    MemberCardProject mcp = new MemberCardProject
                                    {
                                        MemberCardId = NewMemberCard.MemberCardId,
                                        ProjectID = prj.KeyValue,
                                        UnitPrice = Convert.ToDecimal(prjPrice)
                                    };
                                    dbcontent.MemberCardProjects.Add(mcp);
                                }
                                else if (prjGive == "是")
                                {
                                    MemberProject mp = new MemberProject
                                    {
                                        HostID = NewMember.HostID,
                                        BranchId = NewMember.JoinBranch.Value,
                                        GiveId = 0,
                                        IsEntity = 0,
                                        BookTime = Convert.ToInt32(prjTime),
                                        UsedTime = 0,
                                        Amount = Convert.ToInt32(prjTime) * Convert.ToDecimal(prjPrice),
                                        UnitPrice = Convert.ToDecimal(prjPrice),
                                        CreatedDate = DateTime.Now,
                                        DebtFlag = 0,
                                        LastCount = Convert.ToInt32(prjTime),
                                        status = 0,
                                        MemberID = NewMember.MemberID,
                                        MemberCardId = CashCard.MemberCardId,
                                        Type = "1",
                                        ActualPrice = Convert.ToInt32(prjTime) * Convert.ToDecimal(prjPrice),
                                        ProjectID = prj.KeyValue,
                                        IsVaild = 1
                                    };

                                    dbcontent.MemberProjects.Add(mp);
                                }
                                else
                                {
                                    MemberProject mp = new MemberProject
                                    {
                                        HostID = NewMember.HostID,
                                        BranchId = NewMember.JoinBranch.Value,
                                        GiveId = 0,
                                        IsEntity = 0,
                                        BookTime = Convert.ToInt32(prjTime),
                                        UsedTime = 0,
                                        Amount = Convert.ToInt32(prjTime) * Convert.ToDecimal(prjPrice),
                                        UnitPrice = Convert.ToDecimal(prjPrice),
                                        CreatedDate = NewMemberCard.CreatedDate,
                                        DebtFlag = 0,
                                        LastCount = Convert.ToInt32(prjTime),
                                        status = 0,
                                        MemberID = NewMember.MemberID,
                                        MemberCardId = NewMemberCard.MemberCardId,
                                        Type = "0",
                                        ActualPrice = Convert.ToInt32(prjTime) * Convert.ToDecimal(prjPrice),
                                        ProjectID = prj.KeyValue,
                                        IsVaild = 1
                                    };

                                    dbcontent.MemberProjects.Add(mp);
                                }
                            }

                            dbcontent.SaveChanges();
                        }
                        else if (EventType == "AppendCheck")
                        {
                            // 卡号检测
                            //if (string.IsNullOrEmpty(cardNo))
                            //{
                            //    msg.Append("第" + (rowIndex + 1) + @"行，卡号为空；<br />");
                            //    continue;
                            //}
                            //else
                            //{
                            if (!string.IsNullOrEmpty(cardNo))
                            {
                                var mb = dbcontent.Members.Where(t => t.CardNo == cardNo && t.HostID == HostID).FirstOrDefault();
                                if (mb == null)
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，卡号不存在；<br />");
                                    continue;
                                }
                            }
                            //}

                            var s = allPrj.Where(t => t.Contents == prjName).FirstOrDefault();
                            if (s == null)
                            {
                                msg.Append("第" + (rowIndex + 1) + @"行，项目名称不匹配；<br />");
                                continue;
                            }
                            if (CardType != "4" && string.IsNullOrEmpty(prjTime))
                            {
                                msg.Append("第" + (rowIndex + 1) + @"行，项目次数不能空；<br />");
                                continue;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(prjName))
                            {
                                if (!string.IsNullOrEmpty(cardNo))
                                {
                                    // 卡号检测
                                    NewMember = dbcontent.Members.Where(t => t.CardNo == cardNo && t.HostID == HostID).FirstOrDefault();
                                    CashCard = dbcontent.MemberCards.Where(t => t.MemberID == NewMember.MemberID && t.Type == "9").FirstOrDefault();
                                }
                                if (CashCard == null)
                                {
                                    // 添加现金流通卡
                                    MemberCard card = new MemberCard
                                    {
                                        MemberID = NewMember.MemberID,
                                        HostID = NewMember.HostID,
                                        BranchID = NewMember.JoinBranch.Value,
                                        Amount = 0,
                                        ActualPrice = 0,
                                        Amt = 0,
                                        Status = 1,
                                        Title = "现金通道",
                                        Type = "9",
                                        CreatedDate = DateTime.Now,
                                    };
                                    CashCard = dbcontent.MemberCards.Add(card);
                                    dbcontent.SaveChanges();
                                }

                                var prj = allPrj.Where(t => t.Contents == prjName).FirstOrDefault();

                                if (prjGive == "是")
                                {
                                    MemberProject mp = new MemberProject
                                    {
                                        HostID = NewMember.HostID,
                                        BranchId = NewMember.JoinBranch.Value,
                                        GiveId = 0,
                                        IsEntity = 0,
                                        BookTime = Convert.ToInt32(prjTime),
                                        UsedTime = 0,
                                        Amount = 0,
                                        UnitPrice = 0,
                                        CreatedDate = DateTime.Now,
                                        DebtFlag = 0,
                                        LastCount = Convert.ToInt32(prjTime),
                                        status = 0,
                                        MemberID = NewMember.MemberID,
                                        MemberCardId = CashCard.MemberCardId,
                                        Type = "1",
                                        ActualPrice = 0,
                                        ProjectID = prj.KeyValue,
                                        IsVaild = 1
                                    };

                                    dbcontent.MemberProjects.Add(mp);
                                }
                            }
                        }

                        row++;
                    }

                    dbcontent.SaveChanges();

                    ViewBag.Message = "文件总行数:" + rowsCount + "，成功导入的项目行数：" + row + "。<br/>" + msg.ToString();
                }

                return View();
            }
            catch (Exception ex)
            {
                logger.Error("导入失败", ex);
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public ActionResult ExportToPDF(MemberQModel viewModel)
        {
            var printable = GridViewExtension.CreatePrintableObject(GridViewSettings, GetMemberList(viewModel));

            PrintingSystem ps = new PrintingSystem();

            PrintableComponentLink link1 = new PrintableComponentLink(ps);
            link1.Component = printable;

            link1.PrintingSystem.Document.AutoFitToPagesWidth = 1;
            link1.Landscape = true;
            CompositeLink compositeLink = new CompositeLink(ps);
            compositeLink.Links.Add(link1);


            compositeLink.CreateDocument();
            using (MemoryStream stream = new MemoryStream())
            {
                compositeLink.PrintingSystem.ExportToXls(stream);
                WriteToResponse("客户列表", true, "xls", stream);
            }
            ps.Dispose();

            return Index(new MemberQModel(), "");
        }

        void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            string disposition = saveAsFile ? "attachment" : "inline";
            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition",
            string.Format("{0}; filename={1}.{2}", disposition, fileName, fileFormat));
            Response.BinaryWrite(stream.GetBuffer());
            Response.End();
        }

        static GridViewSettings exportGridViewSettings;
        public static GridViewSettings GridViewSettings
        {
            get
            {
                if (exportGridViewSettings == null)
                    exportGridViewSettings = GetGridViewSettings();
                return exportGridViewSettings;
            }
        }
        static GridViewSettings GetGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "GridView";
            settings.CallbackRouteValues = new { Controller = "LogView", Action = "Grid" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.Theme = "BlackGlass";
            settings.KeyFieldName = "Id";
            settings.SettingsPager.Visible = true;
            settings.Settings.ShowGroupPanel = true;
            settings.Settings.ShowFilterRow = true;
            settings.SettingsBehavior.AllowSelectByRowClick = true;
            settings.SettingsPager.PageSize = 25;
            settings.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Control;
            settings.Settings.ShowHeaderFilterButton = true;
            settings.SettingsPopup.HeaderFilter.Height = 200;
            settings.SettingsExport.Landscape = true;
            settings.SettingsExport.TopMargin = 0;
            settings.SettingsExport.LeftMargin = 0;
            settings.SettingsExport.RightMargin = 0;
            settings.SettingsExport.BottomMargin = 0;
            settings.SettingsExport.PaperKind = PaperKind.A4;
            settings.SettingsExport.RenderBrick = (sender, e) =>
            {
                if (e.RowType == GridViewRowType.Data && e.VisibleIndex % 2 == 0)
                    e.BrickStyle.BackColor = System.Drawing.Color.FromArgb(0xEE, 0xEE, 0xEE);
            };

            settings.Columns.Add("Name", "姓名").SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            settings.Columns.Add("CardNo", "卡号");
            settings.Columns.Add("SexValue", "性别");
            settings.Columns.Add("MobileNumber", "手机号");
            settings.Columns.Add("JoinBranchStr", "所属门店");
            settings.Columns.Add("StatusValue", "状态");
            settings.Columns.Add("TypeValue", "类型");
            settings.Columns.Add("LevelValue", "等级");
            settings.Columns.Add(column =>
            {
                column.FieldName = "Birthday";
                column.Caption = "生日";
                column.PropertiesEdit.DisplayFormatString = "yyyy-MM-dd";
            });

            settings.Settings.ShowPreview = true;
            return settings;
        }

        #endregion

        #region 会员卡项设置

        public ActionResult EditCards(long id)
        {
            ViewData["MemberID"] = id;
            var list = dbcontent.MemberCards.Where(t => t.MemberID == id).ToList();
            InitDrop2();
            return View(list);
        }
        public ActionResult CardList(long MemberID)
        {
            ViewData["MemberID"] = MemberID;
            var list = dbcontent.MemberCards.Where(t => t.MemberID == MemberID).ToList();
            InitDrop2();
            return PartialView("CardList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewCardPartial(MemberCard model)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            ViewData["MemberID"] = model.MemberID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreatedBy = userId;
                    model.CreatedDate = DateTime.Now;
                    model.HostID = hostId;
                    model.Status = 1;

                    dbcontent.MemberCards.Add(model);
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop2();
            var list = dbcontent.MemberCards.Where(t => t.MemberID == model.MemberID).ToList();
            return PartialView("CardList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCardPartial(MemberCard model)
        {
            ViewData["MemberID"] = model.MemberID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = dbcontent.MemberCards.Where(t => t.MemberCardId == model.MemberCardId).FirstOrDefault();

                    entity.TmplID = model.TmplID;
                    entity.Type = model.Type;
                    entity.Title = model.Title;
                    entity.Amount = model.Amount;
                    entity.ActualPrice = model.ActualPrice;
                    entity.Status = model.Status;
                    entity.Amt = model.Amt;
                    entity.BookTime = model.BookTime;
                    entity.UsedTime = model.UsedTime;
                    entity.LastCount = model.LastCount;
                    entity.DebtFlag = model.DebtFlag;
                    entity.DebtStatus = model.DebtStatus;
                    entity.ExpiryDate = model.ExpiryDate;

                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop2();
            var list = dbcontent.MemberCards.Where(t => t.MemberID == model.MemberID).ToList();
            return PartialView("CardList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCardPartial(MemberCard model)
        {
            ViewData["MemberID"] = model.MemberID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.BookProjects.Where(t => t.MemberCardId == model.MemberCardId).Count();
                    var n = dbcontent.AccountRecords.Where(t => t.MemberCardId == model.MemberCardId).Count();
                    if (m > 0)
                    {
                        ViewData["EditError"] = "卡项已有操作数据，不能删除.";
                    }
                    else
                    {
                        dbcontent.MemberProjects.Where(t => t.MemberCardId == model.MemberCardId).Update(t => new MemberProject { IsVaild = 0 });
                        dbcontent.MemberCards.Where(t => t.MemberCardId == model.MemberCardId).Update(t => new MemberCard { Status = 0 });
                        dbcontent.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
            }

            InitDrop2();
            var list = dbcontent.MemberCards.Where(t => t.MemberID == model.MemberID).ToList();

            return PartialView("CardList", list);
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

        /// <summary>
        /// 客户购买的疗程
        /// </summary>
        /// <param name="MemberCardId"></param>
        /// <returns></returns>
        public ActionResult ProjectList(long MemberCardId, long MemberID)
        {
            ViewData["MemberCardId"] = MemberCardId;
            ViewData["MemberID"] = MemberID;

            var list = dbcontent.MemberProjects.Where(t => t.MemberCardId == MemberCardId).ToList();
            InitDrop3();
            return PartialView("ProjectList", list);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewProjectPartial(MemberProject model)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            ViewData["MemberID"] = model.MemberID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreatedBy = userId;
                    model.CreatedDate = DateTime.Now;
                    model.HostID = hostId;
                    model.IsVaild = 1;

                    dbcontent.MemberProjects.Add(model);
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop3();
            var list = dbcontent.MemberProjects.Where(t => t.MemberCardId == model.MemberCardId).ToList();
            return PartialView("ProjectList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProjectPartial(MemberProject model)
        {
            ViewData["MemberID"] = model.MemberID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = dbcontent.MemberProjects.Where(t => t.MemberProjectId == model.MemberProjectId).FirstOrDefault();

                    entity.Type = model.Type;
                    entity.Amount = model.Amount;
                    entity.ActualPrice = model.ActualPrice;
                    entity.BookTime = model.BookTime;
                    entity.UsedTime = model.UsedTime;
                    entity.LastCount = model.LastCount;
                    entity.DebtFlag = model.DebtFlag;
                    entity.status = model.status;
                    entity.ProjectID = model.ProjectID;
                    entity.Remark = model.Remark;
                    entity.ExpiryDate = model.ExpiryDate;
                    entity.UnitPrice = model.UnitPrice;

                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop3();
            var list = dbcontent.MemberProjects.Where(t => t.MemberCardId == model.MemberCardId).ToList();
            return PartialView("ProjectList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteProjectPartial(MemberProject model)
        {
            ViewData["MemberID"] = model.MemberID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.BookProjects.Where(t => t.MemberCardId == model.MemberCardId && t.MemberProjectId == model.MemberProjectId).Count();
                    if (m > 0)
                    {
                        ViewData["EditError"] = "卡项已有操作数据，不能删除.";
                    }
                    else
                    {
                        dbcontent.MemberProjects.Where(t => t.MemberProjectId == model.MemberProjectId).Update(t => new MemberProject { IsVaild = 0 });
                        dbcontent.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
            }

            InitDrop3();
            var list = dbcontent.MemberProjects.Where(t => t.MemberCardId == model.MemberCardId).ToList();
            return PartialView("ProjectList", list);
        }

        private void InitDrop3()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            List<SelectListItem> items = new SelectList(dbcontent.Projects.Where(t => t.HostID == hostId).ToList(), "ProjectID", "Name").ToList();
            ViewBag.ProjectDataList = items;
        }

        public ActionResult ProjectList1(long MemberCardId)
        {
            ViewData["MemberCardId"] = MemberCardId;
            var list = dbcontent.MemberCardProjects.Where(t => t.MemberCardId == MemberCardId).ToList();
            InitDrop3();
            return PartialView("ProjectList1", list);
        }
        public ActionResult AddNewProject1Partial(MemberCardProject model)
        {
            ViewData["MemberCardId"] = model.MemberCardId;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    dbcontent.MemberCardProjects.Add(model);
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop3();
            var list = dbcontent.MemberCardProjects.Where(t => t.MemberCardId == model.MemberCardId).ToList();
            return PartialView("ProjectList1", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProject1Partial(MemberCardProject model)
        {
            ViewData["MemberCardId"] = model.MemberCardId;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = dbcontent.MemberCardProjects.Where(t => t.MemberCardProjectId == model.MemberCardProjectId).FirstOrDefault();

                    entity.ProjectID = model.ProjectID;
                    entity.UnitPrice = model.UnitPrice;

                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop3();
            var list = dbcontent.MemberCardProjects.Where(t => t.MemberCardId == model.MemberCardId).ToList();
            return PartialView("ProjectList1", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteProject1Partial(MemberCardProject model)
        {
            ViewData["MemberCardId"] = model.MemberCardId;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    dbcontent.MemberCardProjects.Where(t => t.MemberCardProjectId == model.MemberCardProjectId).Delete();
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
            }

            InitDrop3();
            var list = dbcontent.MemberCardProjects.Where(t => t.MemberCardId == model.MemberCardId).ToList();
            return PartialView("ProjectList1", list);
        }

        #endregion

        #region 手机端调用

        /// <summary>
        /// 终端添加客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppCreate(Member model)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };
            try
            {
                string defaultStatus = "T01";
                string defaultLevel = "A";
                var dd = dbcontent.Dictionaries.Where(t => t.HostId == model.HostID && t.Identifier == "MemberStatus" && t.IsDefault).FirstOrDefault();
                if (dd != null) defaultStatus = dd.KeyValue; // 默认状态
                var ee = dbcontent.Dictionaries.Where(t => t.HostId == model.HostID && t.Identifier == "MemberLevel" && t.IsDefault).FirstOrDefault();
                if (ee != null) defaultLevel = ee.KeyValue;   // 默认等级
                //
                if (model.JoinDate == default(DateTime)) model.JoinDate = DateTime.Today;
                model.CreatedDate = DateTime.Now;
                model.Amt = 0;
                model.Status = defaultStatus;
                model.Level = defaultLevel;
                model.Points = 0;
                model.IsNew = -1;
                model.MaritalStatus = "CM00";
                model.SalesmanId = model.SalesmanId;  // 顾问默认当前建卡人

                dbcontent.Members.Add(model);
                dbcontent.SaveChanges();

                // 日志
                var log = new EventLog
                {
                    HostId = model.HostID,
                    BranchId = model.JoinBranch.Value,
                    MemberId = model.MemberID,
                    TypeId = 1,
                    UserId = model.CreatedBy,
                    ClientId = model.ClientId,
                    CreatedDate = DateTime.Now,
                    Level = 5
                };
                var eventLog = dbcontent.EventLogs.Add(log);

                // 添加现金流通卡
                MemberCard card = new MemberCard
                {
                    MemberID = model.MemberID,
                    HostID = model.HostID,
                    BranchID = model.JoinBranch.Value,
                    Amount = 0,
                    ActualPrice = 0,
                    Amt = 0,
                    Status = 1,
                    Title = "现金通道",
                    Type = "9",
                    ClientID = model.ClientId,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.Now,
                    LogId = eventLog.LogId
                };
                dbcontent.MemberCards.Add(card);
                dbcontent.SaveChanges();

                return Json(new Member { MemberID = model.MemberID });
            }
            catch (Exception e)
            {
                return Json(new Member { MemberID = 0 });
            }
        }

        /// <summary>
        /// 终端更新客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppUpdate(Member model)
        {
            try
            {
                Member entity = dbcontent.Members.Where(a => a.MemberID == model.MemberID).FirstOrDefault();
                entity.Name = model.Name;
                entity.MobileNumber = model.MobileNumber;
                if (!string.IsNullOrEmpty(model.Passwd))
                    entity.Passwd = model.Passwd;
                entity.Sex = model.Sex;
                entity.Source = model.Source;
                entity.Vocation = model.Vocation;
                entity.Address = model.Address;
                entity.Birthday = model.Birthday;
                entity.JoinDate = model.JoinDate;
                entity.Type = model.Type;
                dbcontent.SaveChanges();

                return Json("true");
            }
            catch
            {
                return Json("false");
            }

        }

        /// <summary>
        /// 终端验证客户密码
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppLogin(long memberId, string passwd)
        {
            try
            {
                Member mb = dbcontent.Members.Where(a => a.MemberID == memberId && a.Passwd == passwd).FirstOrDefault();
                if (mb != null)
                    return Json("true");
                else
                    return Json("false");
            }
            catch
            {
                return Json("false");
            }
        }

        /// <summary>
        /// 终端显示客户列表
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppList(int hostId, int branchId, int pageNum, string q, string userId)
        {
            var entity = dbcontent.Users.Where(t => t.Id == userId).FirstOrDefault();
            var query = dbcontent.Members.Where(a => a.HostID == hostId);
            if (entity != null && (entity.Type == "3" || entity.Type == "1"))    //美容师 或 顾问 
            {
                query = query.Where(t => t.SalesmanId == userId);
            }
            else
            {
                query = query.Where(t => t.JoinBranch == branchId);
            }
            if (!String.IsNullOrEmpty(q))
                query = query.Where(a => a.Name.Contains(q) || a.MobileNumber.Contains(q) || a.CardNo.Contains(q));
            var list = (from _ in query
                        join __ in dbcontent.Dictionaries.Where(t => t.HostId == hostId) on _.Status equals __.KeyValue
                        where __.Identifier == "MemberStatus"
                        select new MemberViewModel
                        {
                            MemberID = _.MemberID,
                            Name = _.Name,
                            CardNo = _.CardNo,
                            MobileNumber = (_.MobileNumber == null) ? "" : _.MobileNumber,
                            Address = (_.Address == null) ? "" : _.Address,
                            StatusValue = __.Contents,
                            Vocation = _.Vocation,
                            JoinDate = _.JoinDate
                        }).Take(pageNum).ToList();

            return Json(list);
        }

        /// <summary>
        /// 终端显示客户信息
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppMemberInfo(int hostId, int branchId, string mid, string cardNo)
        {
            MemberViewModel member = null;
            if (!string.IsNullOrEmpty(cardNo))
            {
                member = (from _ in dbcontent.Members
                          join b in dbcontent.Organs on _.JoinBranch equals b.OrganID
                          where _.CardNo == cardNo && _.HostID == hostId
                          select new MemberViewModel
                          {
                              MemberID = _.MemberID,
                              Name = _.Name,
                              CardNo = _.CardNo,
                              MobileNumber = (_.MobileNumber == null) ? "" : _.MobileNumber,
                              JoinDate = _.JoinDate,
                              JoinBranch = _.JoinBranch,
                              JoinBranchStr = b.Name,
                              Sex = _.Sex,
                              SexValue = (_.Sex == null) ? "" : dbcontent.Dictionaries.Where(t => t.Identifier == "MemberGender" && t.KeyValue == _.Sex).FirstOrDefault().Contents,
                              Birthday = _.Birthday,
                              Address = (_.Address == null) ? "" : _.Address,
                              Vocation = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberVocation" && t.KeyValue == _.Vocation).FirstOrDefault().Contents,
                              Source = (_.Source == null) ? "" : _.Source,
                              SourceValue = (_.Source == null) ? "" : dbcontent.Dictionaries.Where(t => t.Identifier == "MemberSource" && t.KeyValue == _.Source).FirstOrDefault().Contents,
                              Level = (_.Level == null) ? "" : _.Level,
                              LevelValue = (_.Level == null) ? "" : dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberLevel" && t.KeyValue == _.Level).FirstOrDefault().Contents,
                              Amt = _.Amt,
                              Points = _.Points,
                              Remark = (_.Remark == null) ? "" : _.Remark,
                              StatusValue = (_.Status == null) ? "" : dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberStatus" && t.KeyValue == _.Status).FirstOrDefault().Contents,
                              SalesmanId = _.SalesmanId
                          }).FirstOrDefault();

            }
            else if (!string.IsNullOrEmpty(mid))
            {
                var mmid = Convert.ToInt16(mid);
                member = (from _ in dbcontent.Members
                          join b in dbcontent.Organs on _.JoinBranch equals b.OrganID
                          where _.MemberID == mmid && _.HostID == hostId
                          select new MemberViewModel
                          {
                              MemberID = _.MemberID,
                              Name = _.Name,
                              CardNo = _.CardNo,
                              MobileNumber = (_.MobileNumber == null) ? "" : _.MobileNumber,
                              JoinDate = _.JoinDate,
                              JoinBranch = _.JoinBranch,
                              JoinBranchStr = b.Name,
                              Sex = _.Sex,
                              SexValue = (_.Sex == null) ? "" : dbcontent.Dictionaries.Where(t => t.Identifier == "MemberGender" && t.KeyValue == _.Sex).FirstOrDefault().Contents,
                              Birthday = _.Birthday,
                              Address = (_.Address == null) ? "" : _.Address,
                              Vocation = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberVocation" && t.KeyValue == _.Vocation).FirstOrDefault().Contents,
                              Source = (_.Source == null) ? "" : _.Source,
                              SourceValue = (_.Source == null) ? "" : dbcontent.Dictionaries.Where(t => t.Identifier == "MemberSource" && t.KeyValue == _.Source).FirstOrDefault().Contents,
                              Level = (_.Level == null) ? "" : _.Level,
                              LevelValue = (_.Level == null) ? "" : dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberLevel" && t.KeyValue == _.Level).FirstOrDefault().Contents,
                              Amt = _.Amt,
                              Points = _.Points,
                              Remark = (_.Remark == null) ? "" : _.Remark,
                              StatusValue = (_.Status == null) ? "" : dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberStatus" && t.KeyValue == _.Status).FirstOrDefault().Contents,
                              SalesmanId = _.SalesmanId
                          }).FirstOrDefault();
            }


            if (member != null)
            {
                member.Projects = (from _ in dbcontent.MemberProjects.Where(t => t.MemberID == member.MemberID && t.IsVaild == 1 && t.LastCount > 0)
                                   join mc in dbcontent.MemberCards.Where(t => t.Status == 1) on _.MemberCardId equals mc.MemberCardId into mpc
                                   from ur in mpc.DefaultIfEmpty()
                                   select new MemberProjectViewModel
                                   {
                                       MemberProjectId = _.MemberProjectId,
                                       MemberID = _.MemberID,
                                       MemberCardId = _.MemberCardId,
                                       ProjectName = _.Project.Name,
                                       Type = _.Type,
                                       IsEntity = _.IsEntity,
                                       UnitPrice = _.UnitPrice,
                                       UsedTime = _.UsedTime,
                                       BookTime = _.BookTime,
                                       LastCount = _.LastCount,
                                       Amount = _.Amount,
                                       ProjectID = _.ProjectID,
                                       ProjectCode = _.Project.Code,
                                       CardType = (ur.Type == null) ? "N/A" : ur.Type,
                                   }).ToList();

                member.Cards = (from _ in dbcontent.MemberCards.Where(t => t.Status == 1 && t.MemberID == member.MemberID)
                                select new MemberCardModel
                                {
                                    MemberCardId = _.MemberCardId,
                                    MemberID = _.MemberID,
                                    Type = _.Type,
                                    TypeValue = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberCardType" && t.KeyValue == _.Type).FirstOrDefault().Contents,
                                    Title = _.Title,
                                    Amt = _.Amt,
                                    UsedTime = _.UsedTime,
                                    BookTime = _.BookTime,
                                    LastCount = _.LastCount,
                                    ExpiryDate = _.ExpiryDate,
                                    Amount = _.Amount,
                                    ActualPrice = _.ActualPrice,
                                    Status = _.Status
                                }).ToList();

                var v = (from vm in dbcontent.MemberProjects.Where(t => t.IsVaild == 1 && t.DebtFlag == 1 && t.MemberID == member.MemberID)
                         select new DebtViewModel
                         {
                             MemberProjectId = vm.MemberProjectId,
                             Amount = vm.Amount,
                             Debt = vm.Amount - vm.ActualPrice,
                             ProjectName = vm.Project.Name,
                             Quantity = vm.BookTime,
                             CreatedDate = vm.CreatedDate,
                             MemberName = vm.Member.Name,
                             CardNo = vm.Member.CardNo,
                             Status = vm.status,
                             _Salesman = dbcontent.AccountRecords.Where(t => t.RecordID == vm.AccountRecordID && t.Type == "3").FirstOrDefault().Splits,
                             BranchName = dbcontent.Organs.Where(t => t.OrganID == vm.BranchId).FirstOrDefault().Name
                         }).ToList();

                foreach (var bp in v)
                {
                    bp.Salesman = String.Join(",", bp._Salesman.Select(t => t.User.UserCnName).ToArray());
                    if (bp._Salesman.Where(t => t.Position == "1").Count() > 0)
                    {
                        bp.SalesId = bp._Salesman.Where(t => t.Position == "1").FirstOrDefault().UserID;
                        bp.SalesRadix = bp._Salesman.Where(t => t.Position == "1").FirstOrDefault().Percentage.ToString();
                    }
                    if (bp._Salesman.Where(t => t.Position == "2").Count() > 0)
                    {
                        bp.BeauticianId = String.Join(",", bp._Salesman.Where(t => t.Position == "2").Select(t => t.User.Id).ToArray());
                        bp.BeauticianRadix = bp._Salesman.Where(t => t.Position == "2").FirstOrDefault().Percentage.ToString();
                    }

                    bp._Salesman = null;
                }

                var v2 = (from vm in dbcontent.MemberCards.Where(t => t.Status == 1 && t.DebtFlag == 1 && t.DebtStatus == 1 && t.MemberID == member.MemberID)
                          select new DebtViewModel
                          {
                              MemberCardId = vm.MemberCardId,
                              Amount = vm.Amount,
                              Debt = vm.Amount - vm.ActualPrice,
                              CardTitle = vm.Title,
                              Quantity = vm.BookTime,
                              CreatedDate = vm.CreatedDate,
                              MemberName = vm.Member.Name,
                              CardNo = vm.Member.CardNo,
                              Status = vm.DebtStatus,
                              _Salesman = dbcontent.AccountRecords.Where(t => t.MemberCardId == vm.MemberCardId).Where(t => t.Type == "2" || t.Type == "3").FirstOrDefault().Splits,
                              BranchName = dbcontent.Organs.Where(t => t.OrganID == vm.BranchID).FirstOrDefault().Name
                          }).ToList();

                foreach (var bp in v2)
                {
                    bp.Salesman = String.Join(",", bp._Salesman.Select(t => t.User.UserCnName).ToArray());
                    if (bp._Salesman.Where(t => t.Position == "1").Count() > 0)
                    {
                        bp.SalesId = bp._Salesman.Where(t => t.Position == "1").FirstOrDefault().UserID;
                        bp.SalesRadix = bp._Salesman.Where(t => t.Position == "1").FirstOrDefault().Percentage.ToString();
                    }
                    if (bp._Salesman.Where(t => t.Position == "2").Count() > 0)
                    {
                        bp.BeauticianId = String.Join(",", bp._Salesman.Where(t => t.Position == "2").Select(t => t.User.Id).ToArray());
                        bp.BeauticianRadix = bp._Salesman.Where(t => t.Position == "2").FirstOrDefault().Percentage.ToString();
                    }

                    bp._Salesman = null;
                }

                member.DebtRecord = v.Union(v2).ToList();
            }

            return Json(member);
        }

        /// <summary>
        /// 获得客户卡信息
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppMemberCard(int hostId, int branchId, string cardNo)
        {
            var list = (from _ in dbcontent.MemberCards.Where(t => t.Status == 1 && t.Type != "9")
                        join b in dbcontent.Members on _.MemberID equals b.MemberID
                        where b.CardNo == cardNo && _.HostID == hostId
                        select new MemberCardModel
                        {
                            MemberCardId = _.MemberCardId,
                            MemberID = _.MemberID,
                            Type = _.Type,
                            Amount = _.Amount,
                            BookTime = _.BookTime,
                            LastCount = _.LastCount,
                            UsedTime = _.UsedTime,
                            ExpiryDate = _.ExpiryDate,
                            ClientID = _.ClientID,
                            HostID = _.HostID,
                            BranchID = _.BranchID,
                            CreatedBy = _.CreatedBy,
                            CreatedDate = _.CreatedDate,
                            DebtFlag = _.DebtFlag,
                            DebtStatus = _.DebtStatus,
                            Status = _.Status,
                            ActualPrice = _.ActualPrice,
                            Amt = _.Amt
                        }).FirstOrDefault();

            return Json(list);
        }

        /// <summary>
        /// 获得客户卡信息
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppMemberCardProject(int hostId, long cardId)
        {
            var list = (from _ in dbcontent.MemberCardProjects
                        join b in dbcontent.Projects on _.ProjectID equals b.ProjectID
                        where _.MemberCardId == cardId
                        select new
                        {
                            id = _.ProjectID,
                            code = b.Code,
                            name = b.Name,
                            unit = _.UnitPrice
                        }).ToList();

            return Json(list);
        }

        /// <summary>
        ///  终端充值
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="Money"></param>
        /// <param name="IncreasePrice">增值</param>
        /// <param name="Type"></param>
        /// <param name="SalesId">顾问</param>
        /// <param name="SalesPercentage">顾问占比</param>
        /// <param name="BeauticianId">美容师</param>
        /// <param name="BeauticianPercentage">美容师占比</param>
        /// <param name="Sales">助理美容师</param>
        /// <param name="AssistBeauticianPercentage">助理占比</param>
        /// <param name="Password"></param>
        /// <param name="Remark"></param>
        /// <param name="BranchId"></param>
        /// <param name="ClientId"></param>
        /// <param name="UserId">操作用户</param>
        /// <param name="Points">会员获得积分</param>
        /// <param name="CardId">储值卡ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppRechargeAmount(long MemberId, int Money, int IncreasePrice, string Type, string SalesId, decimal SalesPercentage,
             string BeauticianId, decimal BeauticianPercentage, string Sales, decimal AssistBeauticianPercentage, string Password, string Remark, int BranchId,
             string ClientId, string UserId, string Points, long CardId)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };
            try
            {
                var member = dbcontent.Members.Where(_ => _.MemberID == MemberId).FirstOrDefault();
                var mCard = dbcontent.MemberCards.Where(_ => _.MemberCardId == CardId).FirstOrDefault();
                if (member == null)
                {
                    result = new
                    {
                        code = 2,
                        message = "无效的会员卡号"
                    };
                }
                else if (mCard == null)
                {
                    result = new
                    {
                        code = 2,
                        message = "无效的储值卡"
                    };
                }
                else
                {
                    if (!member.Passwd.Equals(Password))
                    {
                        result = new
                        {
                            code = 3,
                            message = "密码错误"
                        };
                    }
                    else
                    {
                        var bag = (HostContainerBag)this.RouteData.Values["tenant"];

                        var log = new EventLog
                        {
                            HostId = member.HostID,
                            BranchId = BranchId,
                            MemberId = MemberId,
                            TypeId = 7,
                            UserId = UserId,
                            ClientId = ClientId,
                            CreatedDate = DateTime.Now,
                            Level = 5
                        };
                        var eventLog = dbcontent.EventLogs.Add(log);
                        dbcontent.SaveChanges();

                        //储值卡更新数据
                        member.Amt += Money + IncreasePrice;
                        mCard.Amt += Money + IncreasePrice;

                        // 新增充值记录表
                        var record = new AccountRecord
                        {
                            EventLogId = eventLog.LogId,
                            MemberID = member.MemberID,
                            MemberCardId = mCard.MemberCardId,
                            InAmount = Convert.ToDecimal(Money),
                            Balance = mCard.Amt,
                            PaymentType = Type,
                            Type = "1",
                            HostID = member.HostID,
                            BranchId = BranchId,
                            ClientID = ClientId,
                            SaleID = SalesId,
                            BeauticianID = BeauticianId,
                            Remark = Remark,
                            CreatedDate = DateTime.Now,
                            IsVaild = 1
                        };

                        List<AccountRecordSplit> us = new List<AccountRecordSplit>();
                        // 顾问提成
                        us.Add(new AccountRecordSplit
                        {
                            UserID = SalesId,
                            Position = "1",
                            Percentage = SalesPercentage,
                            Amount = Convert.ToDecimal(Money * SalesPercentage),
                            ModifiedBy = SalesId,
                            ModifiedTime = DateTime.Now,
                        });
                        // 美容师提成
                        us.Add(new AccountRecordSplit
                        {
                            UserID = BeauticianId,
                            Position = "2",
                            Percentage = BeauticianPercentage,
                            Amount = Convert.ToDecimal(Money * BeauticianPercentage),
                            ModifiedBy = UserId,
                            ModifiedTime = DateTime.Now,
                        });

                        // 助理美容师列表
                        string[] wo = Sales.Split(',');
                        int num = 0;
                        foreach (string works in wo)
                        {
                            if (!string.IsNullOrEmpty(works))
                            {
                                num++;
                            }
                        }
                        foreach (string works in wo)
                        {
                            if (!string.IsNullOrEmpty(works))
                            {
                                AccountRecordSplit u = new AccountRecordSplit();
                                u.UserID = works;
                                u.Position = "3";
                                u.Percentage = (AssistBeauticianPercentage != 0 ? AssistBeauticianPercentage / num : 0);
                                u.Amount = (AssistBeauticianPercentage != 0 ? Money * (AssistBeauticianPercentage / num) : 0);
                                u.ModifiedBy = UserId;
                                u.ModifiedTime = DateTime.Now;

                                us.Add(u);
                            }
                        }

                        record.Splits = us;
                        dbcontent.AccountRecords.Add(record);

                        dbcontent.SaveChanges();

                        // 微信客户提醒
                        if (!string.IsNullOrEmpty(member.OpenID))
                        {
                            var accessToken = AccessTokenContainer.TryGetAccessToken(bag.AppId, bag.Secret);
                            var testData = new
                            {
                                first = new TemplateDataItem("客户充值提醒"),
                                keyword1 = new TemplateDataItem(record.InAmount.ToString()),
                                keyword2 = new TemplateDataItem(record.CreatedDate.ToString("yy-MM-dd HH:mm:ss")),
                                keyword3 = new TemplateDataItem(mCard.Amt.ToString()),
                                remark = new TemplateDataItem("点击可查看充值详情。")
                            };
                            string url = "http://cn.mdss.hk/wap/account/" + record.RecordID;
                            var result1 = TemplateApi.SendTemplateMessage(accessToken, member.OpenID, bag.TmplMsg_Recharge, "#FF0000", url, testData);
                        }
                    }
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Error("APP recharge failure.", ex);
                result = new
                {
                    code = 0,
                    message = ex.Message
                };
                return Json(result);
            }
        }

        /// <summary>
        /// 积分赠送
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="Money"></param>
        /// <param name="Type"></param>
        /// <param name="ExpiryDate"></param>
        /// <param name="Salesman"></param>
        /// <param name="Password"></param>
        /// <param name="Remark"></param>
        /// <param name="BranchId"></param>
        /// <param name="ClientId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppGiveAmount(long MemberId, int Money, string Type, string ExpiryDate,
            string Salesman, string Password, string Remark, int BranchId, string ClientId, string UserId)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };
            try
            {
                var member = dbcontent.Members.Where(_ => _.MemberID == MemberId).FirstOrDefault();
                if (!member.Passwd.Equals(Password))
                {
                    result = new
                    {
                        code = 3,
                        message = "密码错误"
                    };
                }
                else
                {
                    var log = new EventLog
                    {
                        HostId = member.HostID,
                        BranchId = BranchId,
                        MemberId = MemberId,
                        TypeId = 2,
                        UserId = UserId,
                        ClientId = ClientId,
                        CreatedDate = DateTime.Now,
                        Level = 5
                    };
                    var eventLog = dbcontent.EventLogs.Add(log);
                    dbcontent.SaveChanges();


                    //会员积分累加 <过期扣除>
                    member.Points = member.Points + Money;

                    // 赠送保存
                    var give = new MemberGive
                    {
                        InPoints = Money,
                        RemainPoints = Money,
                        MemberID = member.MemberID,
                        LogId = eventLog.LogId,
                        HostID = member.HostID,
                        BranchId = BranchId,
                        ClientId = ClientId,
                        Salesman = Salesman,
                        Type = "0",
                        Remark = Remark,
                        IsVaild = 1,
                        CreatedDate = DateTime.Now,
                        CreatedBy = UserId
                    };

                    if (!String.IsNullOrEmpty(ExpiryDate))
                        give.ExpiryDate = Convert.ToDateTime(ExpiryDate);
                    dbcontent.MemberGives.Add(give);
                    dbcontent.SaveChanges();


                    //新增充值记录表
                    var book = new PointBook
                    {
                        MemberId = member.MemberID,
                        HostId = member.HostID,
                        LogId = eventLog.LogId,
                        InPoints = Money,
                        RemainPoints = Money, // 剩余
                        InOut = 1,
                        BranchId = BranchId,
                        Remark = Remark,
                        CreatedDate = DateTime.Now,
                        Salesman = Salesman,
                        ClientId = ClientId,
                        GiveId = give.GiveId
                    };

                    if (!String.IsNullOrEmpty(ExpiryDate))
                        book.ExpiryDate = Convert.ToDateTime(ExpiryDate);

                    dbcontent.PointBooks.Add(book);
                    dbcontent.SaveChanges();
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Error("APP buy project failure.", ex);
                return Json(result);
            }
        }

        /// <summary>
        /// 赠送项目
        /// 项目必须选定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppGiveProject(MemberByProjectModel model)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };

            try
            {
                using (var dbtran = new TransactionScope(TransactionScopeOption.Required))
                {
                    var member = dbcontent.Members.Where(_ => _.MemberID == model.MemberId).FirstOrDefault();

                    if (member == null)
                    {
                        result = new
                        {
                            code = 2,
                            message = "无效的会员卡号"
                        };
                        return Json(result);
                    }
                    if (member.Passwd != model.Password)
                    {
                        result = new
                        {
                            code = 3,
                            message = "密码错误"
                        };
                        return Json(result);
                    }
                    var fromCard = dbcontent.MemberCards.Where(t => t.MemberID == model.MemberId && t.Type == "9").FirstOrDefault();
                    if (fromCard == null)
                    {
                        // 添加现金流通卡
                        MemberCard card = new MemberCard
                        {
                            MemberID = model.MemberId,
                            HostID = member.HostID,
                            BranchID = model.BranchId,
                            Amount = 0,
                            ActualPrice = 0,
                            Amt = 0,
                            Status = 1,
                            Title = "现金通道",
                            Type = "9",
                            CreatedDate = DateTime.Now,
                        };
                        fromCard = dbcontent.MemberCards.Add(card);
                        dbcontent.SaveChanges();
                    }

                    // 赠送保存
                    var give = new MemberGive
                    {
                        MemberID = member.MemberID,
                        CreatedDate = DateTime.Now,
                        HostID = member.HostID,
                        BranchId = model.BranchId,
                        ClientId = model.ClientId,
                        Salesman = model.Salesman,
                        BookTime = model.Count,
                        LastCount = model.Count,
                        Type = "1",
                        Remark = model.remark,
                        ProjectID = model.ProjectID,
                        IsVaild = 1
                    };

                    if (!String.IsNullOrEmpty(model.ExpiryDate))
                        give.ExpiryDate = Convert.ToDateTime(model.ExpiryDate);

                    dbcontent.MemberGives.Add(give);
                    dbcontent.SaveChanges();


                    // 添加项目
                    var entity = new MemberProject
                    {
                        MemberID = member.MemberID,
                        ProjectID = model.ProjectID,
                        MemberCardId = fromCard.MemberCardId,
                        ClientId = model.ClientId,
                        UnitPrice = 0,
                        Amount = 0,  // 应付
                        ActualPrice = 0,  //实付
                        BookTime = model.Count,
                        LastCount = model.Count,
                        UsedTime = 0,
                        Type = model.Type,
                        DebtFlag = 0,
                        status = 0,
                        CreatedDate = DateTime.Now,
                        CreatedBy = model.user,
                        Remark = model.remark,
                        HostID = member.HostID,
                        BranchId = model.BranchId,
                        GiveId = give.GiveId,
                        IsVaild = 1
                    };

                    if (!String.IsNullOrEmpty(model.ExpiryDate))
                        entity.ExpiryDate = Convert.ToDateTime(model.ExpiryDate);

                    dbcontent.MemberProjects.Add(entity);
                    dbcontent.SaveChanges();

                    dbtran.Complete();
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Error("APP buy project failure.", ex);
                return Json(result);
            }
        }


        /// <summary>
        /// 终端购买项目 - 需要修改储值卡购买和欠款购买
        /// </summary>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppBuyProject(MemberByProjectModel model)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };

            try
            {
                using (var dbtran = new TransactionScope(TransactionScopeOption.Required))
                {
                    var member = dbcontent.Members.Where(_ => _.MemberID == model.MemberId).FirstOrDefault();

                    #region 数据检查
                    if (member == null)
                    {
                        result = new
                        {
                            code = 2,
                            message = "无效的会员卡号"
                        };
                        return Json(result);
                    }
                    if (member.Passwd != model.Password)
                    {
                        result = new
                        {
                            code = 3,
                            message = "密码错误"
                        };
                        return Json(result);
                    }


                    #endregion

                    #region 主要付款方式

                    decimal ActualPrice = model.Payment;  // 实付金额
                    int DebtFlag = 0;
                    if (model.BookPrice != null)
                    {
                        //余额不足转为定金（欠款）
                        if (model.Payment > model.BookPrice.Value)
                        {
                            ActualPrice = Convert.ToDecimal(model.BookPrice);
                            DebtFlag = 1;
                        }
                    }

                    List<AccountRecordSplit> us = new List<AccountRecordSplit>();
                    List<AccountRecordSplit> bs = new List<AccountRecordSplit>();

                    int salesnum = 0;  // 美容顾问 累计为1
                    if (model.Workers != null)
                    {
                        string[] tt = model.Workers.Split(',');
                        foreach (string works in tt)
                        {
                            if (!string.IsNullOrEmpty(works))
                            {
                                salesnum++;
                            }
                        }
                    }
                    // 顾问提成
                    us.Add(new AccountRecordSplit
                    {
                        UserID = model.Salesman,
                        Position = "1",
                        Percentage = model.SalesRadix,
                        Amount = model.Payment * model.SalesRadix,
                        ModifiedBy = model.user,
                        ModifiedTime = DateTime.Now,
                    });
                    bs.Add(new AccountRecordSplit
                    {
                        UserID = model.Salesman,
                        Position = "1",
                        Percentage = model.SalesRadix,
                        Amount = model.Payment * model.SalesRadix,
                        ModifiedBy = model.user,
                        ModifiedTime = DateTime.Now,
                    });
                    // 美容师提成
                    if (model.Workers != null)
                    {
                        string[] ff = model.Workers.Split(',');
                        foreach (string works in ff)
                        {
                            if (!string.IsNullOrEmpty(works))
                            {
                                ApplicationUser u = dbcontent.Users.Where(a => a.Id == works).FirstOrDefault();
                                if (u != null)
                                {
                                    us.Add(new AccountRecordSplit
                                    {
                                        UserID = works,
                                        Position = "2",
                                        Percentage = model.WorkerRadix / salesnum,
                                        Amount = model.Payment * model.WorkerRadix / salesnum,
                                        ModifiedTime = DateTime.Now,
                                        ModifiedBy = model.user
                                    });
                                    bs.Add(new AccountRecordSplit
                                    {
                                        UserID = works,
                                        Position = "2",
                                        Percentage = model.WorkerRadix / salesnum,
                                        Amount = model.Payment * model.WorkerRadix / salesnum,
                                        ModifiedBy = model.user,
                                        ModifiedTime = DateTime.Now,
                                    });
                                }
                            }
                        }
                    }

                    #region 保存操作日志
                    var log1 = new EventLog
                    {
                        HostId = member.HostID,
                        BranchId = model.BranchId,
                        MemberId = model.MemberId,
                        TypeId = 9,
                        UserId = model.user,
                        Level = 5,
                        ClientId = model.ClientId,
                        CreatedDate = DateTime.Now,
                    };
                    var eventLog = dbcontent.EventLogs.Add(log1);
                    dbcontent.SaveChanges();
                    #endregion

                    MemberCard fromCard = null;  // 付款储值卡
                    if (model.PaymentType != "4")
                    {
                        fromCard = dbcontent.MemberCards.Where(t => t.MemberID == model.MemberId && t.Type == "9").FirstOrDefault();
                        if (fromCard == null)
                        {
                            // 添加现金流通卡
                            MemberCard card = new MemberCard
                            {
                                MemberID = model.MemberId,
                                HostID = member.HostID,
                                BranchID = model.BranchId,
                                Amount = 0,
                                ActualPrice = 0,
                                Amt = 0,
                                Status = 1,
                                Title = "现金通道",
                                Type = "9",
                                CreatedDate = DateTime.Now,
                            };
                            fromCard = dbcontent.MemberCards.Add(card);
                            dbcontent.SaveChanges();
                        }

                        // 充值
                        AccountRecord record = new AccountRecord
                        {
                            EventLogId = eventLog.LogId,
                            MemberID = member.MemberID,
                            HostID = member.HostID,
                            MemberCardId = fromCard.MemberCardId,
                            InAmount = ActualPrice,   // 实收
                            Balance = ActualPrice,
                            PaymentType = model.PaymentType,
                            Type = "2",
                            BranchId = model.BranchId,
                            ClientID = model.ClientId,
                            Remark = "充值",
                            CreatedDate = DateTime.Now,
                            CreatedBy = model.user,
                            SaleID = model.Salesman,
                            Splits = bs,
                            IsVaild = 1
                        };
                        dbcontent.AccountRecords.Add(record);
                    }
                    else if (model.MemberCardId != null)
                    {
                        fromCard = dbcontent.MemberCards.Where(t => t.MemberCardId == model.MemberCardId).FirstOrDefault();
                    }
                    else
                    {
                        result = new
                        {
                            code = 3,
                            message = "会员卡项错误."
                        };
                        return Json(result);
                    }

                    // TODO 允许会员卡欠款购买，其他卡不允许欠款
                    if (fromCard.Type == "6" && fromCard.Amt < model.BookPrice)
                    {
                        result = new
                        {
                            code = 4,
                            message = "可用金额不足"
                        };
                        return Json(result);
                    }

                    if (fromCard.Type != "9")
                    {
                        member.Amt -= ActualPrice;
                        fromCard.Amt -= ActualPrice;
                    }

                    // 账户流水 - 购买项目
                    AccountRecord log = new AccountRecord
                    {
                        EventLogId = eventLog.LogId,
                        MemberID = member.MemberID,
                        ClientID = model.ClientId,
                        MemberCardId = fromCard.MemberCardId,
                        SalesType = (fromCard.Type == "9" ? 2 : 1),
                        Type = "3",
                        OutAmount = model.Payment, // 应付
                        Balance = fromCard.Amt,
                        CreatedDate = DateTime.Now,
                        CreatedBy = model.user,
                        Remark = model.remark,
                        HostID = member.HostID,
                        BranchId = model.BranchId,
                        SaleID = model.Salesman,
                        Splits = us,
                        IsVaild = 1
                    };
                    dbcontent.AccountRecords.Add(log);
                    #endregion

                    // 添加项目
                    var entity = new MemberProject
                    {
                        LogId = eventLog.LogId,
                        MemberID = member.MemberID,
                        AccountRecordID = log.RecordID,
                        ProjectID = model.ProjectID,
                        MemberCardId = fromCard.MemberCardId,
                        ClientId = model.ClientId,
                        UnitPrice = model.AllPay / model.Count,
                        Amount = model.AllPay,     // 应付
                        ActualPrice = model.AllPay - model.Payment + ActualPrice,  //实付   总的-主付+实付
                        BookTime = model.Count,
                        LastCount = model.Count,
                        UsedTime = 0,
                        Type = model.Type,
                        DebtFlag = DebtFlag,
                        status = DebtFlag,     // 欠款初始状态
                        CreatedDate = DateTime.Now,
                        CreatedBy = model.user,
                        Remark = model.remark,
                        HostID = member.HostID,
                        BranchId = model.BranchId,
                        IsEntity = model.IsEntity,
                        IsVaild = 1
                    };
                    if (!String.IsNullOrEmpty(model.ExpiryDate))
                        entity.ExpiryDate = Convert.ToDateTime(model.ExpiryDate);

                    dbcontent.MemberProjects.Add(entity);

                    #region 其他付款方式

                    if (!string.IsNullOrEmpty(model.OtherPay))
                    {
                        int row = 0;
                        string[] di = model.OtherPay.Split(';');
                        foreach (string dd in di)
                        {
                            if (!string.IsNullOrEmpty(dd))
                            {
                                row++;
                                string[] dt = dd.Split(','); // [付款方式，金额，卡号]

                                var type = dt[0];
                                var crr = dt[2].Split(':');

                                var pay = Convert.ToDecimal(dt[1]);

                                #region 员工分成
                                List<AccountRecordSplit> us1 = new List<AccountRecordSplit>();
                                List<AccountRecordSplit> bs1 = new List<AccountRecordSplit>();
                                us1.Add(new AccountRecordSplit
                                {
                                    UserID = model.Salesman,
                                    Position = "1",
                                    Percentage = model.SalesRadix,
                                    Amount = pay * model.SalesRadix,
                                    ModifiedBy = model.user,
                                    ModifiedTime = DateTime.Now,
                                });
                                bs1.Add(new AccountRecordSplit
                                {
                                    UserID = model.Salesman,
                                    Position = "1",
                                    Percentage = model.SalesRadix,
                                    Amount = pay * model.SalesRadix,
                                    ModifiedBy = model.user,
                                    ModifiedTime = DateTime.Now,
                                });
                                if (model.Workers != null)
                                {
                                    string[] ff = model.Workers.Split(',');
                                    foreach (string works in ff)
                                    {
                                        if (!string.IsNullOrEmpty(works))
                                        {
                                            ApplicationUser u = dbcontent.Users.Where(a => a.Id == works).FirstOrDefault();
                                            if (u != null)
                                            {
                                                us1.Add(new AccountRecordSplit
                                                {
                                                    UserID = works,
                                                    Position = "2",
                                                    Percentage = model.WorkerRadix / salesnum,
                                                    Amount = pay * model.WorkerRadix / salesnum,
                                                    ModifiedTime = DateTime.Now,
                                                    ModifiedBy = model.user
                                                });
                                                bs1.Add(new AccountRecordSplit
                                                {
                                                    UserID = works,
                                                    Position = "2",
                                                    Percentage = model.WorkerRadix / salesnum,
                                                    Amount = pay * model.WorkerRadix / salesnum,
                                                    ModifiedBy = model.user,
                                                    ModifiedTime = DateTime.Now,
                                                });
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (type != "4")   // 有业绩
                                {
                                    fromCard = dbcontent.MemberCards.Where(t => t.MemberID == model.MemberId && t.Type == "9").FirstOrDefault();
                                    AccountRecord record = new AccountRecord
                                    {
                                        EventLogId = eventLog.LogId,
                                        MemberID = member.MemberID,
                                        HostID = member.HostID,
                                        MemberCardId = fromCard.MemberCardId,
                                        InAmount = ActualPrice,   // 实收
                                        Balance = ActualPrice,
                                        PaymentType = type,
                                        Type = "2",
                                        BranchId = model.BranchId,
                                        ClientID = model.ClientId,
                                        Remark = "充值",
                                        CreatedDate = DateTime.Now,
                                        CreatedBy = model.user,
                                        SaleID = model.Salesman,
                                        Splits = us1,
                                        IsVaild = 1
                                    };
                                    dbcontent.AccountRecords.Add(record);

                                }
                                else        //储值卡扣
                                {
                                    var cid = Convert.ToInt64(crr[0]);
                                    fromCard = dbcontent.MemberCards.Where(t => t.MemberCardId == cid).FirstOrDefault();
                                }

                                if (fromCard.Type == "6" && fromCard.Amt < pay)
                                {
                                    result = new
                                    {
                                        code = 4,
                                        message = "可用金额不足"
                                    };
                                    return Json(result);
                                }

                                if (fromCard.Type != "9")
                                {
                                    member.Amt -= pay;
                                    fromCard.Amt -= pay;
                                }

                                // 账户流水 - 购买项目
                                AccountRecord log2 = new AccountRecord
                                {
                                    EventLogId = eventLog.LogId,
                                    MemberID = member.MemberID,
                                    ClientID = model.ClientId,
                                    MemberCardId = fromCard.MemberCardId,
                                    SalesType = (fromCard.Type == "9" ? 2 : 1),
                                    Type = "3",
                                    OutAmount = pay, // 应付
                                    Balance = fromCard.Amt,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = model.user,
                                    Remark = model.remark,
                                    HostID = member.HostID,
                                    BranchId = model.BranchId,
                                    SaleID = model.Salesman,
                                    Splits = us1,
                                    IsVaild = 1
                                };
                                dbcontent.AccountRecords.Add(log2);

                            }
                        }

                    }

                    #endregion


                    // 即销即耗 => 货品
                    if (model.IsEntity == 1)
                    {
                        // 货品数据保存
                        var pgs = dbcontent.ProjectGoods.Where(t => t.ProjectID == model.ProjectID).ToList();
                        foreach (var pg in pgs)
                        {
                            var bgs = new MemberProjectGoods
                            {
                                GoodsID = pg.GoodsID,
                                MemberProjectId = entity.MemberProjectId,
                                Quantity = pg.Quantity * model.Count,
                                ProjectID = pg.ProjectID
                            };
                            dbcontent.MemberProjectGoods.Add(bgs);
                        }
                        dbcontent.SaveChanges();
                    }

                    dbcontent.SaveChanges();
                    dbtran.Complete();
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Error("APP buy project failure.", ex);
                return Json(result);
            }
        }

        /// <summary>
        /// 购买卡片
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppBuyCard(MemberByCardModel model)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };

            try
            {
                using (var dbtran = new TransactionScope(TransactionScopeOption.Required))
                {
                    var member = dbcontent.Members.Where(_ => _.MemberID == model.MemberId).FirstOrDefault();
                    if (member == null)
                    {
                        result = new
                        {
                            code = 2,
                            message = "无效的会员卡号"
                        };
                        return Json(result);
                    }
                    else if (member.Passwd != model.Password)
                    {
                        result = new
                        {
                            code = 3,
                            message = "密码错误"
                        };
                        return Json(result);
                    }

                    var log = new EventLog
                    {
                        HostId = member.HostID,
                        BranchId = model.BranchId,
                        MemberId = model.MemberId,
                        TypeId = 8,
                        UserId = model.user,
                        ClientId = model.ClientId,
                        CreatedDate = DateTime.Now,
                        Level = 5,
                        Content = "购买卡项：" + model.Title
                    };
                    var eventLog = dbcontent.EventLogs.Add(log);
                    dbcontent.SaveChanges();

                    #region 美容师列表
                    List<AccountRecordSplit> us = new List<AccountRecordSplit>();
                    List<AccountRecordSplit> bs = new List<AccountRecordSplit>();
                    string[] wo = model.Beautician.Split(',');
                    int salesnum = 1;
                    foreach (string works in wo)
                    {
                        if (!string.IsNullOrEmpty(works))
                        {
                            salesnum++;
                        }
                    }

                    // -----------------------------------------------------------------
                    decimal ActualPrice = model.Payment;  // 实付金额
                    decimal IncreasePrice = 0; // 增值部分
                    MemberCard fromCard = null;  // 付款储值卡
                    if (model.MemberCardId != null)
                    {
                        fromCard = dbcontent.MemberCards.Where(t => t.MemberCardId == model.MemberCardId).FirstOrDefault();
                    }

                    int DebtFlag = 0;
                    // 定金方式  + 定金不能小于50%
                    if (string.IsNullOrEmpty(model.BookPrice) == false && model.Payment > Convert.ToDecimal(model.BookPrice))
                    {
                        // 定金方式
                        ActualPrice = Convert.ToDecimal(model.BookPrice);
                        DebtFlag = 1;
                    }

                    // 添加顾问
                    us.Add(new AccountRecordSplit
                    {
                        UserID = model.Salesman,
                        Position = "1",
                        Percentage = model.SalesRadix,
                        Amount = ActualPrice * model.SalesRadix,
                        ModifiedBy = model.user,
                        ModifiedTime = DateTime.Now,
                    });
                    bs.Add(new AccountRecordSplit
                    {
                        UserID = model.Salesman,
                        Position = "1",
                        Percentage = model.SalesRadix,
                        Amount = model.Payment * model.SalesRadix,
                        ModifiedBy = model.user,
                        ModifiedTime = DateTime.Now,
                    });

                    int workernum = 0;
                    foreach (string works in wo)
                    {
                        if (!string.IsNullOrEmpty(works))
                        {
                            workernum++;
                        }
                    }

                    // 添加美容师
                    foreach (string works in wo)
                    {
                        if (!string.IsNullOrEmpty(works))
                        {
                            ApplicationUser u = dbcontent.Users.Where(a => a.Id == works).FirstOrDefault();
                            if (u != null)
                            {
                                us.Add(new AccountRecordSplit
                                {
                                    UserID = works,
                                    Position = "2",
                                    Percentage = model.WorkerRadix / workernum,
                                    Amount = ActualPrice * model.WorkerRadix / workernum,
                                    ModifiedBy = model.user,
                                    ModifiedTime = DateTime.Now,
                                });
                                bs.Add(new AccountRecordSplit
                                {
                                    UserID = works,
                                    Position = "2",
                                    Percentage = model.WorkerRadix / workernum,
                                    Amount = model.Payment * model.WorkerRadix / workernum,
                                    ModifiedBy = model.user,
                                    ModifiedTime = DateTime.Now,
                                });
                            }
                        }
                    }

                    #endregion

                    #region 保存卡信息

                    // 添加购卡 欠款数据
                    MemberCard card = new MemberCard
                    {
                        LogId = eventLog.LogId,
                        MemberID = member.MemberID,
                        Type = model.route.ToString(),
                        Title = model.Title,
                        Amt = 0,
                        ActualPrice = ActualPrice,
                        Amount = model.Payment,
                        BookTime = 0,
                        LastCount = 0,
                        UsedTime = 0,
                        DebtFlag = DebtFlag,
                        DebtStatus = DebtFlag,
                        Status = 1,
                        CreatedDate = DateTime.Now,
                        HostID = member.HostID,
                        BranchID = model.BranchId,
                        ClientID = model.ClientId,
                        CreatedBy = model.user,
                        TmplID = model.TmplID
                    };

                    if (model.route == 0 || model.route == 6)  // 储值卡|拓客增值卡
                    {
                        card.Amt = ActualPrice;
                        if (!string.IsNullOrEmpty(model.IncreasePrice))
                        {
                            IncreasePrice = Convert.ToDecimal(model.IncreasePrice);
                            card.Amt = ActualPrice + IncreasePrice;
                        }
                    }

                    // 综合 限次
                    if (model.route == 5 || model.route == 8)
                        card.BookTime = model.Count;
                    // 综合 限时
                    if (model.route == 4 || model.route == 8)
                        card.ExpiryDate = Convert.ToDateTime(model.ExpiryDate);

                    dbcontent.MemberCards.Add(card);
                    dbcontent.SaveChanges();

                    #endregion

                    #region 体验客户转换

                    if (new string[] { "0", "1", "4", "5", "6" }.Contains(card.Type) && member.Type == "L01")
                    {
                        member.Type = "L02";
                        member.IsNew = 1;
                        member.JoinDate = DateTime.Today;
                    }

                    #endregion

                    #region 新增充值记录

                    if (model.Type != "4")   // 去除欠款购买
                    {
                        AccountRecord record = new AccountRecord
                        {
                            EventLogId = eventLog.LogId,
                            MemberID = member.MemberID,
                            HostID = member.HostID,
                            MemberCardId = card.MemberCardId,
                            InAmount = ActualPrice,   // 实收
                            Balance = ActualPrice + IncreasePrice,
                            PaymentType = model.Type,
                            Type = "2",
                            BranchId = model.BranchId,
                            ClientID = model.ClientId,
                            Remark = "充值",
                            CreatedDate = DateTime.Now,
                            CreatedBy = model.user,
                            SaleID = model.Salesman,
                            Splits = us,
                            Debt = model.Payment - ActualPrice,
                            IsVaild = 1
                        };
                        dbcontent.AccountRecords.Add(record);

                        // 储值卡余额变更
                        member.Amt = member.Amt + ActualPrice + IncreasePrice;  // 增加增值部分
                    }
                    else
                    {
                        // 转账出
                        var outRecord = new AccountRecord
                        {
                            EventLogId = eventLog.LogId,
                            HostID = member.HostID,
                            BranchId = model.BranchId,
                            MemberID = member.MemberID,
                            MemberCardId = fromCard.MemberCardId,  // 卡扣
                            OutAmount = ActualPrice,   // 实收
                            Balance = fromCard.Amt - ActualPrice,
                            PaymentType = "1",
                            Type = "4",
                            ClientID = model.ClientId,
                            Remark = "转出",
                            CreatedDate = DateTime.Now,
                            CreatedBy = model.user,
                            SaleID = model.Salesman,
                            IsVaild = 1
                        };
                        fromCard.Amt = fromCard.Amt - ActualPrice;

                        // 转账入
                        var InRecord = new AccountRecord
                        {
                            EventLogId = eventLog.LogId,
                            HostID = member.HostID,
                            BranchId = model.BranchId,
                            MemberID = member.MemberID,
                            MemberCardId = card.MemberCardId,  // 卡扣
                            InAmount = ActualPrice,   // 实收
                            Balance = ActualPrice,
                            PaymentType = "1",
                            Type = "5",
                            ClientID = model.ClientId,
                            Remark = "购卡",
                            CreatedDate = DateTime.Now,
                            CreatedBy = model.user,
                            SaleID = model.Salesman,
                            IsVaild = 1
                        };

                        dbcontent.AccountRecords.Add(outRecord);
                        dbcontent.AccountRecords.Add(InRecord);
                    }

                    dbcontent.SaveChanges();
                    #endregion


                    #region 其他付款方式
                    int row = 0;
                    if (!string.IsNullOrEmpty(model.OtherPay))
                    {
                        string[] di = model.OtherPay.Split(';');
                        foreach (string dd in di)
                        {
                            if (!string.IsNullOrEmpty(dd))
                            {
                                row++;
                                string[] dt = dd.Split(','); // [付款方式，金额，卡号]

                                var type = dt[0];
                                var crr = dt[2].Split(':');
                                var pay = Convert.ToDecimal(dt[1]);

                                #region 员工分成
                                List<AccountRecordSplit> us1 = new List<AccountRecordSplit>();
                                List<AccountRecordSplit> bs1 = new List<AccountRecordSplit>();
                                us1.Add(new AccountRecordSplit
                                {
                                    UserID = model.Salesman,
                                    Position = "1",
                                    Percentage = model.SalesRadix,
                                    Amount = pay * model.SalesRadix,
                                    ModifiedBy = model.user,
                                    ModifiedTime = DateTime.Now,
                                });
                                bs1.Add(new AccountRecordSplit
                                {
                                    UserID = model.Salesman,
                                    Position = "1",
                                    Percentage = model.SalesRadix,
                                    Amount = pay * model.SalesRadix,
                                    ModifiedBy = model.user,
                                    ModifiedTime = DateTime.Now,
                                });
                                if (model.Beautician != null)
                                {
                                    string[] ff = model.Beautician.Split(',');
                                    foreach (string works in ff)
                                    {
                                        if (!string.IsNullOrEmpty(works))
                                        {
                                            ApplicationUser u = dbcontent.Users.Where(a => a.Id == works).FirstOrDefault();
                                            if (u != null)
                                            {
                                                us1.Add(new AccountRecordSplit
                                                {
                                                    UserID = works,
                                                    Position = "2",
                                                    Percentage = model.WorkerRadix / salesnum,
                                                    Amount = pay * model.WorkerRadix / salesnum,
                                                    ModifiedTime = DateTime.Now,
                                                    ModifiedBy = model.user
                                                });
                                                bs1.Add(new AccountRecordSplit
                                                {
                                                    UserID = works,
                                                    Position = "2",
                                                    Percentage = model.WorkerRadix / salesnum,
                                                    Amount = pay * model.WorkerRadix / salesnum,
                                                    ModifiedBy = model.user,
                                                    ModifiedTime = DateTime.Now,
                                                });
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (type != "4")   // 有业绩
                                {
                                    fromCard = dbcontent.MemberCards.Where(t => t.MemberID == model.MemberId && t.Type == "9").FirstOrDefault();
                                    if (fromCard == null)
                                    {
                                        // 添加现金流通卡
                                        MemberCard card1 = new MemberCard
                                        {
                                            MemberID = model.MemberId,
                                            HostID = member.HostID,
                                            BranchID = model.BranchId,
                                            Amount = 0,
                                            ActualPrice = 0,
                                            Amt = 0,
                                            Status = 1,
                                            Title = "现金通道",
                                            Type = "9",
                                            CreatedDate = DateTime.Now,
                                        };
                                        fromCard = dbcontent.MemberCards.Add(card1);
                                        dbcontent.SaveChanges();
                                    }

                                    AccountRecord record = new AccountRecord
                                    {
                                        EventLogId = eventLog.LogId,
                                        MemberID = member.MemberID,
                                        HostID = member.HostID,
                                        MemberCardId = fromCard.MemberCardId,
                                        InAmount = ActualPrice,   // 实收
                                        Balance = ActualPrice,
                                        PaymentType = type,
                                        Type = "2",
                                        BranchId = model.BranchId,
                                        ClientID = model.ClientId,
                                        Remark = "充值",
                                        CreatedDate = DateTime.Now,
                                        CreatedBy = model.user,
                                        SaleID = model.Salesman,
                                        Splits = us1,
                                        IsVaild = 1
                                    };
                                    dbcontent.AccountRecords.Add(record);
                                }
                                else        //储值卡扣
                                {
                                    var cid = Convert.ToInt64(crr[0]);
                                    fromCard = dbcontent.MemberCards.Where(t => t.MemberCardId == cid).FirstOrDefault();
                                }

                                if (fromCard.Type == "6" && fromCard.Amt < pay)
                                {
                                    result = new
                                    {
                                        code = 4,
                                        message = "可用金额不足"
                                    };
                                    return Json(result);
                                }

                                if (fromCard.Type != "9")
                                {
                                    member.Amt -= pay;
                                    fromCard.Amt -= pay;
                                }

                                // 账户流水 - 购买项目
                                AccountRecord log2 = new AccountRecord
                                {
                                    EventLogId = eventLog.LogId,
                                    MemberID = member.MemberID,
                                    ClientID = model.ClientId,
                                    MemberCardId = fromCard.MemberCardId,
                                    SalesType = (fromCard.Type == "9" ? 2 : 1),
                                    Type = "3",
                                    OutAmount = pay, // 应付
                                    Balance = fromCard.Amt,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = model.user,
                                    Remark = model.remark,
                                    HostID = member.HostID,
                                    BranchId = model.BranchId,
                                    SaleID = model.Salesman,
                                    Splits = bs1,
                                    IsVaild = 1
                                };
                                dbcontent.AccountRecords.Add(log2);

                            }
                        }
                    }
                    #endregion


                    long MemberProjectId = default(long);
                    int num = 0;  // 项目总次数

                    #region 账户消费记录
                    if (model.route != 0 && model.route != 6)  // 非储值卡 做 项目购买记录
                    {
                        // 实操
                        AccountRecord record = new AccountRecord
                        {
                            EventLogId = eventLog.LogId,
                            HostID = member.HostID,
                            BranchId = model.BranchId,
                            MemberID = member.MemberID,
                            MemberCardId = card.MemberCardId,
                            OutAmount = model.Payment,   // 应付
                            Balance = 0,
                            Debt = ActualPrice - model.Payment,
                            Type = "3",
                            SalesType = (model.Type == "4" ? 1 : 2),
                            ClientID = model.ClientId,
                            Remark = "消费",
                            CreatedDate = DateTime.Now,
                            CreatedBy = model.user,
                            SaleID = model.Salesman,
                            Splits = bs,
                            IsVaild = 1
                        };
                        dbcontent.AccountRecords.Add(record);

                        // 修改客户余额
                        member.Amt = member.Amt - ActualPrice;
                        dbcontent.SaveChanges();

                        // 疗程卡 体验卡 拓客优惠卡 综合限次卡（选择项目 ）
                        if (new int[] { 1, 2, 3, 7 }.Contains(model.route))
                        {
                            int row1 = 0;
                            string[] di3 = model.ProjectList.Split(';');
                            foreach (string dd in di3)
                            {
                                if (!string.IsNullOrEmpty(dd))
                                {
                                    row1++;
                                    string[] dt = dd.Split(','); // [项目ID ，数量，金额，即销即耗，单价]

                                    if (dt[0].Trim() != "0")   // 被除数为零
                                    {
                                        // 添加项目
                                        var entity = new MemberProject
                                        {
                                            LogId = eventLog.LogId,
                                            MemberID = member.MemberID,
                                            AccountRecordID = record.RecordID,
                                            HostID = member.HostID,
                                            BranchId = model.BranchId,
                                            MemberCardId = card.MemberCardId,
                                            ProjectID = Convert.ToInt32(dt[0]),
                                            ClientId = model.ClientId,
                                            UnitPrice = Convert.ToDecimal(dt[4]),
                                            Amount = Convert.ToDecimal(dt[2]),
                                            ActualPrice = Convert.ToDecimal(dt[2]),
                                            // Points = model.Points,
                                            BookTime = Convert.ToInt32(dt[1]),
                                            LastCount = (dt[3].Trim() == "0") ? Convert.ToInt32(dt[1]) : 0,
                                            UsedTime = (dt[3].Trim() == "0") ? 0 : Convert.ToInt32(dt[1]),
                                            Type = "0",
                                            DebtFlag = 0,
                                            status = 0,
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = model.user,
                                            IsEntity = Convert.ToInt32(dt[3]),
                                            Remark = model.remark,
                                            IsVaild = 1
                                        };

                                        num += (dt[3].Trim() == "0") ? Convert.ToInt32(dt[1]) : 0;
                                        dbcontent.MemberProjects.Add(entity);
                                        dbcontent.SaveChanges();
                                        MemberProjectId = entity.MemberProjectId;
                                    }
                                }
                            }
                        }
                        else if ((model.route == 4 || model.route == 5 || model.route == 8) && string.IsNullOrEmpty(model.ProjectList) == false)
                        {
                            // 综合卡 项目限定
                            string[] dii = model.ProjectList.Split(';');
                            foreach (string dd in dii)
                            {
                                if (!string.IsNullOrEmpty(dd))
                                {
                                    string[] dt = dd.Split(','); // [项目ID ，数量，金额]

                                    MemberCardProject mcp = new MemberCardProject();
                                    mcp.MemberCardId = card.MemberCardId;
                                    mcp.ProjectID = Convert.ToInt32(dt[0]);
                                    mcp.UnitPrice = Convert.ToDecimal(dt[4]);

                                    dbcontent.MemberCardProjects.Add(mcp);
                                }
                            }
                            num = model.Count;
                        }
                    }

                    // 购卡总次数
                    if (new int[] { 1, 2, 3, 5, 7, 8 }.Contains(model.route))
                    {
                        card.BookTime = num;
                        card.LastCount = num;
                    }

                    #endregion

                    dbcontent.SaveChanges();
                    dbtran.Complete();

                    var bag = (HostContainerBag)this.RouteData.Values["tenant"];
                    // 微信客户提醒
                    if (!string.IsNullOrEmpty(member.OpenID))
                    {
                        var accessToken = AccessTokenContainer.TryGetAccessToken(bag.AppId, bag.Secret);
                        var testData = new
                        {
                            first = new TemplateDataItem("客户购卡提醒"),
                            keyword1 = new TemplateDataItem(model.Title),
                            keyword2 = new TemplateDataItem(""),
                            remark = new TemplateDataItem("点击可查看卡片详情。")
                        };
                        string url = "http://cn.mdss.hk/wap/card/" + card.MemberCardId;
                        var result1 = TemplateApi.SendTemplateMessage(accessToken, member.OpenID, bag.TmplMsg_GetCard, "#FF0000", url, testData);
                    }
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Error("APP buy project failure.", ex);
                result = new
                {
                    code = 2,
                    message = ex.Message
                };
                return Json(result);
            }
        }

        /// <summary>
        /// 终端取得客户余额
        /// </summary>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult IsValidCard(int hostId, string CardNo)
        {
            var member = dbcontent.Members.Where(_ => _.HostID == hostId && _.CardNo.Equals(CardNo)).FirstOrDefault();
            if (member == null)
            {
                var result = new
                {
                    code = 2,
                    balance = 0
                };
                return Json(result);
            }
            else
            {
                var result = new
                {
                    code = 1,
                    balance = member.Amt,
                    cash = member.Amt
                };
                return Json(result);
            }
        }

        /// <summary>
        /// 终端验证客户密码
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppExchangeCard(long memberId, string cardNo, int client)
        {
            try
            {
                Member mb = dbcontent.Members.Where(a => a.MemberID == memberId).FirstOrDefault();
                string oldCardNo = mb.CardNo;
                if (mb != null)
                {
                    mb.CardNo = cardNo;
                    // 记录日志
                    dbcontent.SaveChanges();

                    var bag = (HostContainerBag)this.RouteData.Values["tenant"];
                    // 微信客户提醒
                    if (!string.IsNullOrEmpty(mb.OpenID))
                    {
                        var accessToken = AccessTokenContainer.TryGetAccessToken(bag.AppId, bag.Secret);
                        var testData = new
                        {
                            first = new TemplateDataItem("客户变更会员卡号"),
                            keyword1 = new TemplateDataItem(oldCardNo),
                            keyword2 = new TemplateDataItem("更换卡"),
                            keyword3 = new TemplateDataItem(cardNo),
                            keyword4 = new TemplateDataItem("终身有效"),
                            keyword5 = new TemplateDataItem("0"),
                            remark = new TemplateDataItem("点击可查看卡片详情。")
                        };
                        string url = ""; // "http://cn.mdss.hk/wap/card/" + card.MemberCardId;
                        var result1 = TemplateApi.SendTemplateMessage(accessToken, mb.OpenID, bag.TmplMsg_ChangeCard, "#FF0000", url, testData);
                    }

                    return Json("true");
                }
                else
                    return Json("false");
            }
            catch
            {
                return Json("false");
            }
        }

        /// <summary>
        /// 转卡
        /// 条件：
        ///    新项目金额要超过原有项目
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="OffProjects">废弃项目ID[MemberProjectId,count;....]</param>
        /// <param name="OffAmount">置换金额</param>
        /// <param name="Project"></param>
        /// <param name="Count"></param>
        /// <param name="UserId"></param>
        /// <param name="BranchId"></param>
        /// <param name="ClientId"></param>
        /// <param name="IsEntity"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppExchangeProject(long MemberId, string OffProjects, decimal OffAmount, int Project, decimal Price, int Count,
            string PayType, string MemberCardId, string Salesman, string Beautician, decimal SalesRadix, decimal WorkerRadix,
            string UserId, int BranchId, string ClientId, int IsEntity)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };

            // 检查
            if (PayType == "4")
            {
                if (string.IsNullOrEmpty(MemberCardId))
                {
                    result = new
                    {
                        code = 2,
                        message = "储值卡不存在"
                    };
                    return Json(result);
                }
            }

            // 弃用项目
            var ops = OffProjects.Split(';').Where(_ => _.Trim().Length > 0);

            using (var dbtran = new TransactionScope(TransactionScopeOption.Required))
            {
                List<long> vs = new List<long>();
                foreach (var opd in ops)
                {
                    string[] dd = opd.Split(',');
                    if (!string.IsNullOrEmpty(dd[0]))
                    {
                        vs.Add(Convert.ToInt64(dd[0]));
                    }
                }
                var member = dbcontent.Members.Where(_ => _.MemberID == MemberId).FirstOrDefault();
                var projects = dbcontent.MemberProjects.Where(_ => vs.Contains(_.MemberProjectId)).ToList();
                var card = dbcontent.MemberCards.Where(t => t.MemberID == MemberId && t.Type == "9").FirstOrDefault();
                if (card == null)
                {
                    // 添加现金流通卡
                    MemberCard card1 = new MemberCard
                    {
                        MemberID = member.MemberID,
                        HostID = member.HostID,
                        BranchID = member.JoinBranch.Value,
                        Amount = 0,
                        ActualPrice = 0,
                        Amt = 0,
                        Status = 1,
                        Title = "现金通道",
                        Type = "9",
                        CreatedDate = DateTime.Now,
                    };
                    card = dbcontent.MemberCards.Add(card1);
                    dbcontent.SaveChanges();
                }


                #region 保存历史
                string offNames = "";
                foreach (var pd in projects)
                {
                    offNames += pd.Project.Name + ",";
                }
                var log = new EventLog
                {
                    HostId = member.HostID,
                    BranchId = BranchId,
                    MemberId = MemberId,
                    TypeId = 5,
                    UserId = UserId,
                    ClientId = ClientId,
                    CreatedDate = DateTime.Now,
                    Level = 5
                };

                var eventLog = dbcontent.EventLogs.Add(log);
                dbcontent.SaveChanges();
                #endregion

                // 日志内容
                EventLogShell shell = new EventLogShell();
                shell.OriginalProjects = OffProjects;
                shell.OriginalProjectNames = offNames;
                shell.NewProjects = Project.ToString();
                shell.NewPrjAmt = Price * Count;

                // 遗弃项目
                foreach (var pd in projects)
                {
                    // 添加赎回记录
                    RedeemProject rp = new RedeemProject
                    {
                        LogId = eventLog.LogId,
                        MemberId = MemberId,
                        MemberProjectId = pd.MemberProjectId,
                        ProjectId = pd.ProjectID,
                        UnitPrice = pd.UnitPrice,
                        HostId = member.HostID,
                        BranchId = BranchId,
                        CardLogId = log.LogId,
                        CreateDate = DateTime.Now,
                        CreatedBy = UserId
                    };

                    // 过滤找到 弃用的次数
                    foreach (var opd in ops)
                    {
                        string[] dd = opd.Split(',');
                        if (pd.MemberProjectId.ToString() == dd[0].Trim())
                        {
                            rp.Count = Convert.ToInt32(dd[1]);
                        }
                    }
                    rp.Amount = rp.Count * pd.UnitPrice; //兑换金额
                    dbcontent.RedeemProjects.Add(rp);

                    // 清位
                    pd.UsedTime = pd.UsedTime + rp.Count; // 购买次数 = 实际使用次数
                    pd.LastCount = pd.LastCount - rp.Count;
                }

                #region 财务流水
                var dbamt = Price * Count - OffAmount;
                if (dbamt > 0)
                {
                    List<AccountRecordSplit> us = new List<AccountRecordSplit>(); // 充值
                    List<AccountRecordSplit> usa = new List<AccountRecordSplit>();  // 消费

                    // 顾问提成
                    us.Add(new AccountRecordSplit
                    {
                        UserID = Salesman,
                        Position = "1",
                        Percentage = SalesRadix,
                        Amount = dbamt * SalesRadix,
                        ModifiedBy = UserId,
                        ModifiedTime = DateTime.Now,
                    });
                    usa.Add(new AccountRecordSplit
                    {
                        UserID = Salesman,
                        Position = "1",
                        Percentage = SalesRadix,
                        Amount = dbamt * SalesRadix,
                        ModifiedBy = UserId,
                        ModifiedTime = DateTime.Now,
                    });

                    if (!string.IsNullOrEmpty(Beautician))
                    {
                        string[] ff = Beautician.Split(',');
                        foreach (string works in ff)
                        {
                            if (!string.IsNullOrEmpty(works))
                            {
                                ApplicationUser u = dbcontent.Users.Where(a => a.Id == works).FirstOrDefault();
                                if (u != null)
                                {
                                    us.Add(new AccountRecordSplit
                                    {
                                        UserID = works,
                                        Position = "2",
                                        Percentage = WorkerRadix,
                                        Amount = dbamt * WorkerRadix,
                                        ModifiedTime = DateTime.Now,
                                        ModifiedBy = UserId
                                    });
                                    usa.Add(new AccountRecordSplit
                                    {
                                        UserID = works,
                                        Position = "2",
                                        Percentage = WorkerRadix,
                                        Amount = dbamt * WorkerRadix,
                                        ModifiedTime = DateTime.Now,
                                        ModifiedBy = UserId
                                    });
                                }
                            }
                        }
                    }

                    if (PayType == "4")
                    {
                        long mc = Convert.ToInt64(MemberCardId);
                        card = dbcontent.MemberCards.Where(t => t.MemberCardId == mc).FirstOrDefault();
                    }
                    else
                    {
                        // 充值
                        AccountRecord record = new AccountRecord
                        {
                            EventLogId = eventLog.LogId,
                            MemberID = member.MemberID,
                            HostID = member.HostID,
                            MemberCardId = card.MemberCardId,
                            InAmount = dbamt,   // 实收
                            Balance = dbamt,
                            PaymentType = PayType,
                            Type = "1",
                            BranchId = BranchId,
                            ClientID = ClientId,
                            Remark = "充值",
                            CreatedDate = DateTime.Now,
                            CreatedBy = UserId,
                            SaleID = Salesman,
                            Splits = us,
                            IsVaild = 1
                        };
                        dbcontent.AccountRecords.Add(record);
                    }

                    // 销售记录
                    AccountRecord record1 = new AccountRecord
                    {
                        EventLogId = eventLog.LogId,
                        MemberID = member.MemberID,
                        HostID = member.HostID,
                        MemberCardId = card.MemberCardId,
                        InAmount = dbamt,   // 实收
                        Balance = dbamt,
                        PaymentType = PayType,
                        Type = "3",
                        BranchId = BranchId,
                        ClientID = ClientId,
                        Remark = "退项目贴钱购买新项目",
                        CreatedDate = DateTime.Now,
                        CreatedBy = UserId,
                        SaleID = Salesman,
                        Splits = usa,
                        IsVaild = 1
                    };
                    dbcontent.AccountRecords.Add(record1);

                }
                else
                {
                    List<AccountRecordSplit> us = new List<AccountRecordSplit>(); // 充值

                    us.Add(new AccountRecordSplit
                    {
                        UserID = Salesman,
                        Position = "1",
                        Percentage = SalesRadix,
                        Amount = dbamt * SalesRadix,
                        ModifiedBy = UserId,
                        ModifiedTime = DateTime.Now,
                    });
                    if (!string.IsNullOrEmpty(Beautician))
                    {
                        string[] ff = Beautician.Split(',');
                        foreach (string works in ff)
                        {
                            if (!string.IsNullOrEmpty(works))
                            {
                                ApplicationUser u = dbcontent.Users.Where(a => a.Id == works).FirstOrDefault();
                                if (u != null)
                                {
                                    us.Add(new AccountRecordSplit
                                    {
                                        UserID = works,
                                        Position = "2",
                                        Percentage = WorkerRadix,
                                        Amount = dbamt * WorkerRadix,
                                        ModifiedTime = DateTime.Now,
                                        ModifiedBy = UserId
                                    });
                                }
                            }
                        }
                    }

                    // 退钱
                    AccountRecord record1 = new AccountRecord
                    {
                        EventLogId = eventLog.LogId,
                        MemberID = member.MemberID,
                        HostID = member.HostID,
                        MemberCardId = card.MemberCardId,
                        InAmount = dbamt,   // 实收
                        Balance = 0,
                        PaymentType = PayType,
                        Type = "6",
                        BranchId = BranchId,
                        ClientID = ClientId,
                        Remark = "退项目，购买金退还",
                        CreatedDate = DateTime.Now,
                        CreatedBy = UserId,
                        SaleID = Salesman,
                        Splits = us,
                        IsVaild = 1
                    };
                    dbcontent.AccountRecords.Add(record1);
                }
                #endregion

                // 新项目
                MemberProject entity = new MemberProject
                {
                    LogId = eventLog.LogId,
                    HostID = member.HostID,
                    BranchId = BranchId,
                    MemberID = member.MemberID,
                    ProjectID = Project,
                    UnitPrice = Price,
                    Amount = Price * Count,
                    MemberCardId = card.MemberCardId,
                    ActualPrice = Price * Count,
                    BookTime = Count,
                    LastCount = Count,
                    Type = "2",
                    UsedTime = 0,
                    CreatedDate = DateTime.Now,
                    CreatedBy = UserId,
                    ClientId = ClientId,
                    DebtFlag = 0,
                    IsEntity = IsEntity,
                    status = 0,
                    IsVaild = 1
                };
                dbcontent.MemberProjects.Add(entity);
                dbcontent.SaveChanges();

                // 日志保存
                shell.NewMemberProjectId = entity.MemberProjectId;
                shell.OriginalPrjAmt = OffAmount;  //弃用项目金额累计
                eventLog.Shell = JsonConvert.SerializeObject(shell);
                eventLog.Content = "客户换卡：MemberId:" + MemberId + " 原始项目:" + shell.OriginalProjects + " 原始项目:"
                    + shell.OriginalProjectNames + " 原始项目累计金额:" + shell.OriginalPrjAmt
                    + " 新项目:" + shell.NewMemberProjectId + " 原始项目:" + shell.NewProjects + " 新项目累计金额:" + shell.NewPrjAmt;

                dbcontent.SaveChanges();
                dbtran.Complete();
            }

            return Json(result);
        }

        /// <summary>
        /// 换卡
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="NewCardNo"></param>
        /// <param name="Passwd"></param>
        /// <param name="UserId"></param>
        /// <param name="BranchId"></param>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppExchangeCardNo(long MemberId, string NewCardNo, string Passwd, string UserId,
            int BranchId, string ClientId)
        {
            var result = new
            {
                code = 1,
                message = string.Empty
            };
            try
            {
                var member = dbcontent.Members.Where(_ => _.MemberID == MemberId).FirstOrDefault();

                // 历史保存
                var log = new EventLog
                {
                    BranchId = BranchId,
                    HostId = member.HostID,
                    TypeId = 4,
                    Level = 5,
                    ClientId = ClientId,
                    UserId = UserId,
                    Content = "客户转卡：MemberId:" + MemberId + " 旧卡号:" + member.CardNo + " 新卡号:" + NewCardNo,
                    Shell = JsonConvert.SerializeObject(new EventLogShell { OriginalCardNo = member.CardNo, NewCardNo = NewCardNo }),
                    MemberId = MemberId,
                    CreatedDate = DateTime.Now
                };
                dbcontent.EventLogs.Add(log);

                //修改客户表
                member.CardNo = NewCardNo;
                dbcontent.SaveChanges();

                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                result = new
                {
                    code = 0,
                    message = "未知错误"
                };
                return Json(result);
            }
        }

        /// <summary>
        /// 新加会员验证手机号
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppRepeatMobile(int hostId, string Mobile)
        {
            bool flag = true;
            try
            {
                int mb = dbcontent.Members.Where(a => a.HostID == hostId && a.MobileNumber == Mobile).Count();
                if (mb > 0)
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return Json(flag);
        }

        /// <summary>
        /// 修改会员验证手机号
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppEditRepeatMobile(int hostId, int memberId, string Mobile)
        {
            bool flag = true;
            try
            {
                int mb = dbcontent.Members.Where(a => a.HostID == hostId
                    && a.MemberID != memberId && a.MobileNumber == Mobile).Count();
                if (mb > 0)
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return Json(flag);
        }

        /// <summary>
        /// 新会员验证卡号
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppRepeatCardNo(int hostId, string CardNo)
        {
            bool flag = true;
            try
            {
                int mb = dbcontent.Members.Where(a => a.HostID == hostId && a.CardNo == CardNo).Count();
                if (mb > 0)
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return Json(flag);
        }

        #endregion
    }
}
