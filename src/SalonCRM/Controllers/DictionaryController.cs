using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using SalonCRM.Models;
using SalonCRM.Web;

namespace SalonCRM.Controllers
{
    [Authorize(Roles = "管理员")]
    public class DictionaryController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        private static IList<DictionaryExt> _Dict;
        private static Dictionary<string, string> _DictSet;

        public static IList<DictionaryExt> DictionaryType
        {
            get
            {
                if (_Dict == null)
                {
                    IList<DictionaryExt> result = new List<DictionaryExt>();
                    //result.Add(new DictionaryExt("", "全部", 0));
                    result.Add(new DictionaryExt("ClientIsVald", "终端是否有效", 0));
                    result.Add(new DictionaryExt("FeedbackWay", "客户回访方式", 1));
                    result.Add(new DictionaryExt("FeedbackType", "客户回访类型", 1));
                    result.Add(new DictionaryExt("MemberGender", "客户性别", 0));
                    result.Add(new DictionaryExt("MemberLevel", "客户等级", 1));
                    result.Add(new DictionaryExt("MemberSource", "客户来源", 0));
                    result.Add(new DictionaryExt("MemberStatus", "客户状态", 1));
                    result.Add(new DictionaryExt("EventType", "操作类型", 0));

                    result.Add(new DictionaryExt("MemberType", "客户类型", 0));
                    result.Add(new DictionaryExt("MemberVocation", "客户行业", 0));
                    result.Add(new DictionaryExt("MemberCardType", "客户卡类型", 0));
                    //客户信息
                    result.Add(new DictionaryExt("MaritalStatus", "婚姻状况", 0));
                    result.Add(new DictionaryExt("SkinType", "皮肤类型", 0));
                    result.Add(new DictionaryExt("SkinConditions", "肌肤状况", 0));
                    result.Add(new DictionaryExt("FacialDemand", "面部需求", 0));
                    result.Add(new DictionaryExt("BodyDemand", "身体需求", 0));
                    result.Add(new DictionaryExt("CustomerDemand", "客户需求", 0));
                    result.Add(new DictionaryExt("ConsumptionHabit", "消费习惯", 0));
                    result.Add(new DictionaryExt("Personality", "性格", 0));
                    
                    result.Add(new DictionaryExt("ProjectCategory", "项目类型", 0));
                    result.Add(new DictionaryExt("ProjectBrand", "项目品牌", 1));
                    result.Add(new DictionaryExt("ProjectExtCategory", "项目来源类型", 0));
                    result.Add(new DictionaryExt("ProjectStatus", "项目状态", 0));

                    result.Add(new DictionaryExt("UserCategory", "用户类型", 0));
                    result.Add(new DictionaryExt("UserStatus", "用户状态", 0));
                    result.Add(new DictionaryExt("AppointmentStaus", "预约状态", 0));
                    result.Add(new DictionaryExt("HostProfiles", "商户定义", 0));

                    result.Add(new DictionaryExt("BookState", "订单状态", 0));
                    result.Add(new DictionaryExt("AccountRecordType", "交易类型", 0));

                    _Dict = result;

                }
                return _Dict;
            }
        }

