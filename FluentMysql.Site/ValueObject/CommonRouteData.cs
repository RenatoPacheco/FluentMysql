using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site.ValueObject
{
    public class CommonRouteData
    {
        public string Area { get; protected set; }
        public string Controller { get; protected set; }
        public string Action { get; protected set; }

        public CommonRouteData(ActionExecutingContext filterContext)
                : this(filterContext.HttpContext) { }

        public CommonRouteData(HttpContextBase httpContext)
            {
                var RouteData = httpContext.Request.RequestContext.RouteData;

                Area = RouteData.DataTokens.Keys.Contains("area") ? RouteData.DataTokens["area"].ToString() : string.Empty;
                Controller = RouteData.Values.ContainsKey("controller") ? RouteData.GetRequiredString("controller") : string.Empty;
                Action = RouteData.Values.ContainsKey("action") ? RouteData.GetRequiredString("action") : string.Empty;
            }
    }
}
