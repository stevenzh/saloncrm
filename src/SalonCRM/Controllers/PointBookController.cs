using SalonCRM.Identity;
using SalonCRM.Manager;
using SalonCRM.Models;
using SalonCRM.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 积分取得和消耗记录
    /// </summary>
    public class PointBookController : Controller
    {

        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: PointBook
        public ActionResult Index(PointBookQModel qModel)
        {
            CustomPrincipal cu = (CustomPrincipal)User;
            if (cu.Type != "2")
            {
                qModel.BranchId = GlobalContext.Current.UserDepartment.OrganID;
            }

            ViewData["BranchId"] = qModel.BranchId;
            ViewData["CardNo"] = qModel.CardNo;
            ViewData["MemberName"] = qModel.MemberName;
            ViewData["StartDate"] = qModel.StartDate;
            ViewData["EndDate"] = qModel.EndDate;

            InitDrop();
            qModel.PointList = GetList(qModel);
            return View(qModel);
        }

        public ActionResult PointList(PointBookQModel qModel)
        {
            ViewData["BranchId"] = qModel.BranchId;
            ViewData["CardNo"] = qModel.CardNo;
            ViewData["MemberName"] = qModel.MemberName;
            ViewData["StartDate"] = qModel.StartDate;
            ViewData["EndDate"] = qModel.EndDate;

            InitDrop();
            qModel.PointList = GetList(qModel);
            return PartialView("PointList", qModel.PointList);
        }


        private IList<PointBook> GetList(PointBookQModel viewModel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.PointBooks.Where(t => t.HostId == hostId);

            if (viewModel.BranchId != 0)
                query.Where(t => t.BranchId == viewModel.BranchId);
            if (viewModel.StartDate != default(DateTime))
                query = query.Where(t => t.CreatedDate > viewModel.StartDate);
            if (viewModel.EndDate != default(DateTime))
            {
                var d = viewModel.EndDate.AddDays(1);
                query = query.Where(t => t.CreatedDate < d);
            }

            return query.ToList();
        }


        private void InitDrop()
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            List<SelectListItem> items = new SelectList(dbcontent.Organs.Where(t => t.HostID == hostId).ToList(), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;
        }


        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppCreate(long MemberID, string Password, int OutPoints, string Remark, int HostID, int BranchId, string CreatedBy, string ClientId)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            var entity = new PointBook();
            try
            {
                var member = dbcontent.Members.Where(_ => _.MemberID.Equals(MemberID)).FirstOrDefault();

                if (Password.Trim() != member.Passwd)
                {
                    var result = new
                    {
                        code = 3,
                        message = "会员密码不正确。"
                    };
                    return Json(result);
                }

                if (OutPoints > member.Points)
                {
                    var result = new
                    {
                        code = 3,
                        message = "会员积分不足。"
                    };
                    return Json(result);
                }

                entity.HostId = HostID;
                entity.BranchId = BranchId;
                entity.MemberId = member.MemberID;
                entity.OutPoints = OutPoints;
                entity.RemainPoints = member.Points - OutPoints;
                entity.InOut = 2;
                entity.Remark = Remark;
                entity.ClientId = ClientId;
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = CreatedBy;

                if (member == null)
                {
                    var result = new
                    {
                        code = 2,
                        message = "无效的会员卡号"
                    };
                    return Json(result);
                }
                else
                {

                    dbcontent.PointBooks.Add(entity);

                    member.Points = member.Points - OutPoints;  // 顾客积分减除
                    dbcontent.SaveChanges();


                    var result = new
                    {
                        code = 1,
                        message = string.Empty
                    };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                var result = new
                {
                    code = 3,
                    message = "其他错误:" + ex.Message
                };
                return Json(entity);
            }
        }

    }
}