using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Common.Logging;
using SalonCRM.Cache;

namespace SalonCRM.Mvc
{
    public class TenantRouteConstraint : IRouteConstraint
    {
        ILog logger = LogManager.GetLogger("TenantRouteConstraint");

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var host = Consts.GetTestHost(httpContext.Request.Url.Host);
            var bag = CacheContext.Current.Get(Consts.HostCode + host);

            if (!values.ContainsKey("tenant"))
            {
                values.Add("tenant", bag);
            }

            return true;
        }
    }
}