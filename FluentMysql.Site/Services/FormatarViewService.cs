using FluentMysql.Infrastructure;
using FluentMysql.Site.Web.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FluentMysql.Site.Services
{
    public static class FormatarViewService
    {
        public static ActionResult Xml(ActionExecutedContext filterContext)
        {
            return Xml(filterContext, null, null);
        }

        public static ActionResult Xml(ActionExecutedContext filterContext, string[] viewData)
        {
            return Xml(filterContext, viewData, null);
        }

        public static ActionResult Xml(ActionExecutedContext filterContext, string[] viewData, string[] tempData)
        {
            ControllerBase controller = filterContext.Controller;
                
            object xRef;
            XDocument xDoc = new XDocument(new XElement("Sistema"));
            XElement xElem;

            if (object.Equals(tempData, null))
                tempData = new string[] { };

            if (object.Equals(viewData, null))
                viewData = new string[] { "Model" };
            else if (!viewData.Contains("Model"))
                viewData = viewData.Concat(new List<string>() { "Model" }).ToArray();

            // Temp Data
            foreach (string item in viewData)
            {
                xElem = new XElement(item);
                xRef = string.Empty;

                if (!object.Equals(controller, null) && !object.Equals(controller.ViewData, null))
                {
                    if (item.Equals("Model"))
                        xRef = controller.ViewData.Model;
                    else
                        xRef = controller.ViewData[item];
                }
                
                if (!(xRef is string) && (xRef is IList || xRef is IEnumerable))
                {
                    foreach (var subItem in (IEnumerable)xRef)
                    {
                        xElem.Add(ObjectUtility.PropertyToXElement(subItem, new XElement("Item")));
                    }
                }
                else
                {
                    xElem = ObjectUtility.PropertyToXElement(xRef, xElem);
                }
                xDoc.Element("Sistema").Add(xElem);
            }

            // TempData
            foreach (string item in tempData)
            {
                xElem = new XElement(item);
                xRef = string.Empty;

                if (!object.Equals(controller, null) && !object.Equals(controller.TempData, null))
                {
                    xRef = controller.TempData[item];
                }

                if (!(xRef is string) && (xRef is IList || xRef is IEnumerable))
                {
                    foreach (var subItem in (IEnumerable)xRef)
                    {
                        xElem.Add(ObjectUtility.PropertyToXElement(subItem, new XElement("Item")));
                    }
                }
                else
                {
                    xElem = ObjectUtility.PropertyToXElement(xRef, xElem);
                }
                xDoc.Element("Sistema").Add(xElem);
            }

            return new XmlResult() { XmlContent = xDoc.ToString() };
        }

        public static ActionResult Json(ActionExecutedContext filterContext)
        {
            return Json(filterContext, null, null);
        }

        public static ActionResult Json(ActionExecutedContext filterContext, string[] viewData)
        {
            return Json(filterContext, viewData, null);
        }

        public static ActionResult Json(ActionExecutedContext filterContext, string[] viewData, string[] tempData)
        {
            ControllerBase controller = filterContext.Controller;

            object jRef;
            IDictionary<string, object> jElm = new Dictionary<string, object>();
            IList<object> jList;
            
            if (object.Equals(tempData, null))
                tempData = new string[] { };

            if (object.Equals(viewData, null))
                viewData = new string[] { "Model" };
            else if (!viewData.Contains("Model"))
                viewData = viewData.Concat(new List<string>() { "Model" }).ToArray();

            // ViewData
            foreach (string item in viewData)
            {
                jRef = string.Empty;
                if (!object.Equals(controller, null) && !object.Equals(controller.ViewData, null))
                {
                    if (item.Equals("Model"))
                        jRef = controller.ViewData.Model;
                    else
                        jRef = controller.ViewData[item];
                }

                if (!(jRef is string) && (jRef is IList || jRef is IEnumerable))
                {
                    jList = new List<object>();
                    foreach (var subItem in (IEnumerable)jRef)
                    {
                        jList.Add(ObjectUtility.PropertyToDictionary(subItem));
                    }
                    jElm.Add(item, jList);
                }
                else
                {
                    jElm.Add(item, ObjectUtility.PropertyToDictionary(jRef));
                }
            }

            // TempData
            foreach (string item in tempData)
            {
                jRef = string.Empty;
                if (!object.Equals(controller, null) && !object.Equals(controller.TempData, null))
                {
                    jRef = controller.TempData[item];
                }

                if (!(jRef is string) && (jRef is IList || jRef is IEnumerable))
                {
                    jList = new List<object>();
                    foreach (var subItem in (IEnumerable)jRef)
                    {
                        jList.Add(ObjectUtility.PropertyToDictionary(subItem));
                    }
                    jElm.Add(item, jList);
                }
                else
                {
                    jElm.Add(item, ObjectUtility.PropertyToDictionary(jRef));
                }
            }

            return new JsonResult() { ContentEncoding = Encoding.UTF8, Data = jElm, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
