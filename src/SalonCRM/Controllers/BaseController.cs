using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using GlobalContext = SalonCRM.Web.GlobalContext;

namespace SalonCRM.Controllers
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
            var userInfo = GlobalContext.Current.UserInfo;
            if (userInfo == null)
            {
                //HttpCookie uidCookie = Request.Cookies.Get(FormsAuthentication.FormsCookieName);
                //if (uidCookie != null)
                //{
                //    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(uidCookie.Value);

                //    string cookieValue = System.Text.Encoding.Default.GetString(Convert.FromBase64String(Request.Cookies["uid_cookie"].Value));
                //    var arr = cookieValue.Split(':');
                //    var loginName = arr[0];
                //    var loginPwd = arr[1];

                //    var tempModel = new AccountBiz().GetByLogin(loginName, loginPwd);
                //    if (tempModel != null)
                //    {
                //        GlobalContext.Current.UserInfo = tempModel;

                //        return;
                //    }
                //}

                // 没有session的场合
                string urls = "/Account/Login?returnUrl=" + HttpContext.Request.Url;
                filterContext.Result = new RedirectResult(urls);
            }
        }


        public ActionResult SaveResult(string result, string successUrl = "", string failJS = "")
        {
            string script = "";
            switch (result)
            {
                case "1":
                    if (string.IsNullOrEmpty(successUrl))
                        script = "<script type=\"text/javascript\">alert('操作执行成功！');</script>";
                    else
                        script = "<script type=\"text/javascript\">window.location.href='" + successUrl + "';alert('操作执行成功！');</script>";
                    break;
                case "0":
                    script = "<script type=\"text/javascript\">alert('操作执行失败！');" + failJS + "</script>";
                    break;

            }

            return Content(script);
        }

        public ActionResult AlertResult(string msg)
        {
            return Content(string.Format("<script type=\"text/javascript\">alert('{0}');history.back(0);</script>", msg));
        }

        /// <summary>
        /// 初始化页面数据
        /// </summary>
        protected virtual void InitPage()
        {
        }

        //public ApplicationUser UserInfo
        //{
        //    get
        //    {
        //        return GlobalContext.Current.UserInfo;
        //    }
        //}


        private ILog _logger = null;
        /// <summary>
        /// 此处进行异常记录,记录到文本中
        /// 通过filterContext.Exception来获取这个异常
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            // 执行基类中的OnException
            base.OnException(filterContext);
            if (filterContext.Exception != null)
            {
                _logger = LogManager.GetLogger(filterContext.Controller.GetType());

                string error = string.Format("类名：{0}  \r\n  错误信息：{1} \r\n  {2}",
                     filterContext.Controller.GetType(),
                                   filterContext.Exception.Message,
                                   filterContext.Exception.StackTrace)
                ;
                _logger.Error(error);

                // Response.Redirect("/Shared/Error");
            }

        }

    }
}
