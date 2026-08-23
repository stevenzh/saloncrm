using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SalonCRM.Web
{
    public class AdminPage : System.Web.UI.Page
    {

        private void Page_Load(object sender, System.EventArgs e)
        {

        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);

            if (!IsAuthenticated())
            {
                Response.Redirect("~/Account/Login?returnUrl=" + Server.UrlEncode(Page.AppRelativeVirtualPath + Request.Url.Query));
            }

            base.OnInit(e);
        }

        public static bool IsAuthenticated()
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                return false;
            if (GlobalContext.Current.UserHost != null)
                return true;

            return false;
        }

    }
}
