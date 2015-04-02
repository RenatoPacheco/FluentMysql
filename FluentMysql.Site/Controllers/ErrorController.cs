using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index()
        {
            Exception exception = null;
            if (HttpContext != null
                && HttpContext.AllErrors != null
                && HttpContext.AllErrors.Count() > 0)
                exception = HttpContext.AllErrors[0];

            ViewBag.Exception = exception;

            return View();
        }
        public ActionResult NotFound()
        {
            Exception exception = null;
            if (HttpContext != null
                && HttpContext.AllErrors != null
                && HttpContext.AllErrors.Count() > 0)
                exception = HttpContext.AllErrors[0];

            ViewBag.Exception = exception;

            return View();
        }
    }
}