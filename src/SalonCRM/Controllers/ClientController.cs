using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Identity;

namespace SalonCRM.Controllers
{
    [CustomAuthorize(Roles = "管理员")]
    public class ClientController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        // GET: Client
        public ActionResult Index(ClientQModel viewModel)
        {
            //ViewData["ProjectName"] = viewModel.ProjectName;
            //ViewData["Category"] = viewModel.Category;
            viewModel.ClientList = GetList(viewModel);
            return View(viewModel);
        }

        public ActionResult PageList(ClientQModel viewModel)
        {
            //ViewData["ProjectName"] = viewModel.ProjectName;
            //ViewData["Category"] = viewModel.Category;
            return PartialView("PageList", GetList(viewModel));
        }

        private List<ClientViewModel> GetList(ClientQModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.Clients.Where(a => a.HostID == hostId);
            //if (!string.IsNullOrEmpty(viewModel.ProjectName))
            //    query = query.Where(t => t.Name.Contains(viewModel.ProjectName));
            //if (!string.IsNullOrEmpty(viewModel.Category))
            //    query = query.Where(t => t.Category == viewModel.Category);
            var ClientList = (from d in query
                              select new ClientViewModel
                              {
                                  ClientID = d.ClientID,
                                  HostID = d.HostID,
                                  IsVaild = d.IsVaild,
                                  IsVaildValue = dbcontent.Dictionaries.Where(t => t.Identifier == "ClientIsVald" && t.KeyValue == d.IsVaild).FirstOrDefault().Contents,
                                  MobileGUID = d.MobileGUID,
                                  MobileNumber = d.MobileNumber,
                                  MobileModel = d.MobileModel,
                                  OrganID = d.OrganID,
                                  UserId = d.UserId,
                                  LastSignIn = d.LastSignIn,
                                  BranchName = dbcontent.Organs.Where(t => t.OrganID == d.OrganID).FirstOrDefault().Name,
                                  UserName = dbcontent.Users.Where(t => t.Id == d.UserId).FirstOrDefault().UserCnName
                              }).ToList();
            return ClientList;
        }

        // GET: Client/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            InitDrop();
            return View();
        }

        // POST: Client/Create
        [HttpPost]
        public ActionResult Create(Client formModel)
        {
            try
            {
                formModel.HostID = GlobalContext.Current.UserHost.HostID;

                // 验证终端数量
                int count = dbcontent.Clients.Where(t => t.HostID == formModel.HostID && t.IsVaild == "1").Count();
                int clientnum = dbcontent.Hosts.Where(t => t.HostID == formModel.HostID).FirstOrDefault().ClientNum.Value;

                if (count >= clientnum)
                {
                    ModelState.AddModelError("", "终端数量有限，和管理员联系。");
                    return View(formModel);
                }

                dbcontent.Clients.Add(formModel);
                dbcontent.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Client/Edit/5
        public ActionResult Edit(int id)
        {
            InitDrop();
            return View(dbcontent.Clients.FirstOrDefault(t => t.ClientID == id));
        }

        // POST: Client/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Client formModel)
        {
            try
            {
                int hostid = GlobalContext.Current.UserHost.HostID;
                Client prj = dbcontent.Clients.FirstOrDefault(t => t.ClientID == formModel.ClientID);

                // 验证终端数量
                int count = dbcontent.Clients.Where(t => t.HostID == hostid && t.IsVaild == "1").Count();
                int clientnum = dbcontent.Hosts.Where(t => t.HostID == hostid).FirstOrDefault().ClientNum.Value;

                if (count >= clientnum && prj.IsVaild == "0" && formModel.IsVaild == "1")
                {
                    ModelState.AddModelError("", "终端数量有限，和管理员联系。");
                    InitDrop();
                    return View(formModel);
                }

                prj.MobileGUID = formModel.MobileGUID;
                prj.MobileNumber = formModel.MobileNumber;
                prj.OrganID = formModel.OrganID;
                prj.IsVaild = formModel.IsVaild;
                dbcontent.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                InitDrop();
                return View();
            }
        }


        /// <summary>
        /// 初始化下拉菜单
        /// </summary>
        private void InitDrop()
        {
            ViewBag.HostId = new SelectList(dbcontent.Hosts.ToList(), "HostID", "Name");
            ViewBag.OrganId = new SelectList(dbcontent.Organs.Where(t => t.HostID == GlobalContext.Current.UserHost.HostID).ToList(), "OrganID", "Name");
            //ViewBag.ClientIsVald = new SelectList(dbcontent.Dictionaries.Where(t => t.Identifier == "ClientIsVald").ToList(), "KeyValue", "Contents");
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Client/Delete/5
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
    }
}
