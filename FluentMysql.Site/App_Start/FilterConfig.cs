using FluentMysql.Site.Filters;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InitFilterAttribute());
        }
    }
}
