using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Models.Services;
using FluentMysql.Site.Areas.Admin.ViewsData.MinhaConta;
using FluentMysql.Site.Filters;
using FluentMysql.Site.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FluentMysql.Site.Areas.Admin.Controllers
{
    [AuthorizeUser(Nivel = new Nivel[] { Nivel.Visitante })]
    public class MinhaContaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            MinhaContaService.Logout();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginForm dados)
        {
            MinhaContaService.Logout();
            if (ModelState.IsValid)
            {
                try
                {
                    Usuario usuario = MinhaContaService.Logar(dados);
                    if (Request.QueryString["ReturnUrl"] != null)
                    {
                        return Redirect(Request.QueryString["ReturnUrl"]);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
            }
            return View(dados);
        }

        [AllowAnonymous]
        public ActionResult Sair()
        {
            MinhaContaService.Logout();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RecuperaAcesso()
        {
            MinhaContaService.Logout();
            return View(new RecuperaAcessoForm());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RecuperaAcesso(RecuperaAcessoForm dados)
        {
            MinhaContaService.Logout();
            if (ModelState.IsValid)
            {
                try
                {
                    Usuario usuario = MinhaContaService.RecuperarAcesso(dados);
                    if (object.Equals(usuario, null) || usuario.Id <= 0)
                        throw new ValidationException("Seus dados de acesso não foram encontrados");

                    TempData["Mensagem"] = AlertsMessages.Success(string.Format("Os dados solicitados foram enviado para <strong>{0}</strong>", usuario.Email));
                    return RedirectToAction("RecuperaAcesso");
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
                catch (ArgumentException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
                catch (Exception ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Danger(string.Format("<p>Ocorreu um erro ao enviar o e-mail da sua solicitação: <strong>{0}</strong><p><pre>{1}</pre>", ex.Message.ToString(), ex.StackTrace.ToString()), "div");
                }
            }
            return View(dados);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Autentica(string token)
        {
            MinhaContaService.Logout();
            Usuario usuario = MinhaContaService.ExtrairTokenAutenticacao(token);
            AutenticaForm dados = new AutenticaForm();

            dados.Nome = usuario.Nome;
            dados.Sobrenome = usuario.Sobrenome;
            dados.Token = token;

            return View("Autentica", "_LayoutClean", dados);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Autentica(AutenticaForm dados)
        {
            MinhaContaService.Logout();
            if (ModelState.IsValid)
            {
                try
                {
                    // Autenticando
                    Usuario usuario = MinhaContaService.AutenticarConta(dados);
                    TempData["Mensagem"] = AlertsMessages.Warning("Autenticação realizada com sucesso");
                    
                    // Logando e redirecionando para a home
                    usuario = MinhaContaService.Logar(new LoginForm() { LembarAcesso = true, Login = usuario.Email, Senha = usuario.Senha });
                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(string.Format("Atenção: <strong>{0}</strong>", ex.Message.ToString()));
                }
                catch (Exception ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Danger(string.Format("<p>Ocorreu um erro ao enviar o e-mail de autenticação: <strong>{0}</strong><p><pre>{1}</pre>", ex.Message.ToString(), ex.StackTrace.ToString()), "div");
                }
            }

            return View("Autentica", "_LayoutClean", dados);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RedefineSenha(string token)
        {
            MinhaContaService.Logout();
            Usuario usuario = MinhaContaService.ExtrairTokenRedefinirSenha(token);
            RedefineSenhaForm dados = new RedefineSenhaForm();

            dados.Token = token;
            
            ViewBag.MinhaConta = usuario;
            return View("RedefineSenha", "_LayoutClean", dados);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RedefineSenha(RedefineSenhaForm dados)
        {
            MinhaContaService.Logout();
            Usuario usuario = MinhaContaService.ExtrairTokenRedefinirSenha(dados.Token);
            if (ModelState.IsValid)
            {
                try
                {
                    // Autenticando
                    usuario = MinhaContaService.RedefinirSenha(dados);
                    TempData["Mensagem"] = AlertsMessages.Warning("Senha redefinida com sucesso");

                    // Logando e redirecionando para a home
                    usuario = MinhaContaService.Logar(new LoginForm() { LembarAcesso = true, Login = usuario.Email, Senha = usuario.Senha });
                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(string.Format("Atenção: <strong>{0}</strong>", ex.Message.ToString()));
                }
                catch (Exception ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Danger(string.Format("<p>Ocorreu um erro ao enviar o e-mail de autenticação: <strong>{0}</strong><p><pre>{1}</pre>", ex.Message.ToString(), ex.StackTrace.ToString()), "div");
                }
            }

            ViewBag.MinhaConta = usuario;
            return View("RedefineSenha", "_LayoutClean", dados);
        }
    }
}