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
                    var usuario = MinhaContaService.Login(dados);
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
        public ActionResult Recupera()
        {
            MinhaContaService.Logout();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Recupera(RecuperaForm dados)
        {
            MinhaContaService.Logout();
            if (ModelState.IsValid)
            {
                var usuario = MinhaContaService.Recupera(dados);
                if (!object.Equals(usuario, null))
                {
                    TempData["Mensagem"] = AlertsMessages.Success("Um link para rediginir sua senha foi enviado para seu e-mail");
                    return RedirectToAction("Recupera");
                }
                ViewBag.Mensagem += AlertsMessages.Warning("Seus dados de acesso não foram encontrados");
            }
            return View(dados);
        }
    }
}