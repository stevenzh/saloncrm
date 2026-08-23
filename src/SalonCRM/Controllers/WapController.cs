using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using SalonCRM.Manager;
using SalonCRM.Models;
using SalonCRM.Tools;

namespace SalonCRM.Controllers
{
    public class WapController : Controller
    {
        ILog logger = LogManager.GetLogger("WapController");
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        WxMemberManager wservice = new WxMemberManager();
        MemberManager mservice = new MemberManager();

        // GET: Wap
        public ActionResult Index(string code, string state)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            MemberViewModel entity = (MemberViewModel)Session["WeixinUser"];

            if (entity == null)
            {
                try
                {
                    //微信通过auth2，用code换取access_token
                    if (!string.IsNullOrEmpty(code))
                    {
                        var result = OAuthApi.GetAccessToken(bag.AppId, bag.Secret, code);
                        if (result.errcode == ReturnCode.请求成功)
                        {
                            ViewData["OpenID"] = result.openid;
                            entity = wservice.getMemberModel(result.openid, bag.HostID);
                            Session["WeixinUser"] = entity;
                        }
                    }
                    else
                    {
                        ViewData["OpenID"] = "ok6cAuKoOc85PZtNdKlSurbNiaGQ";
                        entity = wservice.getMemberModel("ok6cAuKoOc85PZtNdKlSurbNiaGQ", bag.HostID);
                        Session["WeixinUser"] = entity;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("用户显示错误", ex);
                }
            }
            if (entity == null)
                return Content("数据错误。");

            return View(entity);
        }

        public ActionResult MemberCard(string code, string state)
        {
            MemberViewModel entity = (MemberViewModel)Session["WeixinUser"];
            entity.Cards = (from _ in dbcontent.MemberCards
                            where _.MemberID == entity.MemberID && _.Status == 1
                            select new MemberCardModel
                            {
                                MemberCardId = _.MemberCardId,
                                MemberID = _.MemberID,
                                Title = _.Title,
                                Type = _.Type,
                                Amt = _.Amt,
                                UsedTime = _.UsedTime,
                                BookTime = _.BookTime,
                                LastCount = _.LastCount,
                                Amount = _.Amount,
                                Status = _.Status,
                                ActualPrice = _.ActualPrice,
                                CreatedDate = _.CreatedDate,
                                ExpiryDate = _.ExpiryDate
                            }).ToList();

            return View(entity);
        }

        public ActionResult MemberAccount(string code, string state)
        {
            MemberViewModel entity = (MemberViewModel)Session["WeixinUser"];
            entity.RechargeRecords = mservice.GetRechargeRecords(entity.MemberID);   // 充值记录

            return View(entity);
        }

        public ActionResult MemberProject(string code, string state)
        {
            MemberViewModel entity = (MemberViewModel)Session["WeixinUser"];
            entity.UsableProjects = mservice.GetAllProjects(entity.MemberID);      // 可用项目

            return View(entity);
        }

        public ActionResult MemberBook(string code, string state)
        {
            MemberViewModel entity = (MemberViewModel)Session["WeixinUser"];
            entity.ExpenseBooks = dbcontent.Books.Where(t => t.MemberID == entity.MemberID && t.State == "20").ToList();

            return View(entity);
        }

        [AllowAnonymous]
        public ActionResult Binding(string code, string state)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            WxMember member = new WxMember();

