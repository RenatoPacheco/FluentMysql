using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site.Areas.Admin.Controllers
{
    [AuthorizeUser(Nivel = Nivel.Visitante)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}