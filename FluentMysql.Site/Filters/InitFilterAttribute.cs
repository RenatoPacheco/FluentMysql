using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FluentMysql.Site.Filters
{
    class InitFilterAttribute : BaseFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            WebData web = new WebData(filterContext);

            web.Controller.ViewBag.AreaRef = string.Format("{0}", web.Route.Area);
            web.Controller.ViewBag.ControllerRef = string.Format("{0}-{1}", web.Route.Area, web.Route.Controller);
            web.Controller.ViewBag.AtionRef = string.Format("{0}-{1}-{2}", web.Route.Area, web.Route.Controller, web.Route.Action);

            filterContext.Controller.TempData["Area-" + web.Route.Area] = "active";
            filterContext.Controller.TempData["Controller-" + web.Route.Controller] = "active";
            filterContext.Controller.TempData["Action-" + web.Route.Action] = "active";

            web.Controller.ViewBag.Alerta = string.Empty;

            if (filterContext.Controller.TempData["Mensagem"] != null)
                filterContext.Controller.ViewBag.Mensagem = filterContext.Controller.TempData["Mensagem"];

        }
    }
}
