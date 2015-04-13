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
            dynamic viewBag = web.Controller.ViewBag;
            TempDataDictionary tempData = filterContext.Controller.TempData;
            string areaStr = web.Route.Area;
            string controllerStr = web.Route.Controller;
            string actionStr = web.Route.Action;
            
            viewBag.AreaRef = string.Format("{0}", areaStr);
            viewBag.ControllerRef = string.Format("{0}-{1}", areaStr, controllerStr);
            viewBag.ActionRef = string.Format("{0}-{1}-{2}", areaStr, controllerStr, actionStr);
            
            if (tempData.ContainsKey("AreaAtual"))
                tempData[tempData["AreaAtual"].ToString()] = string.Empty;

            if (tempData.ContainsKey("ControllerAtual"))
                tempData[tempData["ControllerAtual"].ToString()] = string.Empty;

            if (tempData.ContainsKey("ActionAtual"))
                tempData[tempData["ActionAtual"].ToString()] = string.Empty;

            tempData["AreaAtual"] = string.Format("Area-{0}",areaStr);
            tempData["ControllerAtual"] = string.Format("Controller-{0}",controllerStr);
            tempData["ActionAtual"] = string.Format("Action-{0}", actionStr);

            tempData[tempData["AreaAtual"].ToString()] = "active";
            tempData[tempData["ControllerAtual"].ToString()] = "active";
            tempData[tempData["ActionAtual"].ToString()] = "active";

            if (tempData["Mensagem"] == null)
                tempData["Mensagem"] = string.Empty;

            viewBag.Mensagem = tempData["Mensagem"];
        }
    }
}
