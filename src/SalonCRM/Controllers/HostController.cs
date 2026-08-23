using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Manager;

namespace SalonCRM.Controllers
{
    [Authorize(Roles = "超级管理员")]
    /// <summary>
    /// 多商户支持
    /// </summary>
    public class HostController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: Host
        public ActionResult Index()
        {
            return View(GetList());
        }
        public ActionResult HostList()
        {
            return PartialView("HostList", GetList());
        }

        private List<HostViewModel> GetList()
        {
            List<HostViewModel> h = (from d in dbcontent.Hosts
                                     select new HostViewModel
                                     {
                                         HostCode = d.HostCode,
                                         HostID = d.HostID,
                                         Name = d.Name,
                                         Address = d.Address,
                                         Province = d.Province.Length > 0 ? dbcontent.Regions.Where(t => t.Code == d.Province).FirstOrDefault().Name : "",
                                         City = d.City.Length > 0 ? dbcontent.Regions.Where(t => t.Code == d.City).FirstOrDefault().Name : "",
                                         Url = d.Url,
                                         IsVaild = d.IsVaild,
                                         BranchNum = d.BranchNum,
                                         ClientNum = d.ClientNum,
                                         Manager = d.Manager,
                                         Industry = d.Industry,
                                         StartDate = d.StartDate,
                                         EndDate = d.EndDate
                                     }).ToList();
            return h;
        }

        //// GET: Host/Details/5
        //public ActionResult Details(int id)
        //{
        //    var model = (from v in dbcontent.Hosts.Where(t => t.HostID == id)
        //                 select new HostViewModel
        //                 {
        //                     HostID = v.HostID,
        //                     HostCode = v.HostCode,
        //                     Name = v.Name,
        //                     Province = v.Province,
        //                     City = v.City,
        //                     Address = v.Address,
        //                     BranchNum = v.BranchNum,
        //                     Manager = v.Manager,
        //                     StartDate = v.StartDate,
        //                     ClientNum = v.ClientNum,
        //                     EndDate = v.EndDate,
        //                     Industry = v.Industry
        //                 }).FirstOrDefault();
        //    return View(model);
        //}

        // GET: Host/Create
        public ActionResult Create()
        {
            InitDrop(null);
            return View();
        }

        // POST: Host/Create
        [HttpPost]
        public ActionResult Create(HostViewModel formModel)
        {
            if (ModelState.IsValid)
            {
                Host h = new Host
                {
                    HostCode = formModel.HostCode,
                    Name = formModel.Name,
                    Province = formModel.Province,
                    City = formModel.City,
                    Address = formModel.Address,
                    BranchNum = formModel.BranchNum,
                    ClientNum = formModel.ClientNum,
                    Industry = formModel.Industry,
                    Url = formModel.Url,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddYears(1),
                    IsVaild = 1
                };
                dbcontent.Hosts.Add(h);
                dbcontent.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                InitDrop(formModel.Province);
                ModelState.AddModelError("", "表单字段验证错误.");
                return View();
            }
        }

        // GET: Host/Edit/5
        public ActionResult Edit(int id)
        {
            var model = (from v in dbcontent.Hosts.Where(t => t.HostID == id)
                         select new HostViewModel
                         {
                             HostID = v.HostID,
                             HostCode = v.HostCode,
                             Name = v.Name,
                             Province = v.Province,
                             City = v.City,
                             Address = v.Address,
                             BranchNum = v.BranchNum,
                             Manager = v.Manager,
                             StartDate = v.StartDate,
                             ClientNum = v.ClientNum,
                             Url = v.Url,
                             EndDate = v.EndDate,
                             Industry = v.Industry,
                             IsVaild = v.IsVaild,
                         }).FirstOrDefault();

            model.Profiles = GetProfiles(id);
            ViewData["HostID"] = id;

            InitDrop(model.Province);
            return View(model);
        }

        // POST: Host/Edit/5
        [HttpPost]
        public ActionResult Edit(Host formModel)
        {
            if (ModelState.IsValid)
            {
                Host prj = dbcontent.Hosts.FirstOrDefault(t => t.HostID == formModel.HostID);
                prj.Name = formModel.Name;
                prj.BranchNum = formModel.BranchNum;
                prj.ClientNum = formModel.ClientNum;
                prj.Url = formModel.Url;
                prj.Industry = formModel.Industry;
                prj.Province = formModel.Province;
                prj.City = formModel.City;
                prj.Address = formModel.Address;
                prj.IsVaild = formModel.IsVaild;
                dbcontent.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                InitDrop(formModel.Province);
                return View();
            }
        }

        // GET: Host/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Host/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditProfile(int id)
        {
            ViewData["HostID"] = id;
            InitDrop(null);
            return PartialView("EditProfile", GetProfiles(id));
        }

        private List<HostProfileModel> GetProfiles(int id)
        {
            var model = (from v in dbcontent.HostProfiles.Where(t => t.HostID == id)
                         select new HostProfileModel
                         {
                             HostID = v.HostID,
                             ProfileID = v.ProfileID,
                             PropertyText = v.PropertyText,
                             PropertyValue = v.PropertyValue
                         }).ToList();

            return model;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProfile(HostProfileModel product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = dbcontent.HostProfiles.Where(t => t.ProfileID == product.ProfileID).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.PropertyText = product.PropertyText;
                        entity.PropertyValue = product.PropertyValue;
                    }
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop(null);
            return PartialView("EditProfile", GetProfiles(product.HostID));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewProfile(HostProfileModel product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcontent.HostProfiles.Add(new HostProfile
                    {
                        HostID = product.HostID,
                        PropertyText = product.PropertyText,
                        PropertyValue = product.PropertyValue
                    });
                    dbcontent.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            InitDrop(null);
            return PartialView("EditProfile", GetProfiles(product.HostID));
        }

        /// <summary>
        /// 初始化下拉菜单
        /// </summary>
        private void InitDrop(string province)
        {
            IList<Region> dd = CommonManager.GetProvinces();
            ViewBag.Provinces = new SelectList(dd, "Code", "Name");
            if (String.IsNullOrEmpty(province))
                ViewBag.Citys = new SelectList(CommonManager.GetCitys(dd.First().Code), "Code", "Name");
            else
                ViewBag.Citys = new SelectList(CommonManager.GetCitys(province), "Code", "Name");

            // 客户类型
            List<SelectListItem> items2 = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "HostProfiles").ToList(), "KeyValue", "Contents").ToList();
            items2.Insert(0, new SelectListItem { Value = "", Text = "--请选择--" });
            ViewBag.ProfilesDict = items2;
        }
    }
}
