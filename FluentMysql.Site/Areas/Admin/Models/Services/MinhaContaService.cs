using FluentMysql.Domain.Repository;
using FluentMysql.Domain.Services;
using FluentMysql.Domain.ValueObject;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.Security;
using FluentMysql.Site.Areas.Admin.ViewsData.MinhaConta;
using FluentMysql.Site.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace FluentMysql.Site.Areas.Admin.Models.Services
{
    public static class MinhaContaService
    {
        public static void Logout()
        {
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = new HttpCookie("usuario");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Value = "0";

            context.Response.SetCookie(cookie);
            context.Session["usuario"] = "0";

            FormsAuthentication.SignOut();
        }

        public static Usuario Login(LoginForm dados)
        {
            HttpContext context = HttpContext.Current;

            Usuario resultado = null;

            resultado = AutenticacaoService.Logar(dados.Login, dados.Senha);

            if (object.Equals(resultado, null))
                throw new ValidationException("Login ou senha inválida");

            if (resultado.DataInicio != null && DateTime.Now.CompareTo((DateTime)resultado.DataInicio) < 0)
                throw new ValidationException("Seu usuário ainda não tem acesso ao sistema");

            if (resultado.DataTermino != null && DateTime.Now.CompareTo((DateTime)resultado.DataTermino) > 0)
                throw new ValidationException("Seu usuário não tem mais acesso ao sistema");

            FormsAuthentication.SetAuthCookie(resultado.Id.ToString(), false);
            context.Session["usuario"] = resultado.Id.ToString();
            
            if(dados.LembarAcesso)
            {
                HttpCookie cookie = new HttpCookie("usuario");
                cookie.Expires = DateTime.Now.AddDays(14);
                cookie.Value = resultado.Id.ToString();

                context.Response.SetCookie(cookie);
            }

            return resultado;
        }

        public static Usuario ExtrairConta()
        {
            Usuario resultado = null;
            long id = 0;
            HttpContext context = HttpContext.Current;

            if (context.User.Identity.IsAuthenticated)
                if (long.TryParse(context.User.Identity.Name, out id))

            if (id.Equals(0))
                if (!object.Equals(context.Session["usuario"], null))
                    if (!long.TryParse(context.Session["usuario"].ToString(), out id))
                        id = 0;

            if (id.Equals(0))
                if (!object.Equals(context.Request.Cookies["usuario"], null))
                    if (!long.TryParse(context.Request.Cookies["usuario"].ToString(), out id))
                        id = 0;

            using (var acao = new UsuarioRepository())
            {
                resultado = acao.Find(id);
            }

            return resultado;
        }

        public static Usuario Recupera(RecuperaForm dados)
        {
            Usuario resultado = null;

            resultado = AutenticacaoService.Recuperar(dados.Login);

            return resultado;
        }

        public static void SolicitarAutenticacao(Usuario usuario)
        {
            if (object.Equals(usuario, null))
                throw new ArgumentNullException("O valor não pode ser nulo", "usuario");

            UsuarioInfo info = new UsuarioInfo(usuario);

            if (info.Autenticado)
                throw new ValidationException("Este registro já foi autenticado");
            
            string token = Token.EncryptString(string.Format("{0}|{1}",usuario.Id, usuario.Email), DateTime.Now.AddDays(14));
            string mensagem = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Areas/Admin/Template/Email/SolicitarAutenticacaoUsuario.html"));
            
            mensagem = Regex.Replace(mensagem, "{nome}", usuario.Nome, RegexOptions.IgnoreCase);
            mensagem = Regex.Replace(mensagem, "{link}", string.Format("?token={0}",token), RegexOptions.IgnoreCase);

            EmailSimples.Enviar("Autenticar conta de acesso", mensagem, new List<string>() { usuario.Email });
        }
    }
}
