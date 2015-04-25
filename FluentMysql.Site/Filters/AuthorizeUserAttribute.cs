using FluentMysql.Domain;
using FluentMysql.Domain.ValueObject;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Services;
using FluentMysql.Site.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace FluentMysql.Site.Filters
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        protected MinhaConta MinhaConta { get; set; }

        public Nivel Nivel { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            TokenAcesso token = (TokenAcesso)filterContext.HttpContext.User.Identity.Name;
            this.MinhaConta = MinhaConta.Factory((Usuario)token);

            if (object.Equals(this.MinhaConta, null)
                || this.MinhaConta.Acesso.Nivel.Equals(Nivel.Indefinido))
            {

                this.MinhaConta = MinhaConta.Factory(null);
                FormsAuthentication.SignOut();
            }

            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated
                && this.MinhaConta.Acesso.AutorizadoSobre(this.Nivel);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var routeData = new CommonRouteData(filterContext.HttpContext);

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        @Controller = "MinhaConta",
                        @Action = "Login",
                        @Area = routeData.Area,
                        @ReturnUrl = filterContext.HttpContext.Request.Url.ToString()
                    }));
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        @Controller = "MinhaConta",
                        @Action = "Acesso",
                        @Area = routeData.Area,
                        @ReturnUrl = filterContext.HttpContext.Request.Url.ToString()
                    }));
            }
        }
    }
}