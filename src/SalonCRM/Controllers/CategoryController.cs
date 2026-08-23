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
    public class CategoryController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: Objective
        public ActionResult Index(CategoryQModel qmodel)
        {
            qmodel.CategoryList = GetList();
            return View(qmodel);
        }

        public ActionResult TreePartial()
        {
            return PartialView("TreePartial", GetList());
        }

        private List<CategoryViewModel> GetList()
        {
            var result = (from a in dbcontent.ProjectCategorys
                          select new CategoryViewModel
                          {
                              CategoryId = a.Id,
                              ParentId = a.ParentId,
                              Name = a.Name,
                              Level = a.Level,
                              IsActive = a.IsActive,
                              Description = a.Description
                          }).ToList();

            return result;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewMenuPartial(CategoryViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProjectCategory item = new ProjectCategory
                    {
                        Name = vmodel.Name,
                        ParentId = vmodel.ParentId,
                        IsActive = true,
                        Level = vmodel.Level,
                        Description = vmodel.Description
                    };
                    dbcontent.ProjectCategorys.Add(item);
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditNodeError"] = e.Message;
                }
            }
            else
                ViewData["EditNodeError"] = "Please, correct all errors.";
            return PartialView("TreePartial", GetList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateMenuPartial(CategoryViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var item = dbcontent.ProjectCategorys.Where(t => t.Id == vmodel.CategoryId).FirstOrDefault();
                    item.Name = vmodel.Name;
                    item.Description = vmodel.Description;
                    item.Level = vmodel.Level;
                    item.IsActive = vmodel.IsActive;

                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditNodeError"] = e.Message;
                }
            }
            else
                ViewData["EditNodeError"] = "Please, correct all errors.";
            return PartialView("TreePartial", GetList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteMenuPartial(int CategoryId)
        {
            try
            {
                var item = dbcontent.ProjectCategorys.Where(t => t.Id == CategoryId).FirstOrDefault();
                dbcontent.ProjectCategorys.Remove(item);
                dbcontent.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["EditNodeError"] = e.Message;
            }
            return PartialView("TreePartial", GetList());
        }
    }
}
