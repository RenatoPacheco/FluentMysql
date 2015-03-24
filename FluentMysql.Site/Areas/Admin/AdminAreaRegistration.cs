using System.Web.Mvc;

namespace FluentMysql.Site.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FluentMysql.Site.Areas.Admin.Controllers" }
            );

            AutoMapperConfig.RegisterAutoMappers();
        }
    }
}