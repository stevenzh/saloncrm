using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using SalonCRM.Identity;
using SalonCRM.Web;
using System.Text;
using SalonCRM.Models;

namespace SalonCRM.Views.Shared
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected string MenuString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 调用验证方法
                var userInfo = GlobalContext.Current.UserInfo;
                if (userInfo == null)
                {
                    Response.Redirect("/Account/Login?url=" + Request.Url);
                }
            }

            MenuString = MenuBuild(GlobalContext.Current.SiteNav, GlobalContext.Current.SiteNavNext);
        }


        public String MenuBuild(string Nav, string NavNext)
        {
            StringBuilder sb = new StringBuilder();

            IList<MenuViewModel> models = GlobalContext.Current.LoginUserFunctions;
            if (models != null)
            {
                // 查找所有父类的功能。
                var parentFuncs = models.Where(a => a.Level == 1).AsEnumerable();
                if (!string.IsNullOrEmpty(Nav))
                    parentFuncs = parentFuncs.Where(t => t.SiteNav == Nav);
                if (!string.IsNullOrEmpty(NavNext))
                    parentFuncs = parentFuncs.Where(t => t.SiteNavNext == NavNext);
                parentFuncs = parentFuncs.OrderBy(a => a.SortOrder);
                foreach (MenuViewModel parentFunc in parentFuncs)
                {
                    var childFuncs = models.Where(a => a.ParentId == parentFunc.MenuId && a.Level == 2).AsEnumerable();
                    if (!string.IsNullOrEmpty(Nav))
                        childFuncs = childFuncs.Where(t => t.SiteNav == Nav);
                    if (!string.IsNullOrEmpty(NavNext))
                        childFuncs = childFuncs.Where(t => t.SiteNavNext == NavNext);
                    childFuncs = childFuncs.OrderBy(a => a.SortOrder);
                    // 如果没有子菜单的场合
                    if (childFuncs.Count() < 1)
                    {
                        string icon = string.IsNullOrEmpty(parentFunc.Icon) ? "fa-link" : parentFunc.Icon;
                        sb.Append("<li><a href=\"" + parentFunc.MenuPath + "\" class=\"nav-header\"><i class=\"fa fa-fw " + icon + "\"></i>" + parentFunc.Name + "</a></li>");
                    }
                    else
                    {
                        string icon = string.IsNullOrEmpty(parentFunc.Icon) ? "fa-folder" : parentFunc.Icon;
                        sb.Append(@"<li>
    <a href='#' data-target='.pm" + parentFunc.MenuId + @"-menu' class='nav-header collapsed' data-toggle='collapse'>
            <i class='fa fa-fw " + icon + "'></i>" + parentFunc.Name + @"<i class='fa fa-collapse'></i>
    </a>
</li>
<li>
    <ul id='pm" + parentFunc.MenuId + @"-menu' class='pm" + parentFunc.MenuId + @"-menu nav nav-list collapse'>
");
                    }

                    foreach (MenuViewModel childFunc in childFuncs)
                    {
                        sb.Append("<li> ");
                        sb.Append("<a href=\"" + childFunc.MenuPath + "\" >");
                        sb.Append("<span class=\"fa fa-caret-right\"></span>" + childFunc.Name);
                        sb.Append("</a> ");
                        sb.Append(@"</li>
");
                    }
                    if (childFuncs.Count() > 0)
                    {
                        sb.Append(@"</ul>
");
                        sb.Append(@"</li>
");
                    }
                }
            }


            return sb.ToString();
        }
    }
}