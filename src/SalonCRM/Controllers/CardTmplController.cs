using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Web;
using DevExpress.Web.Mvc;

namespace SalonCRM.Controllers
{
    public class CardTmplController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: CardTmpl
        public ActionResult Index()
        {
            InitDrop();
            return View(GetList());
        }

        public ActionResult TmplList()
        {
            InitDrop();
            return PartialView("TmplList", GetList());
        }


        private IList<CardTmpl> GetList()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var list = dbcontent.CardTmpls.Where(t => t.HostID == hostId).ToList();

            return list;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewTmplPartial(CardTmpl model)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = new CardTmpl
                    {
                        CardType = model.CardType,
                        Title = model.Title,
                        HostID = hostId,
                        Amount = model.Amount,
                        IsVaild = 1,
                        CreatedDate = DateTime.Now,
                        Remark = model.Remark
                    };

                    dbcontent.CardTmpls.Add(m);
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
            return PartialView("TmplList", GetList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateTmplPartial(CardTmpl model)
        {
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.CardTmpls.Where(t => t.TmplID == model.TmplID).FirstOrDefault();
                    m.Title = model.Title;
                    m.CardType = model.CardType;
                    m.Amount = model.Amount;
                    m.IsVaild = model.IsVaild;
                    m.Remark = model.Remark;
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
            return PartialView("TmplList", GetList());
        }


        /// <summary>
        /// 级联下拉框使用
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="textField"></param>
        /// <returns></returns>
        public ActionResult GetProjects(string Category, string textField)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            return GridViewExtension.GetComboBoxCallbackResult(p =>
            {
                p.TextField = textField;
                p.ValueField = "ProjectID";
                p.BindList(dbcontent.Projects.Where(t => t.HostID == hostId && t.Category == Category).ToList());
            });
        }

        private void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            List<SelectListItem> items = new SelectList(dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "MemberCardType").ToList(), "KeyValue", "Contents").ToList();
            ViewBag.CardTypeList = items;
            ViewBag.ProjectList = dbcontent.Projects.Where(t => t.HostID == hostId).ToList();
            ViewBag.CategoryList = dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "ProjectCategory").ToList();
        }


        public ActionResult ProjectList(long TmplID)
        {
            ViewData["TmplID"] = TmplID;
            InitDrop();
            var list = dbcontent.CardTmplProjects.Where(t => t.TmplID == TmplID).ToList();
            return PartialView("ProjectList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewSplitPartial(CardTmplProject model)
        {
            ViewData["TmplID"] = model.TmplID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = new CardTmplProject
                    {
                        TmplID = model.TmplID,
                        ProjectID = Convert.ToInt32(model.Project.Name),
                        Quantity = model.Quantity,
                        Amount = model.Amount,
                        Price = model.Price
                    };

                    dbcontent.CardTmplProjects.Add(m);
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
            var list = dbcontent.CardTmplProjects.Where(t => t.TmplID == model.TmplID).ToList();
            return PartialView("ProjectList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateSplitPartial(CardTmplProject model)
        {
            ViewData["TmplID"] = model.TmplID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.CardTmplProjects.Where(t => t.TmplProjectID == model.TmplProjectID).FirstOrDefault();
                    if (m.Project.Name != model.Project.Name)
                    {
                        m.ProjectID = Convert.ToInt32(model.Project.Name);
                    }
                    m.Quantity = model.Quantity;
                    m.Price = model.Price;
                    m.Amount = model.Amount;
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
            var list = dbcontent.CardTmplProjects.Where(t => t.TmplID == model.TmplID).ToList();
            return PartialView("ProjectList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteSplitPartial(CardTmplProject model)
        {
            ViewData["TmplID"] = model.TmplID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.CardTmplProjects.Where(t => t.TmplProjectID == model.TmplProjectID).FirstOrDefault();
                    if (m != null)
                    {
                        dbcontent.CardTmplProjects.Remove(m);
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
            var list = dbcontent.CardTmplProjects.Where(t => t.TmplID == model.TmplID).ToList();
            return PartialView("ProjectList", list);
        }


        #region 终端使用
        /// <summary>
        /// 终端取得项目类型
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult GetList(int hostId, string type)
        {
            var list = (from _ in dbcontent.CardTmpls.Where(a => a.HostID == hostId && a.CardType == type && a.IsVaild == 1)
                        select new
                        {
                            id = _.TmplID,
                            title = _.Title,
                            amount = _.Amount,
                            remark = _.Remark,
                            projects = (from dd in dbcontent.CardTmplProjects.Where(t => t.TmplID == _.TmplID)
                                        select new
                                        {
                                            projectid = dd.ProjectID,
                                            projectname = dd.Project.Name,
                                            quantity = dd.Quantity,
                                            category = dd.Project.Category,
                                            price = dd.Price,
                                            amount = dd.Amount
                                        }).ToList()
                        }).ToList();
            return Json(list);
        }

        #endregion
    }
}
