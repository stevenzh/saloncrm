using SalonCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SalonCRM.Web.MVC
{
    public static partial class HtmlHelperEx
    {
        public static MvcHtmlString CheckboxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> values, string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var temp in values)
            {
                sb.Append("<label style=\"margin-right:15px; width:100px;\"> <input type=\"checkbox\" disabled=\"disabled\" alt=\"" + temp.Text + "\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + temp.Value + "\"");
                if (null != value && value.Split(',').Contains(temp.Value))
                {
                    sb.Append(" checked=\"checked\"");
                }
                sb.Append(" />" + temp.Text + "</label>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static List<SelectListItem> ToSelectListFor<T>(this IEnumerable<T> enumerable, Func<T, string> value, Func<T, string> text)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f)
            }).ToList();
            return items;
        }


        /// <summary>
        ///  菜单生成
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString MenuBuild(this HtmlHelper htmlHelper, string Nav, string NavNext)
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
    <ul id='pm" + parentFunc.MenuId + @"-menu' class='pm" + parentFunc.MenuId + @"-menu nav nav-list collapse in'>
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


            return MvcHtmlString.Create(sb.ToString());
        }
    }
}