            try
            {
                //微信通过auth2，用code换取access_token
                if (!string.IsNullOrEmpty(code))
                {
                    var result = OAuthApi.GetAccessToken(bag.AppId, bag.Secret, code);
                    if (result.errcode == ReturnCode.请求成功)
                    {
                        ViewData["OpenID"] = result.openid;
                        member = dbcontent.WxMembers.Where(t => t.OpenID == result.openid).FirstOrDefault();
                        var mb = dbcontent.Members.Where(t => t.OpenID == result.openid).FirstOrDefault();
                        if (mb != null)
                        {
                            Session["CurrentMember"] = mb; // 当前客户
                        }
                        if (member != null)
                        {
                            // 取得微信个人信息
                            if (string.IsNullOrEmpty(member.NickName))
                            {
                                var u = OAuthApi.GetUserInfo(result.access_token, result.openid);
                                wservice.UpdateMember(bag.HostID, u.openid, "1", u.nickname, u.sex, u.city, u.province, u.country, u.headimgurl, DateTime.MinValue);
                                // 重新获取
                                member = dbcontent.WxMembers.Where(t => t.OpenID == result.openid).FirstOrDefault();
                            }
                        }
                        else
                        {
                            var u = OAuthApi.GetUserInfo(result.access_token, result.openid);
                            wservice.Subscribe(bag.HostID, u.openid, u.nickname, "", u.sex, u.city, u.province, u.country, u.headimgurl, DateTime.Now);
                            // 重新获取
                            member = dbcontent.WxMembers.Where(t => t.OpenID == result.openid).FirstOrDefault();
                        }

                        //Session["WeixinUser"] = member; // 当前客户
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("用户显示错误", ex);
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Binding(string CardNo, string OpenID, string Passwd)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];

            Member mb = dbcontent.Members.Where(t => t.HostID == bag.HostID && t.CardNo == CardNo && t.Passwd == Passwd).FirstOrDefault();
            if (mb != null)
            {
                mb.OpenID = OpenID;
                var xmb = dbcontent.WxMembers.Where(t => t.OpenID == mb.OpenID).FirstOrDefault();
                xmb.Binding = "1";

                dbcontent.SaveChanges();
                return Content("绑定成功。");
            }

            return Content("绑定失败。");
        }


        [AllowAnonymous]
        public ActionResult UserBinding(string code, string state)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            WxMember member = new WxMember();

            try
            {
                //微信通过auth2，用code换取access_token
                if (!string.IsNullOrEmpty(code))
                {
                    var result = OAuthApi.GetAccessToken(bag.AppId, bag.Secret, code);
                    if (result.errcode == ReturnCode.请求成功)
                    {
                        ViewData["OpenID"] = result.openid;
                        member = dbcontent.WxMembers.Where(t => t.OpenID == result.openid).FirstOrDefault();
                        var mb = dbcontent.Users.Where(t => t.OpenID == result.openid).FirstOrDefault();
                        if (mb != null)
                        {
                            Session["CurrentUser"] = mb; // 当前客户
                        }
                        if (member != null)
                        {
                            // 取得微信个人信息
                            if (string.IsNullOrEmpty(member.NickName))
                            {
                                var u = OAuthApi.GetUserInfo(result.access_token, result.openid);
                                wservice.UpdateMember(bag.HostID, u.openid, "1", u.nickname, u.sex, u.city, u.province, u.country, u.headimgurl, DateTime.MinValue);
                                // 重新获取
                                member = dbcontent.WxMembers.Where(t => t.OpenID == result.openid).FirstOrDefault();
                            }
                        }
                        else
                        {
                            var u = OAuthApi.GetUserInfo(result.access_token, result.openid);
                            wservice.Subscribe(bag.HostID, u.openid, u.nickname, "", u.sex, u.city, u.province, u.country, u.headimgurl, DateTime.Now);
                            // 重新获取
                            member = dbcontent.WxMembers.Where(t => t.OpenID == result.openid).FirstOrDefault();
                        }

                        // Session["WeixinUser"] = member; // 当前客户
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("用户显示错误", ex);
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UserBinding(string UserName, string Passwd, string OpenID)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            string Password = Security.ToEncrypt(Passwd);

            ApplicationUser mb = dbcontent.Users.Where(t => t.HostId == bag.HostID && t.UserName == UserName && t.Password == Password).FirstOrDefault();
            if (mb != null)
            {
                mb.OpenID = OpenID;
                var xmb = dbcontent.WxMembers.Where(t => t.OpenID == mb.OpenID).FirstOrDefault();
                xmb.EmployeeID = mb.Id;
                xmb.Binding = "1";

                dbcontent.SaveChanges();
                return Content("绑定成功。");
            }

            return Content("绑定失败。");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Appointment(long id)
        {
            var entity = dbcontent.Appointments.Where(_ => _.AppointmentID == id).FirstOrDefault();
            return View(entity);
        }

        public ActionResult Account(long id)
        {
            var entity = dbcontent.AccountRecords.Where(_ => _.RecordID == id).FirstOrDefault();
            return View(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Book(long id)
        {
            var entity = (from bb in dbcontent.Books.Where(t => t.BookID == id)
                          select new BookModel
                          {
                              CreatedDate = bb.CreatedDate,
                              CreatedBy = bb.CreatedBy,
                              ClientID = bb.ClientID,
                              BookID = bb.BookID,
                              Amount = bb.Amount,
                              BranchId = bb.BranchId,
                              HostID = bb.HostID,
                              MemberID = bb.MemberID,
                              SalesmanID = bb.SalesmanID,
                              Satisfaction = bb.Satisfaction,
                              State = bb.State,
                              Remark = bb.Remark,
                              BookProjects = bb.BookProjects,
                              Member = bb.Member,
                              Branch = dbcontent.Organs.Where(t => t.OrganID == bb.BranchId).FirstOrDefault(),
                              StateValue = dbcontent.Dictionaries.Where(a => a.KeyValue == bb.State && a.Identifier == "BookState").FirstOrDefault().Contents,
                          }).FirstOrDefault();

            return View(entity);
        }
    }
}