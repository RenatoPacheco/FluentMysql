using AutoMapper;
using FluentMysql.Domain.Services;
using FluentMysql.Domain.ValueObject;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Models.Services;
using FluentMysql.Site.Areas.Admin.ViewsData.Usuario;
using FluentMysql.Site.Filters;
using FluentMysql.Site.Helpers;
using FluentMysql.Site.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site.Areas.Admin.Controllers
{
    [AuthorizeUser(Nivel = new Nivel[] { Nivel.Administrador })]
    public class UsuarioController : Controller
    {
        public ActionResult Index(FiltroForm filtro = null, bool voltar = false)
        {
            IList<Usuario> lista = new List<Usuario>();
            CommonRouteData routeData = new CommonRouteData(ControllerContext);
            string sessionRef = string.Format("{0}-{1}-{2}", routeData.Action, routeData.Controller, routeData.Area);
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (filtro.Acao == "ativar")
                    {
                        Models.Services.UsuarioService.Ativar(filtro.Selecionados, (Usuario)ViewBag.MinhaConta);
                        ViewBag.Mensagem += AlertsMessages.Success("Registro(s) ativado(s) com sucesso");
                        filtro.Selecionados = null;
                    }
                    else if (filtro.Acao == "desativar")
                    {
                        Models.Services.UsuarioService.Desativar(filtro.Selecionados, (Usuario)ViewBag.MinhaConta);
                        ViewBag.Mensagem += AlertsMessages.Success("Registro(s) desativado(s) com sucesso");
                        filtro.Selecionados = null;
                    }
                    else if (filtro.Acao == "remover")
                    {
                        Models.Services.UsuarioService.Excluir(filtro.Selecionados, (Usuario)ViewBag.MinhaConta);
                        ViewBag.Mensagem += AlertsMessages.Success("Registro(s) excluído(s) com sucesso");
                        filtro.Selecionados = null;
                    }
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
                catch (ArgumentException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }

                if (voltar && !object.Equals(Session[sessionRef], null))
                    filtro = (FiltroForm)Session[sessionRef];

                Session[sessionRef] = filtro;
                lista = Models.Services.UsuarioService.Filtrar(filtro);
            }

            ViewBag.Lista = lista;
            return View(filtro);
        }

        [HttpGet]
        public ActionResult Insere()
        {
            return View(new InsereForm());
        }
        
        [HttpPost]
        public ActionResult Insere(InsereForm dados)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Usuario info = Models.Services.UsuarioService.Inserir(dados, (Usuario)ViewBag.MinhaConta);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro inserido com sucesso");
                    try
                    {
                        Models.Services.MinhaContaService.SolicitarAutenticacao(info);
                        TempData["Mensagem"] += AlertsMessages.Success("E-mail de autenticação enviado com sucesso");
                    }
                    catch (Exception ex)
                    {
                        TempData["Mensagem"] += AlertsMessages.Danger(string.Format("<p>Ocorreu um erro ao enviar o e-mail de autenticação: <strong>{0}</strong><p><pre>{1}</pre>", ex.Message.ToString(), ex.StackTrace.ToString()), "div");
                    }
                    
                    return RedirectToAction("Altera", new { @Id = info.Id });
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
                catch (ArgumentException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
            }

            return View(dados);
        }

        [HttpGet]
        public ActionResult Altera(long id = 0)
        {
            Usuario info = Models.Services.UsuarioService.Info(id);

            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            AlteraForm dados = Mapper.Map<Usuario, AlteraForm>(info);
            PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, info);
            
            ViewBag.Info = info;
            return View(dados);
        }
        
        [HttpPost]
        public ActionResult Altera(AlteraForm dados)
        {
            Usuario info = Models.Services.UsuarioService.Info(dados.Id);
            
            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, info);

            if (ModelState.IsValid)
            {
                try
                {
                    var Usuario = Models.Services.UsuarioService.Alterar(dados, (Usuario)ViewBag.MinhaConta);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro alterado com sucesso");
                    return RedirectToAction("Altera", new { @Id = Usuario.Id });
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
                catch (ArgumentException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
            }

            ViewBag.Info = info;
            return View(dados);
        }

        [HttpGet]
        public ActionResult Info(long id = 0)
        {
            Usuario info = Models.Services.UsuarioService.Info(id);
            
            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            return View(info);
        }
        
        [HttpGet]
        public ActionResult SolicitaAutenticacao(long id = 0)
        {
            try
            {
                Usuario usuario = Models.Services.UsuarioService.Info(id);

                if (object.Equals(usuario, null) || usuario.Id <= 0)
                    throw new HttpException(404, "O registro solicitado não foi encontrado");
                
                Models.Services.MinhaContaService.SolicitarAutenticacao(usuario);                
                TempData["Mensagem"] = AlertsMessages.Success("E-mail solicitando autenticação enviado com sucesso");
            }
            catch (ValidationException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(string.Format("Atenção: <strong>{0}</strong>", ex.Message.ToString()));
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = AlertsMessages.Danger(string.Format("<p>Ocorreu um erro ao enviar o e-mail de autenticação: <strong>{0}</strong><p><pre>{1}</pre>", ex.Message.ToString(), ex.StackTrace.ToString()), "div");
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }
        
        public ActionResult Ativa(IList<long> id)
        {            
            try
            {
                IList<Usuario> usuarios = Models.Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, usuarios, true);

                Models.Services.UsuarioService.Ativar(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) ativado(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (HttpException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        public ActionResult Desativa(IList<long> id)
        {
            try
            {
                IList<Usuario> usuarios = Models.Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, usuarios, true);

                Models.Services.UsuarioService.Desativar(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) desativado(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (HttpException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        public ActionResult Exclue(IList<long> id)
        {
            try
            {
                IList<Usuario> usuarios = Models.Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, usuarios, true);

                Models.Services.UsuarioService.Excluir(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) excluído(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (HttpException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }
    }
}