using FluentMysql.Domain.Repository;
using FluentMysql.Domain.Services;
using FluentMysql.Domain.ValueObject;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.Security;
using FluentMysql.Infrastructure.Web;
using FluentMysql.Site.Areas.Admin.Template.Email;
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

        public static Usuario Logar(LoginForm dados)
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

            using (UsuarioRepository acao = new UsuarioRepository())
            {
                resultado = acao.Find(id);
            }

            return resultado;
        }

        public static Usuario RecuperarAcesso(RecuperaAcessoForm dados)
        {
            Usuario resultado = null;

            resultado = UsuarioService.Info(dados.Identificacao);
            if (!object.Equals(resultado, null))
            {
                if (dados.Acao.Equals("redefinir-senha"))
                {
                    MinhaContaService.SolicitarRedefinirSenha(resultado);
                }
                else if (dados.Acao.Equals("solicitar-login"))
                {
                    MinhaContaService.EnviarLoginPorEmail(resultado);
                }
                else
                {
                    throw new ValidationException("Nenhuma ação válida foi solicitada");
                }
            }

            return resultado;
        }

        public static void SolicitarAutenticar(Usuario usuario)
        {
            if (object.Equals(usuario, null))
                throw new ArgumentNullException("usuario", "O valor não pode ser nulo");

            UsuarioInfo info = new UsuarioInfo(usuario);

            if (info.Autenticado)
                throw new ValidationException("Este registro já foi autenticado");
            
            string token = Token.EncryptString(string.Format("{0}|{1}",usuario.Id, usuario.Email), DateTime.Now.AddDays(14));
            string link = string.Format("{0}?token={1}", UriUtility.ToAbsoluteUrl("~/Admin/MinhaConta/Autentica/"), HttpUtility.UrlEncode(token));
            
            AutenticarUsuarioTemp mensagem = new AutenticarUsuarioTemp(usuario.Nome, link, "Sistema");
            EmailSimples.Enviar("Autenticar conta de acesso", mensagem.ToString(), new List<string>() { usuario.Email });
        }

        internal static Usuario Autenticar(AutenticaForm dados)
        {
            Usuario resultado = MinhaContaService.ExtrairTokenAutenticar(dados.Token);
            resultado.Nome = dados.Nome;
            resultado.Sobrenome = dados.Sobrenome;
            resultado.Login = dados.Login;
            resultado.CPF = dados.CPF;
            resultado.Senha = dados.Senha;
            resultado.DataAlteracao = DateTime.Now;
            resultado.Responsavel = resultado;
            
            resultado = Domain.Services.UsuarioService.AlterarUnico(resultado);

            return resultado;
        }

        internal static Usuario ExtrairTokenAutenticar(string token)
        {
            Usuario resultado = new Usuario();
            string[] extrair;
            long id;
            string email;
            try
            {
                extrair = Token.DecryptString(token).Split('|');
                id = long.Parse(extrair[0]);
                email = extrair[1].Trim();
            }
            catch
            {
                throw new ValidationException("O token informado expirou ou não é válido");
            }

            resultado = UsuarioService.Info(id);

            if (object.Equals(resultado, null) || resultado.Id <= 0 || resultado.Email != email)
                throw new ValidationException("O token informado expirou ou não é válido");

            UsuarioInfo info = new UsuarioInfo(resultado);
            if (info.Autenticado)
                throw new ValidationException("Este registro já foi autenticado");


            return resultado;
        }

        public static void SolicitarRedefinirSenha(Usuario usuario)
        {
            if (object.Equals(usuario, null) || usuario.Id <= 0)
                throw new ArgumentNullException("usuario", "O valor não pode ser nulo");

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                throw new ArgumentNullException("nome", "O valor não pode ser nulo ou vazio");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new ArgumentNullException("email", "O valor não pode ser nulo ou vazio");
            
            string token = Token.EncryptString(string.Format("{0}|{1}", usuario.Id, usuario.Email), DateTime.Now.AddDays(1));
            string link = string.Format("{0}?token={1}", UriUtility.ToAbsoluteUrl("~/Admin/MinhaConta/RedefineSenha/"), HttpUtility.UrlEncode(token));

            RedefinirSenhaTemp mensagem = new RedefinirSenhaTemp(usuario.Nome, link, "Sistema");
            EmailSimples.Enviar("Redefinir senha", mensagem.ToString(), new List<string>() { usuario.Email });
        }

        internal static Usuario RedefinirSenha(RedefineSenhaForm dados)
        {
            if (object.Equals(dados, null))
                throw new ArgumentNullException("dados", "O valor não pode ser nulo");

            Usuario resultado = MinhaContaService.ExtrairTokenRedefinirSenha(dados.Token);
            resultado.Senha = dados.NovaSenha;
            resultado.DataAlteracao = DateTime.Now;
            resultado.Responsavel = resultado;

            using (UsuarioRepository acao = new UsuarioRepository())
            {
                acao.Edit(resultado);
            }

            return resultado;
        }

        internal static Usuario ExtrairTokenRedefinirSenha(string token)
        {
            Usuario resultado = new Usuario();
            string[] extrair;
            long id;
            string email;
            try
            {
                extrair = Token.DecryptString(token).Split('|');
                id = long.Parse(extrair[0]);
                email = extrair[1].Trim();
            }
            catch
            {
                throw new ValidationException("O token informado expirou ou não é válido");
            }

            resultado = UsuarioService.Info(id);

            if (object.Equals(resultado, null) || resultado.Id <= 0 || resultado.Email != email)
                throw new ValidationException("O token informado expirou ou não é válido");
            
            return resultado;
        }
        
        public static void EnviarLoginPorEmail(Usuario usuario)
        {
            if (object.Equals(usuario, null) || usuario.Id <= 0)
                throw new ArgumentNullException("usuario", "O valor não pode ser nulo");

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                throw new ArgumentNullException("nome", "O valor não pode ser nulo ou vazio");

            if (string.IsNullOrWhiteSpace(usuario.Login))
                throw new ArgumentNullException("login", "O valor não pode ser nulo ou vazio");

            EnviarLoginTemp mensagem = new EnviarLoginTemp(usuario.Nome, usuario.Login, "Sistema");
            EmailSimples.Enviar("Login de acesso", mensagem.ToString(), new List<string>() { usuario.Email });
        }

        public static Usuario AlterarDados(AlterarDadosForm dados)
        {
            bool requerSenha = !string.IsNullOrWhiteSpace(dados.NovaSenha)
                || !string.IsNullOrWhiteSpace(dados.NovoEmail);

            if (object.Equals(dados, null))
                throw new ArgumentNullException("dados", "O valor não pode ser nulo");

            Usuario resultado = requerSenha 
                ? UsuarioService.Info(dados.Id, dados.SenhaAtual) 
                : UsuarioService.Info(dados.Id);

            if (object.Equals(resultado, null) && requerSenha)
                throw new ValidationException("Registro não foi encontrado para ser alterado, verifique sua senha de autenticação");
            else if (object.Equals(resultado, null))
                throw new ValidationException("Registro não foi encontrado para ser alterado");
            
            resultado.Nome = dados.Nome;
            resultado.Sobrenome = dados.Sobrenome;
            resultado.Login = dados.Login;
            resultado.CPF = dados.CPF;
            resultado.DataAlteracao = DateTime.Now;
            resultado.Responsavel = resultado;

            if (!string.IsNullOrWhiteSpace(dados.NovaSenha))
                resultado.Senha = dados.NovaSenha;
            
            if (!string.IsNullOrWhiteSpace(dados.NovoEmail) && !resultado.Email.Equals(dados.NovoEmail))
                MinhaContaService.SolicitarAutenticarEmail(resultado, dados.NovoEmail);
            
            resultado = FluentMysql.Domain.Services.UsuarioService.AlterarUnico(resultado);

            return resultado;
        }
        
        public static void SolicitarAutenticarEmail(Usuario usuario, string novoEmail)
        {
            if (object.Equals(usuario, null) || usuario.Id <= 0)
                throw new ArgumentNullException("usuario", "O valor não pode ser nulo");

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                throw new ArgumentNullException("nome", "O valor não pode ser nulo ou vazio");

            if (string.IsNullOrWhiteSpace(novoEmail))
                throw new ArgumentNullException("novoEmail", "O valor não pode ser nulo ou vazio");

            string token = Token.EncryptString(string.Format("{0}|{1}", usuario.Id, novoEmail), DateTime.Now.AddDays(6));
            string link = string.Format("{0}?token={1}", UriUtility.ToAbsoluteUrl("~/Admin/MinhaConta/AutenticaEmail/"), HttpUtility.UrlEncode(token));

            AutenticarEmailTemp mensagem = new AutenticarEmailTemp(usuario.Nome, link, "Sistema");
            EmailSimples.Enviar("Autenticar E-mail", mensagem.ToString(), new List<string>() { novoEmail });
        }

        internal static Usuario ExtrairTokenAutenticarEmail(string token)
        {
            Usuario resultado = new Usuario();
            string[] extrair;
            long id;
            string email;
            try
            {
                extrair = Token.DecryptString(token).Split('|');
                id = long.Parse(extrair[0]);
                email = extrair[1].Trim();
            }
            catch
            {
                throw new ValidationException("O token informado expirou ou não é válido");
            }

            resultado = UsuarioService.Info(id);

            if (object.Equals(resultado, null) || resultado.Id <= 0)
                throw new ValidationException("O token informado expirou ou não é válido");

            resultado.Email = email;

            return resultado;
        }

        internal static Usuario AutenticarEmail(AutenticaEmailForm dados)
        {
            Usuario resultado = MinhaContaService.ExtrairTokenAutenticarEmail(dados.Token);
            resultado.DataAlteracao = DateTime.Now;
            resultado.Responsavel = resultado;

            if (!resultado.Senha.Equals(dados.Senha))
                throw new ValidationException("Senha de confirmação não é válida");

            if (!resultado.Email.Equals(dados.NovoEmail))
                throw new ValidationException("O e-mail envido não é válido para esse token");

            resultado = Domain.Services.UsuarioService.AlterarUnico(resultado);

            return resultado;
        }
    }
}
