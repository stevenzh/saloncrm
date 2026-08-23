using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Identity;
using SalonCRM.Manager;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 门店管理
    /// </summary>
    [CustomAuthorize(Roles = "管理员")]
    public class BranchController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: Branch
        public ActionResult Index(BranchQModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            viewModel.BranchList = GetList();
            return View(viewModel);
        }

        public ActionResult BranchList()
        {
            return PartialView("BranchList", GetList());
        }

        private IList<BranchViewModel> GetList()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Organs.AsQueryable();
            if (!User.IsInRole("超级管理员"))
                query = query.Where(a => a.HostID == hostId);
            var BranchPageList = (from v in query
                                  select new BranchViewModel
                                  {
                                      OrganID = v.OrganID,
                                      HostName = v.Host.Name,
                                      Name = v.Name,
                                      Province = v.Province.Length > 0 ? dbcontent.Regions.Where(t => t.Code == v.Province).FirstOrDefault().Name : "",
                                      City = v.City.Length > 0 ? dbcontent.Regions.Where(t => t.Code == v.City).FirstOrDefault().Name : "",
                                      Address = v.Address,
                                      ClientNum = v.ClientNum,
                                      Manager = v.Manager,
                                      IsVaild = v.IsVaild,
                                      Phone = v.Phone,
                                      Level = v.Level
                                  }).ToList();

            return BranchPageList;
        }

        // GET: Branch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Branch/Create
        public ActionResult Create()
        {
            InitDrop(null);

            BranchViewModel model = new BranchViewModel();
            if (User.IsInRole("超级管理员"))
            {
                model.HostID = GlobalContext.Current.UserHost.HostID;
            }
            return View(model);
        }

        // POST: Branch/Create
        [HttpPost]
        public ActionResult Create(BranchViewModel formModel)
        {
            if (ModelState.IsValid)
            {
                var o = new Organ
                {
                    HostID = formModel.HostID,
                    Name = formModel.Name,
                    Province = formModel.Province,
                    City = formModel.City,
                    Address = formModel.City,
                    ClientNum = formModel.ClientNum,
                    Manager = formModel.Manager,
                    Phone = formModel.Phone
                };
                dbcontent.Organs.Add(o);
                dbcontent.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                InitDrop(formModel.Province);
                return View();
            }
        }

        // GET: Branch/Edit/5
        public ActionResult Edit(int id)
        {
            var h = (from v in dbcontent.Organs.Where(t => t.OrganID == id)
                     select new BranchViewModel
                     {
                         OrganID = v.OrganID,
                         Name = v.Name,
                         Manager = v.Manager,
                         Level = v.Level,
                         Address = v.Address,
                         Province = v.Province,
                         City = v.City,
                         ClientNum = v.ClientNum,
                         HostID = v.HostID,
                         IsVaild = v.IsVaild,
                         ParentID = v.ParentID,
                         Phone = v.Phone
                     }).FirstOrDefault();
            InitDrop(h.Province);
            return View(h);
        }

        // POST: Branch/Edit/5
        [HttpPost]
        public ActionResult Edit(BranchViewModel formModel)
        {
            if (ModelState.IsValid)
            {
                Organ org = dbcontent.Organs.FirstOrDefault(t => t.OrganID == formModel.OrganID);
                org.Name = formModel.Name;
                org.Address = formModel.Address;
                org.Manager = formModel.Manager;
                org.ClientNum = formModel.ClientNum;
                org.Phone = formModel.Phone;
                org.Level = formModel.Level;
                org.Province = formModel.Province;
                org.City = formModel.City;
                org.IsVaild = formModel.IsVaild;
                dbcontent.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                InitDrop(formModel.Province);
                return View();
            }
        }

        // GET: Branch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Branch/Delete/5
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

        [AllowAnonymous]
        public ActionResult AppList(int hostId)
        {
            var list = (from temp in dbcontent.Organs.Where(a => a.HostID == hostId).ToList()
                        select new SelectListItem
                        {
                            Text = temp.Name,
                            Value = temp.OrganID.ToString()
                        }).ToList();
            return Json(list);
        }

        /// <summary>
        /// 初始化下拉菜单
        /// </summary>
        private void InitDrop(string province)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            IList<Region> dd = CommonManager.GetProvinces();
            ViewBag.Provinces = new SelectList(dd, "Code", "Name");
            if (String.IsNullOrEmpty(province))
                ViewBag.Citys = new SelectList(CommonManager.GetCitys(dd.First().Code), "Code", "Name");
            else
                ViewBag.Citys = new SelectList(CommonManager.GetCitys(province), "Code", "Name");

            if (User.IsInRole("超级管理员"))
            {
                ViewBag.HostList = new SelectList(dbcontent.Hosts.ToList(), "HostID", "Name");
            }
        }
    }
}
