using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace SalonCRM.Identity
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role == r))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int HostId { get; set; }
        public string UserId { get; set; }
        public string UserCnName { get; set; }
        public string[] roles { get; set; }
        public int IsAdminUser { get; set; }
        /// <summary>
        /// 1: 美容师 2:账户 3：顾问 4：店长
        /// </summary>
        public string Type { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public int HostId { get; set; }
        public string UserId { get; set; }
        public string UserCnName { get; set; }
        public string[] roles { get; set; }
        public int IsAdminUser { get; set; }
        /// <summary>
        /// 2:账户  1: 美容师  3：顾问
        /// </summary>
        public string Type { get; set; }
    }
}