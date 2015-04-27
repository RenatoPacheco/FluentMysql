using FluentMysql.Domain;
using FluentMysql.Domain.ValueObject;
using FluentMysql.Infrastructure;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Services;
using FluentMysql.Site.ValueObject;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace FluentMysql.Site.Filters
{
    public class AuthorizeAboutUserAttribute : AuthorizeAttribute
    {
        public string Reference = "Id";

        public bool ProibirAutenticado = false;

        public bool ProibirMinhaConta = false;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                var routeData = ((MvcHandler)httpContext.Handler).RequestContext.RouteData;
                IList<Usuario> usuarios;
                string[] valueRequest = null;
                IList<long> ids = new List<long>();
                HttpRequestBase httpRequest = httpContext.Request;

                if ((valueRequest = httpRequest.QueryString.GetValues(Reference)) == null)
                    valueRequest = httpRequest.Form.GetValues(Reference);

                if (!object.Equals(valueRequest, null))
                    ids = valueRequest.Select(x => long.Parse(x)).ToList();

                if (routeData.Values.ContainsKey(Reference))
                    ids.Add(long.Parse(routeData.Values[Reference].ToString()));
                
                using (Connection connection = new Connection())
                {
                    using (ISession session = connection.Session)
                    {
                        usuarios = session.CreateQuery(@"FROM Usuario WHERE Id = :id")
                            .SetParameterList("id", ids.Distinct())
                            .SetMaxResults(1)
                            .List<Usuario>();
                    }
                }

                return usuarios.Where(x => 
                    !MinhaConta.Instance.Acesso.AutorizadoSobre(x)
                    && (!ProibirAutenticado
                    || (new ContaAcesso(x)).Autenticado)
                    && (!ProibirMinhaConta 
                    || MinhaConta.Instance.Info.Id == x.Id)).FirstOrDefault() == null;
            }
            catch
            {

            }
            
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary(new
            {
                @Controller = "MinhaConta",
                @Action = "Acesso",
                @Area = "Admin",
                @ReturnUrl = filterContext.HttpContext.Request.Url.ToString()
            }));
        }
    }
}