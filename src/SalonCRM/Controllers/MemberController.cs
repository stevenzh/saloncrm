using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.AdvancedAPIs;
using SalonCRM.Models;
using SalonCRM.Manager;
using SalonCRM.Web;
using SalonCRM.Identity;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 微信客户
    /// </summary>
    [CustomAuthorize]
    public class MemberController : BaseController
    {
        ILog logger = LogManager.GetLogger("MemberController");
        WxMemberManager service = new WxMemberManager();
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        // GET: Member
        public ActionResult Index(WxMemberQModel model)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            model.HostID = bag.HostID;
            ViewData["Name"] = model.Name;
            model.MemberList = service.GetMember(model);
            // 关注列表
            List<SelectListItem> subscribelist = new List<SelectListItem>();
            subscribelist.Add(new SelectListItem { Text = "所有", Value = "", Selected = true });
            subscribelist.Add(new SelectListItem { Text = "关注", Value = "1" });
            subscribelist.Add(new SelectListItem { Text = "不关注", Value = "0" });
            ViewBag.SubscribeList = subscribelist;
            // 绑定列表
            List<SelectListItem> bindinglist = new List<SelectListItem>();
            bindinglist.Add(new SelectListItem { Text = "所有", Value = "", Selected = true });
            bindinglist.Add(new SelectListItem { Text = "绑定", Value = "1" });
            bindinglist.Add(new SelectListItem { Text = "未绑定", Value = "0" });
            ViewBag.BindingList = bindinglist;

            return View(model);
        }
        public ActionResult PageList(WxMemberQModel model)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];

            ViewData["Name"] = model.Name;
            model.HostID = bag.HostID;
            model.MemberList = service.GetMember(model);
            return PartialView("PageList", model);
        }
        public ActionResult SyncUser()
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            // 取得所有关注用户
            var accessToken = AccessTokenContainer.TryGetAccessToken(bag.AppId, bag.Secret);
            var result = UserApi.Get(accessToken, "");
            for (var i = 0; i < result.count; i++)
            {
                var ss = result.data.openid[i];
                accessToken = AccessTokenContainer.GetAccessToken(bag.AppId);
                UserInfoJson info = UserApi.Info(accessToken, ss);
                service.UpdateMember(bag.HostID, ss, "1", info.nickname, info.sex, info.city, info.province,
                    info.country, info.headimgurl, DateTimeHelper.GetDateTimeFromXml(info.subscribe_time));
                logger.Info("Current row:" + i + ",OpenID:" + ss);
            }
            return Json(new { Success = "true", Message = "更新成功！" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SyncOneUser(string openID)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            // 取得所有关注用户
            var accessToken = AccessTokenContainer.TryGetAccessToken(bag.AppId, bag.Secret);
            UserInfoJson info = UserApi.Info(accessToken, openID);
            service.UpdateMember(bag.HostID, openID, info.subscribe.ToString(), info.nickname, info.sex, info.city, info.province,
                info.country, info.headimgurl, DateTimeHelper.GetDateTimeFromXml(info.subscribe_time));
            return Json(new { Success = "true", Message = "更新成功！" }, JsonRequestBehavior.AllowGet);
        }


        // GET: Member/Details/5
        public ActionResult Details(int id)
        {
            // 绑定列表
            List<SelectListItem> bindinglist = new List<SelectListItem>();
            bindinglist.Add(new SelectListItem { Text = "绑定", Value = "1" });
            bindinglist.Add(new SelectListItem { Text = "未绑定", Value = "0" });
            ViewBag.BindingList = bindinglist;

            MemberModel model = service.getMemberByID(id);
            ViewData["OpenID"] = model.OpenID;
            return View(model);
        }


        public ActionResult SendMessage(MemberMessage message)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            string userId = GlobalContext.Current.UserInfo.Id;
            // 直客微信发消息
            var accessToken = AccessTokenContainer.TryGetAccessToken(bag.AppId, bag.Secret);
            var result = CustomApi.SendText(accessToken, message.OpenID, message.Content);
            if (result.errmsg == "ok")
            {
                message.CreatedBy = userId;
                message.InOut = 1;
                message.CreatedDate = DateTime.Now;
                dbcontent.SaveChanges();

                return Content("ok");
            }

            return Content("error");
        }
    }
}