using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using SalonCRM.Models;

namespace SalonCRM.Web
{
    /// <summary>
    ///  上下文传递中需要用的数据集合类
    /// </summary>
    public class GlobalContext
    {
        private static GlobalContext _current;

        public static GlobalContext Current
        {
            get
            {
                if (_current == null)
                    _current = Activator.CreateInstance<GlobalContext>();
                return _current;
            }

        }

        /// <summary>
        ///  获取当前用户的信息
        /// </summary>
        public ApplicationUser UserInfo
        {
            get
            {
                return HttpContext.Current.Session["userInfo"] as ApplicationUser;
            }
            set { HttpContext.Current.Session["userInfo"] = value; }
        }

        public Host UserHost
        {
            get
            {
                return HttpContext.Current.Session["userHost"] as Host;
            }
            set { HttpContext.Current.Session["userHost"] = value; }
        }

        /// <summary>
        /// 当前登录用户的所在部门列表
        /// </summary>
        public Organ UserDepartment
        {
            get
            {
                return HttpContext.Current.Session["userDepartment"] as Organ;
            }
            set
            {
                HttpContext.Current.Session["userDepartment"] = value;
            }
        }

        /// <summary>
        ///  获取配置文件的信息
        /// </summary>
        public NameValueCollection Config { get; set; }


        /// <summary>
        /// 返回路径
        /// </summary>
        public string UrlReferrerSession
        {
            get
            {
                return HttpContext.Current.Session["UrlReferrerSession"] as string;
            }
            set { HttpContext.Current.Session["UrlReferrerSession"] = value; }
        }
        
        /// <summary>
        /// 系统三大类 SN1:运营 SN2:经营 SN3:财务
        /// </summary>
        public string SiteNav
        {
            get
            {
                return HttpContext.Current.Session["SiteNav"] as string;
            }
            set { HttpContext.Current.Session["SiteNav"] = value; }
        }
        public string SiteNavName
        {
            get
            {
                return HttpContext.Current.Session["SiteNavName"] as string;
            }
            set { HttpContext.Current.Session["SiteNavName"] = value; }
        }
        /// <summary>
        /// 二级分类 SNN1:客户类 SNN2:员工类  SNN3:项目类
        /// </summary>
        public string SiteNavNext
        {
            get
            {
                return HttpContext.Current.Session["SiteNavNext"] as string;
            }
            set { HttpContext.Current.Session["SiteNavNext"] = value; }
        }

        public IList<MenuViewModel> LoginUserFunctions {
            get
            {
                return HttpContext.Current.Session["userMenu"] as IList<MenuViewModel>;
            }
            set { HttpContext.Current.Session["userMenu"] = value; }
        }
    }


}
