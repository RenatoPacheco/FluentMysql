using AutoMapper;
using FluentMysql.Domain;
using FluentMysql.Domain.Services;
using FluentMysql.Domain.ValueObject;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Services;
using FluentMysql.Site.Areas.Admin.ViewsData.Usuario;
using FluentMysql.Site.DataAnnotations;
using FluentMysql.Site.Filters;
using FluentMysql.Site.Helpers;
using FluentMysql.Site.Services;
using FluentMysql.Site.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site.Areas.Admin.Controllers
{
    [AuthorizeUser(Nivel = Nivel.Administrador)]
    public class UsuarioController : Controller
    {
        [FormatarViewFilter()]
        [FormatarViewXml("xml", ViewData = new string[] { "Mensagem", "Lista" })]
        [FormatarViewJson("json", ViewData = new string[] { "Mensagem", "Lista" })]
        [FormatarViewHtml("html", "_IndexTBody", "_LayoutEmpty")]
        public ActionResult Index(bool voltar = false, FiltroForm filtro = null)
        {
            int index = -1;
            string ids;
            IList<Usuario> lista = new List<Usuario>();

            if (ModelState.IsValid)
            {
                try
                {
                    if (int.TryParse(filtro.AlteraNivelIndex, out index))
                    {
                        return RedirectToAction("AlteraNivel", new { @Id = filtro.AlteraUsuarioId[index], @Nivel = filtro.AlteraNivelValor[index] });
                    }

                    if (!filtro.Acao.Equals(AcaoView.Default))
                    {
                        if (filtro.Acao.Equals(AcaoView.Ativar))
                        {
                            ids = string.Join("&", filtro.Selecionados.Select(x => string.Format("id={0}", x)));
                            return Redirect(string.Format("Usuario/Ativa/?{0}", ids));
                        }
                        else if (filtro.Acao.Equals(AcaoView.Desativar))
                        {
                            ids = string.Join("&", filtro.Selecionados.Select(x => string.Format("id={0}", x)));
                            return Redirect(string.Format("Usuario/Desativa/?{0}", ids));
                        }
                        else if (filtro.Acao.Equals(AcaoView.Excluir))
                        {
                            ids = string.Join("&", filtro.Selecionados.Select(x => string.Format("id={0}", x)));
                            return Redirect(string.Format("Usuario/Exclue/?{0}", ids));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ValidationException || ex is ArgumentException)
                        ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                    else
                        throw ex;
                }

                if (voltar && !object.Equals(Session[ViewBag.ActionRef], null))
                    filtro = (FiltroForm)Session[ViewBag.ActionRef];

                Session[ViewBag.ActionRef] = filtro;
                lista = Services.UsuarioService.Filtrar(ref filtro);
            }
            
            ViewData["Lista"] = lista;
            return View(filtro);
        }

        [HttpGet]
        [FormatarViewFilter]
        [FormatarViewXml("xml", ViewData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", ViewData = new string[] { "Mensagem" })]
        [FormatarViewHtml("html", "_InsereForm", "_LayoutEmpty")]
        public ActionResult Insere()
        {
            return View(new InsereForm());
        }

        [HttpPost]
        [FormatarViewFilter]
        [FormatarViewXml("xml", ViewData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", ViewData = new string[] { "Mensagem" })]
        [FormatarViewHtml("html", "_InsereForm", "_LayoutEmpty")]
        public ActionResult Insere(InsereForm dados = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Usuario info = Services.UsuarioService.Inserir(dados);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro inserido com sucesso");
                    try
                    {
                        Services.MinhaContaService.SolicitarAutenticar(info);
                        TempData["Mensagem"] += AlertsMessages.Success("E-mail de autenticação enviado com sucesso");
                    }
                    catch (Exception ex)
                    {
                        TempData["Mensagem"] += AlertsMessages.Danger(string.Format("<p>Ocorreu um erro ao enviar o e-mail de autenticação: <strong>{0}</strong><p><pre>{1}</pre>", ex.Message.ToString(), ex.StackTrace.ToString()), "div");
                    }

                    return RedirectToAction("Altera", new { @Id = info.Id });
                }
                catch (Exception ex)
                {
                    if (ex is ValidationException || ex is ArgumentException)
                        ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                    else
                        throw ex;
                }
            }
            
            return View(dados);
        }

        [HttpGet]
        [FormatarViewFilter]
        [FormatarViewXml("xml", ViewData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", ViewData = new string[] { "Mensagem" })]
        [FormatarViewHtml("html", "_AlteraForm", "_LayoutEmpty")]
        public ActionResult Altera(long id = 0)
        {
            Usuario info = Services.UsuarioService.Info(id);

            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            AlteraForm dados = Mapper.Map<Usuario, AlteraForm>(info);
            PermissaoService.SobreUsuario(info);
            
            ViewBag.Info = info;
            
            return View(dados);
        }

        [HttpPost]
        [FormatarViewFilter]
        [FormatarViewXml("xml", ViewData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", ViewData = new string[] { "Mensagem" })]
        [FormatarViewHtml("html", "_AlteraForm", "_LayoutEmpty")]
        public ActionResult Altera(AlteraForm dados = null)
        {
            Usuario info = Services.UsuarioService.Info(dados.Id);
            
            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            PermissaoService.SobreUsuario(info);

            if (ModelState.IsValid)
            {
                try
                {
                    var Usuario = Services.UsuarioService.Alterar(dados);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro alterado com sucesso");
                    return RedirectToAction("Altera", new { @Id = Usuario.Id });
                }
                catch (Exception ex)
                {
                    if (ex is ValidationException || ex is ArgumentException)
                        ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                    else
                        throw ex;
                }
            }

            ViewBag.Info = info;
            
            return View(dados);
        }

        [FormatarViewFilter]
        [FormatarViewXml("xml", ViewData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", ViewData = new string[] { "Mensagem" })]
        [FormatarViewHtml("html", "_InfoBody", "_LayoutEmpty")]
        public ActionResult Info(long id = 0)
        {
            Usuario info = Services.UsuarioService.Info(id);
            
            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");
            
            return View(info);
        }
        
        [HttpGet]
        public ActionResult SolicitaAutenticacao(long id = 0)
        {
            try
            {
                Usuario usuario = Services.UsuarioService.Info(id);

                if (object.Equals(usuario, null) || usuario.Id <= 0)
                    throw new HttpException(404, "O registro solicitado não foi encontrado");
                
                Services.MinhaContaService.SolicitarAutenticar(usuario);                
                TempData["Mensagem"] = AlertsMessages.Success("E-mail solicitando autenticação enviado com sucesso");
            }
            catch (Exception ex)
            {
                if (ex is ValidationException || ex is ArgumentException)
                    TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
                else
                    TempData["Mensagem"] = AlertsMessages.Danger(string.Format("<p>Ocorreu um erro ao enviar o e-mail de autenticação: <strong>{0}</strong><p><pre>{1}</pre>", ex.Message.ToString(), ex.StackTrace.ToString()), "div");
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        [FormatarViewFilter]
        [FormatarViewXml("xml", TempData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", TempData = new string[] { "Mensagem" })]
        public ActionResult Ativa(IList<long> id)
        {            
            try
            {
                IList<Usuario> usuarios = Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario(usuarios, true);

                Services.UsuarioService.Ativar(id);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) ativado(s) com sucesso");
            }
            catch (Exception ex)
            {
                if (ex is ValidationException || ex is ArgumentException || ex is HttpException)
                    TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
                else
                    throw ex;
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        [FormatarViewFilter]
        [FormatarViewXml("xml", TempData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", TempData = new string[] { "Mensagem" })]
        public ActionResult Desativa(IList<long> id)
        {
            try
            {
                IList<Usuario> usuarios = Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario(usuarios, true);

                Services.UsuarioService.Desativar(id);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) desativado(s) com sucesso");
            }
            catch (Exception ex)
            {
                if (ex is ValidationException || ex is ArgumentException || ex is HttpException)
                    TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
                else
                    throw ex;
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        [FormatarViewFilter]
        [FormatarViewXml("xml", TempData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", TempData = new string[] { "Mensagem" })]
        public ActionResult Exclue(IList<long> id)
        {
            try
            {
                IList<Usuario> usuarios = Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario(usuarios, true);

                Services.UsuarioService.Excluir(id);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) excluído(s) com sucesso");
            }
            catch (Exception ex)
            {
                if (ex is ValidationException || ex is ArgumentException || ex is HttpException)
                    TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
                else
                    throw ex;
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        [FormatarViewFilter]
        [FormatarViewXml("xml", TempData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", TempData = new string[] { "Mensagem" })]
        public ActionResult AlteraNivel(IList<long> id, Nivel nivel)
        {
            try
            {
                Usuario minhaConta = MinhaConta.Instance.Info;
                IList<Usuario> usuarios = Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario(usuarios, true);
                
                if (nivel == Nivel.Indefinido)
                    throw new ValidationException("Um valor de nível não foi selecionado");

                Usuario modelo = (Usuario)minhaConta.Clone();
                modelo.Nivel = nivel;
                ContaAcesso acesso = new ContaAcesso(modelo);
                if (!MinhaConta.Instance.Acesso.AutorizadoSobre(modelo))
                    throw new ValidationException("Seu nível de acesso não permite aplicar esse nível de acesso");

                Services.UsuarioService.AlterarNivel(id, nivel);
                TempData["Mensagem"] = AlertsMessages.Success("Nível dos registro(s) alterados(s) com sucesso");
            }
            catch(Exception ex)
            {
                if (ex is ValidationException || ex is ArgumentException || ex is HttpException)
                    TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
                else
                    throw ex;
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }
    }
}