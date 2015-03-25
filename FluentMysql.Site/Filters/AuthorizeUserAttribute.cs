using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Models.Services;
using FluentMysql.Site.ObjectValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentMysql.Site.Filters
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        protected Usuario MinhaConta { get; set; }

        private Nivel[] _Nivel = new Nivel[] { };
        public Nivel[] Nivel
        {
            get { return _Nivel; }
            set { _Nivel = value != null ? value : new Nivel[] { }; }
        }

        public override void OnAuthorization( AuthorizationContext filterContext)
        {
            MinhaConta = MinhaContaService.ExtrairConta();
            if (object.Equals(MinhaConta, null))
                MinhaConta = new Usuario();

            filterContext.Controller.ViewBag.MinhaConta = MinhaConta;
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            int[] niveis = Nivel.Select(x => (int)x).ToArray();
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return base.AuthorizeCore(httpContext);
            }
            else if (Nivel.Length > 0)
            {
                return (int)MinhaConta.Nivel > 0 && niveis.Where(x => x >= (int)MinhaConta.Nivel).Count() > 0;
            }
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var routeData = new CommonRouteData(filterContext.HttpContext);
            
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { @Controller = "MinhaConta", @Action = "Login", @Area = routeData.Area, @ReturnUrl = filterContext.HttpContext.Request.Url.ToString() }));
            }
            else if (MinhaConta.Nivel > 0)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { @Controller = "MinhaConta", @Action = "Acesso", @Area = routeData.Area }));
            }
        }
    }
}