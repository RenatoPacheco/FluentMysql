using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site.Areas.Admin.Controllers
{

    [AuthorizeUser(Nivel = new Nivel[] { Nivel.Operador })]
    public class SobreController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}