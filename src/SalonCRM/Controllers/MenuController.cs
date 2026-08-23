using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SalonCRM.Models;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 功能菜单
    /// </summary>
    [Authorize(Roles = "管理员")]
    public class MenuController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: Objective
        public ActionResult Index(MenuQModel qmodel)
        {
            DropInit();
            qmodel.MenuList = GetList();
            return View(qmodel);
        }

        public ActionResult TreePartial()
        {
            DropInit();
            return PartialView("TreePartial", GetList());
        }

        private List<MenuViewModel> GetList()
        {
            var result = (from a in dbcontent.MenuItems
                          select new MenuViewModel
                          {
                              MenuId = a.Id,
                              ParentId = a.ParentId,
                              Name = a.Name,
                              Level = a.Level,
                              IsActive = a.IsActive,
                              Description = a.Description,
                              MenuPath = a.MenuPath,
                              SortOrder = a.SortOrder,
                              Icon = a.Icon,
                              SiteNav = a.SiteNav,
                              SiteNavNext = a.SiteNavNext
                          }).OrderBy(t => t.SortOrder).ToList();

            return result;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewMenuPartial(MenuViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MenuItem item = new MenuItem
                    {
                        Name = vmodel.Name,
                        ParentId = vmodel.ParentId,
                        IsActive = true,
                        MenuPath = vmodel.MenuPath,
                        Level = vmodel.Level,
                        Description = vmodel.Description,
                        Icon = vmodel.Icon,
                        SortOrder = vmodel.SortOrder,
                        SiteNav = vmodel.SiteNav,
                        SiteNavNext = vmodel.SiteNavNext
                    };
                    dbcontent.MenuItems.Add(item);
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditNodeError"] = e.Message;
                }
            }
            else
                ViewData["EditNodeError"] = "Please, correct all errors.";

            DropInit();
            return PartialView("TreePartial", GetList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateMenuPartial(MenuViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var item = dbcontent.MenuItems.Where(t => t.Id == vmodel.MenuId).FirstOrDefault();
                    item.Name = vmodel.Name;
                    item.MenuPath = vmodel.MenuPath;
                    item.Description = vmodel.Description;
                    item.Level = vmodel.Level;
                    item.IsActive = vmodel.IsActive;
                    item.SortOrder = vmodel.SortOrder;
                    item.Icon = vmodel.Icon;
                    item.SiteNav = vmodel.SiteNav;
                    item.SiteNavNext = vmodel.SiteNavNext;

                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditNodeError"] = e.Message;
                }
            }
            else
                ViewData["EditNodeError"] = "Please, correct all errors.";

            DropInit();
            return PartialView("TreePartial", GetList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteMenuPartial(int MenuId)
        {
            try
            {
                var item = dbcontent.MenuItems.Where(t => t.Id == MenuId).FirstOrDefault();
                dbcontent.MenuItems.Remove(item);
                dbcontent.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["EditNodeError"] = e.Message;
            }

            DropInit();
            return PartialView("TreePartial", GetList());
        }

        private void DropInit()
        {
            List<SelectListItem> items1 = new List<SelectListItem>();
            items1.Add(new SelectListItem { Value = "SN1", Text = "运营" });
            items1.Add(new SelectListItem { Value = "SN2", Text = "经营" });
            items1.Add(new SelectListItem { Value = "SN3", Text = "财务" });
            ViewBag.NavList = items1;

            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem { Value = "", Text = "--请选择--" });
            items2.Add(new SelectListItem { Value = "SNN1", Text = "客户类" });
            items2.Add(new SelectListItem { Value = "SNN2", Text = "员工类" });
            items2.Add(new SelectListItem { Value = "SNN3", Text = "品项类" });
            items2.Add(new SelectListItem { Value = "SNN4", Text = "综合统计" });
            ViewBag.NavNextList = items2;

        }
    }
}
