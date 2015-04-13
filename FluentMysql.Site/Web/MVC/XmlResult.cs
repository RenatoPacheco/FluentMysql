using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using FluentMysql.Infrastructure.Xml;
using FluentMysql.Infrastructure.Xml.Linq;
using System.Text.RegularExpressions;

namespace FluentMysql.Site.Web.Mvc
{
    internal class XmlResult : ActionResult
    {
        public string FileName { get; set; }

        public string XmlContent { get; set; }
        
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Charset = "utf-8";
            context.HttpContext.Response.AddHeader("Expires", "Mon, 26 Jul 2017 05:00:00 GMT");
            context.HttpContext.Response.AddHeader("Content-type", "text/xml");

            if (!string.IsNullOrWhiteSpace(FileName))
                context.HttpContext.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", FileName));

            context.HttpContext.Response.Write(XmlContent);
            context.HttpContext.Response.End();
        }
    }
}
