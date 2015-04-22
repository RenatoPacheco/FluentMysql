using FluentMysql.Infrastructure;
using FluentMysql.Site.Areas.Admin.Services;
using FluentMysql.Site.DataAnnotations;
using FluentMysql.Site.Services;
using FluentMysql.Site.ValueObject;
using FluentMysql.Site.Web.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FluentMysql.Site.Filters
{
    class FormatarViewFilterAttribute : BaseFilterAttribute
    {
        public string Reference = "FormatoView";

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string formatoView = string.Empty;
            HttpRequestBase httpRequest = filterContext.HttpContext.Request;
            ViewResult viewResult = filterContext.Result as ViewResult;
            IFormatarViewAttribute[] attrs = (IFormatarViewAttribute[])filterContext.ActionDescriptor.GetCustomAttributes(typeof(IFormatarViewAttribute), false);

            if (string.IsNullOrWhiteSpace(formatoView = httpRequest.QueryString[Reference]))
                formatoView = httpRequest.Form[Reference];

            IFormatarViewAttribute attr = attrs.Where(x => x.PalavraChave == formatoView).FirstOrDefault();
            if (!object.Equals(attr, null))
            {
                if(!string.IsNullOrWhiteSpace(attr.MasterName))
                    viewResult.MasterName = attr.MasterName;

                if (!string.IsNullOrWhiteSpace(attr.ViewName))
                    viewResult.ViewName = attr.ViewName;

                if (attr.Tipo.Equals("json"))
                    filterContext.Result = FormatarViewService.Json(filterContext, attr.ViewData, attr.TempData);
                if (attr.Tipo.Equals("xml"))
                    filterContext.Result = FormatarViewService.Xml(filterContext, attr.ViewData, attr.TempData);
            }            
        }
    }
}
