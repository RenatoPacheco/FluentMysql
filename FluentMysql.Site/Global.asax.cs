using FluentMysql.Site.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FluentMysql.Site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterAutoMappers();
        }

        protected void Application_Error()
        {
            Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~/Web.config");
            CustomErrorsSection customErrorsSection = (CustomErrorsSection)configuration.GetSection("system.web/customErrors");
            if (customErrorsSection.Mode == CustomErrorsMode.Off) return;

            var exception = Server.GetLastError();
            Server.ClearError();
            var httpException = exception as HttpException;

            //Logging goes here
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";

            if (httpException != null)
            {
                if (httpException.GetHttpCode() == 404)
                {
                    routeData.Values["action"] = "NotFound";
                }
                Response.StatusCode = httpException.GetHttpCode();
            }
            else
            {
                Response.StatusCode = 500;
            }

            // Avoid IIS7 getting involved
            Response.TrySkipIisCustomErrors = true;

            Response.ContentType = "text/html";

            //Executando o controller Error
            IController errorsController = new ErrorController();
            HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            var rc = new RequestContext(wrapper, routeData);
            errorsController.Execute(rc);
        }
    }
}