        public static Dictionary<string, string> DictionarySet
        {
            get
            {
                if (_DictSet == null)
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    result.Add("S1", "期间连续到店");
                    result.Add("S2", "期间有到过店");
                    result.Add("S3", "期间没有到过店");
                    _DictSet = result;
                }
                return _DictSet;
            }
        }

        // GET: Dictionary
        public ActionResult Index(DictionaryQModel viewModel)
        {
            string category = "";
            if (!String.IsNullOrEmpty(viewModel.Category))
            {
                ViewBag.Title = DictionaryType.Where(t => t.Key == viewModel.Category).FirstOrDefault().Value;
                category = viewModel.Category;
            }
            else
                ViewBag.Title = "词典";

            if (!string.IsNullOrEmpty(viewModel.FCategory))
                category = viewModel.FCategory;

            // 当前类型
            ViewData["Category"] = viewModel.Category;
            ViewData["FCategory"] = viewModel.FCategory;

            List<SelectListItem> l = (from v in DictionaryType
                                      select new SelectListItem
                                      {
                                          Value = v.Value,
                                          Text = v.Key
                                      }).ToList();
            l.Insert(0, new SelectListItem { Text = "", Value = "选择" });
            ViewBag.CategoryType = new SelectList(l, "Text", "Value");
            viewModel.DictionayList = GetList(category);

            return View(viewModel);
        }
        public ActionResult PageList(string Category, string FCategory)
        {
            string category = Category;
            if (!string.IsNullOrEmpty(FCategory))
                category = FCategory;
            ViewData["Category"] = Category; // 来源
            ViewData["FCategory"] = FCategory; // 过滤
            return PartialView("PageList", GetList(category));
        }
        private List<DictionaryViewModel> GetList(string Category)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Dictionaries.Where(t => t.HostId == 0 || t.HostId == hostId);
            if (!String.IsNullOrEmpty(Category))
                query = query.Where(a => a.Identifier == Category);

            var DictionayPageList = (from q in query
                                     orderby q.Identifier, q.KeyValue
                                     select new DictionaryViewModel
                                     {
                                         TypeId = q.TypeId,
                                         Identifier = q.Identifier,
                                         KeyValue = q.KeyValue,
                                         IsVaild = q.IsVaild,
                                         SortOrder = q.SortOrder,
                                         HostId = q.HostId,
                                         Remark = q.Remark,
                                         Contents = q.Contents,
                                         IsDefault = q.IsDefault
                                     }).ToList();
            foreach (DictionaryViewModel vm in DictionayPageList)
            {
                vm.IdentifierName = DictionaryType.Where(t => t.Key == vm.Identifier).FirstOrDefault().Value;
            }
            return DictionayPageList;
        }

        // GET: Dictionary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dictionary/Create
        public ActionResult Create(string Category)
        {
            DictionaryViewModel vm = new DictionaryViewModel();
            if (!String.IsNullOrEmpty(Category))
            {
                ViewBag.Title = DictionaryType.Where(t => t.Key == Category).FirstOrDefault().Value;
                vm.Identifier = Category;
            }
            else
            {
                ViewBag.Title = "词典";
                vm.Identifier = DictionarySet.First().Key;
            }
            vm.MonthSet = DictionarySet.First().Key;

            ViewBag.Category = Category;
            ViewBag.CategorySet = new SelectList(DictionarySet, "Key", "Value");
            ViewBag.DictionaryType = new SelectList(DictionaryType, "Key", "Value");

            return View(vm);
        }

        // POST: Dictionary/Create
        [HttpPost]
        public ActionResult Create(DictionaryViewModel formModel)
        {
            try
            {
                string shell = "";
                if (formModel.Identifier.Equals("MemberStatus"))
                    shell = JsonConvert.SerializeObject(new DictionaryViewModel { FMonth = formModel.FMonth, SMonth = formModel.SMonth, MonthSet = formModel.MonthSet });
                else if (formModel.Identifier.Equals("MemberLevel"))
                    shell = JsonConvert.SerializeObject(new DictionaryViewModel { MaxAmount = formModel.MaxAmount, MinAmount = formModel.MinAmount });

                Dictionary dic = new Dictionary
                {
                    Identifier = formModel.Identifier,
                    Contents = formModel.Contents.Trim(),
                    KeyValue = formModel.KeyValue,
                    Remark = formModel.Remark,
                    HostId = DictionaryType.Where(t => t.Flag == 1 && t.Key == formModel.Identifier).Count() > 0 ? GlobalContext.Current.UserHost.HostID : 0,
                    Shell = shell,
                    SortOrder = formModel.SortOrder,
                    IsVaild = 1,
                    IsDefault = false
                };
                dbcontent.Dictionaries.Add(dic);
                dbcontent.SaveChanges();
                if (string.IsNullOrEmpty(formModel.Category))
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Index", new { Category = formModel.Category });
            }
            catch
            {
                return View();
            }
        }

        // GET: Dictionary/Edit/5
        public ActionResult Edit(int id, string Category)
        {
            Dictionary dc = dbcontent.Dictionaries.FirstOrDefault(t => t.TypeId == id);
            DictionaryViewModel vm = new DictionaryViewModel
            {
                HostId = dc.HostId,
                TypeId = dc.TypeId,
                Identifier = dc.Identifier,
                KeyValue = dc.KeyValue,
                Contents = dc.Contents,
                Remark = dc.Remark,
                IsVaild = dc.IsVaild,
                SortOrder = dc.SortOrder,
                IsDefault = dc.IsDefault
            };

            if (dc.Identifier.Equals("MemberLevel") || dc.Identifier.Equals("MemberStatus"))
            {
                if (!string.IsNullOrEmpty(dc.Shell))
                {
                    DictionaryViewModel vv = JsonConvert.DeserializeObject<DictionaryViewModel>(dc.Shell);
                    vm.MaxAmount = vv.MaxAmount;
                    vm.MinAmount = vv.MinAmount;
                    vm.MonthSet = vv.MonthSet;
                    vm.FMonth = vv.FMonth;
                    vm.SMonth = vv.SMonth;
                }
            }
            ViewBag.CategorySet = new SelectList(DictionarySet, "Key", "Value");

            if (!String.IsNullOrEmpty(Category))
            {
                ViewBag.Title = DictionaryType.Where(t => t.Key == Category).FirstOrDefault().Value;
                vm.Identifier = Category;
            }
            else
                ViewBag.Title = "词典";

            return View(vm);
        }

        // POST: Dictionary/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DictionaryViewModel formModel)
        {
            try
            {
                Dictionary prj = dbcontent.Dictionaries.FirstOrDefault(t => t.TypeId == formModel.TypeId);
                prj.KeyValue = formModel.KeyValue;
                prj.Contents = formModel.Contents.Trim();
                prj.Remark = formModel.Remark;
                prj.SortOrder = formModel.SortOrder;
                prj.IsVaild = formModel.IsVaild;
                prj.IsDefault = formModel.IsDefault;

                string shell = "";
                if (formModel.Identifier.Equals("MemberStatus"))
                    shell = JsonConvert.SerializeObject(new DictionaryViewModel { FMonth = formModel.FMonth, SMonth = formModel.SMonth, MonthSet = formModel.MonthSet });
                else if (formModel.Identifier.Equals("MemberLevel"))
                    shell = JsonConvert.SerializeObject(new DictionaryViewModel { MaxAmount = formModel.MaxAmount, MinAmount = formModel.MinAmount });

                prj.Shell = shell;

                dbcontent.SaveChanges();
                if (string.IsNullOrEmpty(formModel.Category))
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Index", new { Category = formModel.Category });
            }
            catch
            {
                return View();
            }
        }

        //// GET: Dictionary/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Dictionary/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppList(int hostId, string type)
        {
            try
            {
                if (DictionaryType.Where(t => t.Flag == 1 && t.Key == type).Count() > 0)
                {
                    // 商户特定列表
                    var list = (from _ in dbcontent.Dictionaries.Where(a => a.Identifier.Equals(type) && a.HostId == hostId)
                                select new
                                {
                                    code = _.KeyValue,
                                    name = _.Contents
                                }).ToList();
                    return Json(list);
                }
                else
                {
                    var list = (from _ in dbcontent.Dictionaries.Where(a => a.Identifier.Equals(type))
                                select new
                                {
                                    code = _.KeyValue,
                                    name = _.Contents
                                }).ToList();
                    return Json(list);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class DictionaryExt
    {
        public DictionaryExt(string key, string value, int flag)
        {
            this.Key = key;
            this.Value = value;
            this.Flag = flag;
        }
        public string Value { get; set; }
        public string Key { get; set; }
        /// <summary>
        /// 商户特定
        /// </summary>
        public int Flag { get; set; }
    }
}
