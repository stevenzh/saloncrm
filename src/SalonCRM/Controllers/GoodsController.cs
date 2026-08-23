using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using SalonCRM.Models;
using NPOI.HSSF.UserModel;
using log4net;

namespace SalonCRM.Controllers
{
    public class GoodsController : Controller
    {
        // GET: Goods
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProjectController));

        // GET: CardTmpl
        public ActionResult Index(GoodsQModel model)
        {
            InitDrop();
            //ViewData["GoodsName"] = model.GoodsName;
            //ViewData["Category"] = model.Category;
            model.GoodsList = GetList(model);
            return View(model);
        }

        public ActionResult GoodsList(GoodsQModel model)
        {
            InitDrop();
            //ViewData["GoodsName"] = model.GoodsName;
            //ViewData["Category"] = model.Category;
            return PartialView("GoodsList", GetList(model));
        }


        private List<GoodsViewModel> GetList(GoodsQModel model)
        {
            int hostId = Web.GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Goods.Where(t => t.HostID == hostId);
            if (!string.IsNullOrEmpty(model.GoodsName))
                query = query.Where(t => t.Name.Contains(model.GoodsName));
            if (!string.IsNullOrEmpty(model.Category))
                query = query.Where(t => t.Category == model.Category);

            var list = (from gg in query
                        select new GoodsViewModel
                        {
                            HostID = gg.HostID,
                            GoodsID = gg.GoodsID,
                            Brand = gg.Brand,
                            Category = gg.Category,
                            GoodsCode = gg.GoodsCode,
                            IsVaild = gg.IsVaild,
                            Name = gg.Name,
                            Unit = gg.Unit
                        }).ToList();

            return list;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewGoodsPartial(GoodsViewModel model)
        {
            int hostId = Web.GlobalContext.Current.UserHost.HostID;
            //ViewData["GoodsName"] = model.GoodsName;
            //ViewData["Category"] = model.Category;

            if (ModelState.IsValid)
            {
                try
                {
                    var m = new Goods
                    {
                        GoodsCode = model.GoodsCode,
                        Name = model.Name,
                        HostID = hostId,
                        Unit = model.Unit,
                        IsVaild = 1,
                        Brand = model.Brand,
                        Category = model.Category
                    };

                    dbcontent.Goods.Add(m);
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
            return PartialView("GoodsList", GetList(new GoodsQModel { GoodsName = model.Name, Category = model.Category }));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateGoodsPartial(GoodsViewModel model)
        {
            string userId = Web.GlobalContext.Current.UserInfo.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    var m = dbcontent.Goods.Where(t => t.GoodsID == model.GoodsID).FirstOrDefault();
                    m.GoodsCode = model.GoodsCode;
                    m.Name = model.Name;
                    m.Brand = model.Brand;
                    m.Category = model.Category;
                    m.IsVaild = model.IsVaild;
                    m.Unit = model.Unit;
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
            return PartialView("GoodsList", GetList(new GoodsQModel { GoodsName = model.Name, Category = model.Category }));
        }

        private void InitDrop()
        {
            int hostId = Web.GlobalContext.Current.UserHost.HostID;

            ViewBag.CategoryList = dbcontent.Dictionaries.Where(t => t.IsVaild == 1 && t.Identifier == "ProjectCategory").ToList();
            ViewBag.BrandList = dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.IsVaild == 1 && t.Identifier == "ProjectBrand").ToList();
        }

        [HttpPost]
        public ActionResult XlsImport(FormCollection formValues)
        {
            string IsCheck = formValues["IsDataCheck"];
            try
            {
                StringBuilder msg = new StringBuilder();
                int HostID = Web.GlobalContext.Current.UserHost.HostID;
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

                    for (int rowIndex = 1; rowIndex < rowsCount; rowIndex++)
                    {
                        Goods prj = new Goods();
                        prj.GoodsCode = sheet.GetRow(rowIndex).GetCell(0).ToString();
                        string brand = sheet.GetRow(rowIndex).GetCell(1).ToString();
                        string name = sheet.GetRow(rowIndex).GetCell(2).ToString();
                        string unit = sheet.GetRow(rowIndex).GetCell(3).ToString();
                        string category = sheet.GetRow(rowIndex).GetCell(4).ToString();

                        if (IsCheck.IndexOf("true") > -1)
                        {
                            // CODE重复检测
                            //int r = dbcontent.Projects.Where(t => t.Code == prj.Code && t.HostID == HostID).Count();
                            //if (r > 0)
                            //{
                            //    msg.Append("第" + (rowIndex + 1) + @"行，CODE重复；<br/>");
                            //    continue;
                            //}
                            Dictionary c = allCategory.Where(t => t.Contents == category).FirstOrDefault();
                            if (c == null)
                            {
                                msg.Append("第" + (rowIndex + 1) + @"行，类别不存在；<br/>");
                                continue;
                            }
                            if (!string.IsNullOrEmpty(brand))
                            {
                                Dictionary b = allBrand.Where(t => t.Contents == brand).FirstOrDefault();
                                if (b == null)
                                {
                                    msg.Append("第" + (rowIndex + 1) + @"行，品牌不存在；<br/>");
                                    continue;
                                }
                                prj.Brand = b.KeyValue;
                            }
                            Regex objNotNumberPattern = new Regex("^(-?\\d+)(\\.\\d+)?$");
                            if (!objNotNumberPattern.IsMatch(unit))
                            {
                                msg.Append("第" + (rowIndex + 1) + @"行，价格不是数字；<br />");
                                continue;
                            }
                        }
                        else
                        {
                            Dictionary c = allCategory.Where(t => t.Contents == category).FirstOrDefault();

                            prj.HostID = HostID;
                            prj.Category = c.KeyValue;
                            prj.Name = name;
                            prj.Unit = unit;
                            prj.IsVaild = 1;

                            dbcontent.Goods.Add(prj);
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
    }
}