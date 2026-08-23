using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;
using Common.Logging;
using NPOI.HSSF.UserModel;
using SalonCRM.Models;
using SalonCRM.Web;
using DevExpress.Web.Mvc;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 项目管理
    /// </summary>
    [Authorize(Roles = "管理员")]
    public class ProjectController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProjectController));

        // GET: Project
        public ActionResult Index(ProjectQModel viewModel, string message)
        {
            ViewBag.Message = message;

            ViewData["ProjectName"] = viewModel.ProjectName;
            ViewData["Category"] = viewModel.Category;
            ViewData["ProjectStatus"] = viewModel.ProjectStatus;

            var items = new SelectList(dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "ProjectCategory").ToList(), "KeyValue", "Contents").ToList();
            items.Insert(0, new SelectListItem { Text = "请选择", Value = "", Selected = true });
            ViewBag.CategoryList = items;

            var items1 = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "ProjectStatus").ToList(), "KeyValue", "Contents").ToList();
            items1.Insert(0, new SelectListItem { Text = "请选择", Value = "", Selected = true });
            ViewBag.StatusList = items1;

            viewModel.ProjectList = GetList(viewModel);
            return View(viewModel);
        }

        public ActionResult ProjectList(ProjectQModel viewModel)
        {
            ViewData["ProjectName"] = viewModel.ProjectName;
            ViewData["Category"] = viewModel.Category;
            ViewData["ProjectStatus"] = viewModel.ProjectStatus;

            return PartialView("ProjectList", GetList(viewModel));
        }

        private List<ProjectViewModel> GetList(ProjectQModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Projects.Where(a => a.HostID == hostId);
            if (!string.IsNullOrEmpty(viewModel.ProjectName))
                query = query.Where(t => t.Name.Contains(viewModel.ProjectName));
            if (!string.IsNullOrEmpty(viewModel.Category))
                query = query.Where(t => t.Category == viewModel.Category);
            var ProjectList = (from d in query
                               select new ProjectViewModel
                               {
                                   ProjectID = d.ProjectID,
                                   Code = d.Code,
                                   Category = d.Category,
                                   Name = d.Name,
                                   MinUnit = d.MinUnit,
                                   Brand = d.Brand,
                                   ExtCategory = d.ExtCategory,
                                   ExtCategoryName = dbcontent.Dictionaries.Where(t => t.Identifier == "ProjectExtCategory" && t.KeyValue == d.ExtCategory).FirstOrDefault().Contents,
                                   SecCategory = d.SecCategory,
                                   HandicraftFee = d.HandicraftFee,
                                   LowHandicraftFee = d.LowHandicraftFee,
                                   IsEntity = d.IsEntity,
                                   Status = d.Status,
                                   StatusValue = dbcontent.Dictionaries.Where(t => t.Identifier == "ProjectStatus" && t.KeyValue == d.Status).FirstOrDefault().Contents,
                                   BrandValue = dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "ProjectBrand" && t.KeyValue == d.Brand).FirstOrDefault().Contents,
                                   CategoryName = dbcontent.Dictionaries.Where(t => t.Identifier == "ProjectCategory" && t.KeyValue == d.Category).FirstOrDefault().Contents
                               }).ToList();
            return ProjectList;
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            InitDrop();
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(Project formModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    formModel.Status = "20";
                    dbcontent.Projects.Add(formModel);
                    dbcontent.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    logger.Error("Project Create failure.", ex);
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        logger.Error(eve.ToString());
                    }
                }
            }
            InitDrop();
            return View();
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            Project prj = dbcontent.Projects.FirstOrDefault(t => t.ProjectID == id);
            ViewData["ProjectID"] = id;
            InitDrop();
            InitDrop2();
            return View(prj);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Project formModel)
        {
            if (ModelState.IsValid)
            {
                Project prj = dbcontent.Projects.FirstOrDefault(t => t.ProjectID == formModel.ProjectID);
                prj.Name = formModel.Name;
                prj.Category = formModel.Category;
                prj.Brand = formModel.Brand;
                prj.MinUnit = formModel.MinUnit;
                prj.HandicraftFee = formModel.HandicraftFee;
                prj.LowHandicraftFee = formModel.LowHandicraftFee;
                prj.ExtCategory = formModel.ExtCategory;
                prj.SecCategory = formModel.SecCategory;
                prj.IsEntity = formModel.IsEntity;
                prj.Status = formModel.Status;
                dbcontent.SaveChanges();
                return RedirectToAction("Index");
            }
            InitDrop();
            return View();
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
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


        public ActionResult GoodsList(long ProjectID)
        {
            ViewData["ProjectID"] = ProjectID;
            InitDrop();
            InitDrop2();
            var list = dbcontent.ProjectGoods.Where(t => t.ProjectID == ProjectID).ToList();
            return PartialView("GoodsList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewSplitPartial(ProjectGoods model)
        {
            ViewData["ProjectID"] = model.ProjectID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = new ProjectGoods
                    {
                        GoodsID = Convert.ToInt32(model.Goods.Name),
                        ProjectID = model.ProjectID,
                        Quantity = model.Quantity
                    };

                    dbcontent.ProjectGoods.Add(m);
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
            InitDrop2();
            var list = dbcontent.ProjectGoods.Where(t => t.ProjectID == model.ProjectID).ToList();
            return PartialView("GoodsList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateSplitPartial(ProjectGoods model)
        {
            ViewData["ProjectID"] = model.ProjectID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.ProjectGoods.Where(t => t.ProjectGoodsID == model.ProjectGoodsID).FirstOrDefault();
                    if (m.Goods.Name != model.Goods.Name)
                    {
                        m.GoodsID = Convert.ToInt32(model.Goods.Name);
                    }
                    m.Quantity = model.Quantity;
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
            InitDrop2();
            var list = dbcontent.ProjectGoods.Where(t => t.ProjectID == model.ProjectID).ToList();
            return PartialView("GoodsList", list);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteSplitPartial(ProjectGoods model)
        {
            ViewData["ProjectID"] = model.ProjectID;
            string userId = GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.ProjectGoods.Where(t => t.ProjectGoodsID == model.ProjectGoodsID).FirstOrDefault();
                    if (m != null)
                    {
                        dbcontent.ProjectGoods.Remove(m);
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

            InitDrop2();
            var list = dbcontent.ProjectGoods.Where(t => t.ProjectID == model.ProjectID).ToList();
            return PartialView("GoodsList", list);
        }

        public ActionResult GetGoods(string Category, string textField)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            return GridViewExtension.GetComboBoxCallbackResult(p =>
            {
                p.TextField = textField;
                p.ValueField = "GoodsID";
                p.BindList(dbcontent.Goods.Where(t => t.HostID == hostId && t.Category == Category).ToList());
            });
        }


        /// <summary>
        /// 初始化下拉菜单
        /// </summary>
        private void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            ViewBag.CategoryList = new SelectList(dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "ProjectCategory").ToList(), "KeyValue", "Contents");
            ViewBag.ExtCategoryList = new SelectList(dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "ProjectExtCategory").ToList(), "KeyValue", "Contents");
            ViewBag.BrandList = new SelectList(dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.IsVaild == 1 && t.Identifier == "ProjectBrand").ToList(), "KeyValue", "Contents");
            ViewBag.StatusList = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "ProjectStatus").ToList(), "KeyValue", "Contents");
        }

        private void InitDrop2()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            ViewBag.GoodsList = (from vv in dbcontent.Goods.Where(t => t.HostID == hostId && t.IsVaild == 1)
                                 select new
                                 {
                                     Text = vv.Name + "-" + vv.Unit,
                                     Value = vv.GoodsID
                                 }).ToList();
        }

        [HttpPost]
        public ActionResult XlsImport(FormCollection formValues)
        {
            string IsCheck = formValues["IsDataCheck"];
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
                    var allCategory = dbcontent.Dictionaries.Where(t => t.Identifier == "ProjectCategory" && t.IsVaild == 1).ToList();
                    var allBrand = dbcontent.Dictionaries.Where(t => t.HostId == HostID && t.Identifier == "ProjectBrand").ToList();
                    var allExtCategory = dbcontent.Dictionaries.Where(t => t.Identifier == "ProjectExtCategory" && t.IsVaild == 1).ToList();


                    for (int rowIndex = 1; rowIndex < rowsCount; rowIndex++)
                    {
                        var c0 = sheet.GetRow(rowIndex).GetCell(0);
                        string code = (c0 == null ? "" : c0.ToString());
                        string brand = sheet.GetRow(rowIndex).GetCell(1).ToString();
                        string extCategory = sheet.GetRow(rowIndex).GetCell(2).ToString();
                        string name = sheet.GetRow(rowIndex).GetCell(3).ToString();
                        if (string.IsNullOrEmpty(name))
                            continue;
                        var obj = sheet.GetRow(rowIndex).GetCell(4);
                        string price = sheet.GetRow(rowIndex).GetCell(4).ToString();
                        string category = sheet.GetRow(rowIndex).GetCell(5).ToString();
                        string cost = sheet.GetRow(rowIndex).GetCell(6).ToString();
                        string entity = sheet.GetRow(rowIndex).GetCell(7).ToString();

                        if (IsCheck.IndexOf("true") > -1)
                        {
                            // CODE重复检测
                            //int r = dbcontent.Projects.Where(t => t.Code == prj.Code && t.HostID == HostID).Count();
                            //if (r > 0)
                            //{
                            //    msg.Append("第" + rowIndex + @"行，CODE重复；<br />");
                            //    continue;
                            //}
                            Dictionary a = allExtCategory.Where(t => t.Contents == extCategory).FirstOrDefault();
                            if (a == null)
                            {
                                msg.Append("第" + (rowIndex + 1) + @"行，项目来源不存在；<br />");
                                continue;
                            }
                            if (string.IsNullOrEmpty(name))
                            {
                                msg.Append("第" + (rowIndex + 1) + @"行，项目名称为空；<br />");
                                continue;
                            }
                            Dictionary c = allCategory.Where(t => t.Contents == category).FirstOrDefault();
                            if (c == null)
                            {
                                msg.Append("第" + (rowIndex + 1) + @"行，类别不存在；<br />");
                                continue;
                            }
                            if (!string.IsNullOrEmpty(brand))
                            {
                                Dictionary b = allBrand.Where(t => t.Contents == brand).FirstOrDefault();
                                if (b == null)
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，品牌不存在；<br />");
                                    continue;
                                }
                            }
                            Regex objNotNumberPattern = new Regex("^[0-9]*$");
                            if (string.IsNullOrEmpty(price))
                            {
                                msg.Append("第" + (rowIndex + 1) + @"行，价格为空；<br />");
                                continue;
                            }
                            else if (!objNotNumberPattern.IsMatch(price))
                            {
                                msg.Append("第" + (rowIndex + 1) + @"行，价格不是整数；<br />");
                                continue;
                            }
                            if (!string.IsNullOrEmpty(cost))
                            {
                                if (!objNotNumberPattern.IsMatch(cost))
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，手工费不是数字；<br />");
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            Dictionary c = allCategory.Where(t => t.Contents == category).FirstOrDefault();
                            Dictionary a = allExtCategory.Where(t => t.Contents == extCategory).FirstOrDefault();

                            Project prj = new Project
                            {
                                Code = code,
                                Count = 0,
                                HostID = HostID,
                                Category = c.KeyValue,
                                MinUnit = Convert.ToInt32(price),
                                Name = name,
                                ExtCategory = a.KeyValue,
                                Status = "20"
                            };
                            if (!string.IsNullOrEmpty(brand))
                            {
                                Dictionary b = allBrand.Where(t => t.Contents == brand).FirstOrDefault();
                                if (b == null)
                                {
                                    prj.Brand = b.KeyValue;
                                }
                            }
                            if (!string.IsNullOrEmpty(entity))
                            {
                                if (entity == "是")
                                {
                                    prj.IsEntity = 1;
                                }
                            }
                            if (!string.IsNullOrEmpty(cost))
                            {
                                prj.HandicraftFee = Convert.ToInt32(cost);
                            }

                            dbcontent.Projects.Add(prj);
                        }

                        row++;
                    }
                    dbcontent.SaveChanges();

                    ViewBag.Message = "文件总行数:" + rowsCount + ",成功导入的项目行数：" + row + "<br/>" + msg.ToString();
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

        #region 终端使用
        /// <summary>
        /// 终端取得项目类型
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppCategory(int hostId, int branchId)
        {
            var list = (from _ in dbcontent.Dictionaries.Where(a => a.Identifier == "ProjectCategory" && a.IsVaild == 1)
                        select new
                        {
                            code = _.KeyValue,
                            name = _.Contents
                        }).ToList();
            return Json(list);
        }

        /// <summary>
        /// 终端取得项目列表
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppList(int hostId, string Category, string ExtCategory, string q)
        {
            var query = dbcontent.Projects.Where(a => a.HostID == hostId && a.Status == "20");
            if (!String.IsNullOrEmpty(ExtCategory))
                query = query.Where(a => a.ExtCategory == ExtCategory);
            if (!String.IsNullOrEmpty(Category))
                query = query.Where(a => a.Category == Category);
            if (!String.IsNullOrEmpty(q))
                query = query.Where(a => a.Name.Contains(q));
            var list = (from _ in query
                        select new
                        {
                            id = _.ProjectID,
                            code = _.Code,
                            name = _.Name,
                            unit = _.MinUnit,
                            entity = _.IsEntity
                        }).ToList();     // .Take(10) 仅取得10条
            return Json(list);
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult GetList(int hostId, string Category)
        {
            return AppList(hostId, Category, "", "");
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult GetCategory(int hostId, int branchId)
        {
            return AppCategory(hostId, branchId);
        }
        #endregion

    }
}
