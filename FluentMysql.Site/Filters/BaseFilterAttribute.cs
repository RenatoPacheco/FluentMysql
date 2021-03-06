﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site.Filters
{
    public abstract class BaseFilterAttribute : ActionFilterAttribute
    {
        public struct RouteData
        {
            public string Area;
            public string Controller;
            public string Action;

            public RouteData(ActionExecutingContext filterContext)
                : this(filterContext.HttpContext) { }

            public RouteData(HttpContextBase httpContext)
            {
                var RouteData = httpContext.Request.RequestContext.RouteData;

                this.Area = RouteData.DataTokens.Keys.Contains("area") ? RouteData.DataTokens["area"].ToString() : string.Empty;
                this.Controller = RouteData.Values.ContainsKey("controller") ? RouteData.GetRequiredString("controller") : string.Empty;
                this.Action = RouteData.Values.ContainsKey("action") ? RouteData.GetRequiredString("action") : string.Empty;
            }
        }

        public struct WebData
        {
            public RouteData Route;
            public HttpResponseBase Response;
            public HttpRequestBase Request;
            public HttpSessionStateBase Session;
            public ControllerBase Controller;

            public WebData(ActionExecutingContext filterContext)
            {
                this.Route = new RouteData(filterContext);
                this.Response = filterContext.HttpContext.Response;
                this.Request = filterContext.HttpContext.Request;
                this.Session = filterContext.HttpContext.Session;
                this.Controller = filterContext.Controller;
            }
        }

    }
}
