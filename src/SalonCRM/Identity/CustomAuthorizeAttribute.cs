using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalonCRM.Web;
using SalonCRM.Models;
using SalonCRM.Manager;

namespace SalonCRM.Identity
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (GlobalContext.Current.UserInfo == null && CurrentUser != null)
            {
                ApplicationDbContext dbcontent = new ApplicationDbContext();

                ApplicationUser currentUser = dbcontent.Users.FirstOrDefault(x => x.Id == CurrentUser.UserId);
                GlobalContext.Current.UserInfo = currentUser;
                Organ department = dbcontent.Organs.FirstOrDefault(a => a.OrganID == currentUser.OrganId);
                GlobalContext.Current.UserDepartment = department;
                GlobalContext.Current.UserHost = department.Host;

                if (currentUser.IsAdminUser > 0)
                    GlobalContext.Current.LoginUserFunctions = UserManager.GetAllMenus();
                else
                    GlobalContext.Current.LoginUserFunctions = UserManager.GetMenus(currentUser.Id);
            }

            base.OnAuthorization(filterContext); //returns to login url
        }
    }
}