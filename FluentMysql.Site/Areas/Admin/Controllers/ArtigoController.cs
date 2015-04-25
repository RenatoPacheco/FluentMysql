using AutoMapper;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Services;
using FluentMysql.Site.Areas.Admin.ViewsData.Artigo;
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
using FluentMysql.Site.Web.Mvc;
using System.Xml.Linq;
using FluentMysql.Infrastructure;
using System.Text;
using FluentMysql.Site.Services;
using FluentMysql.Site.DataAnnotations;
using FluentMysql.Domain;

namespace FluentMysql.Site.Areas.Admin.Controllers
{
    [AuthorizeUser(Nivel = Nivel.Operador)]
    public class ArtigoController : Controller
    {

        [FormatarViewFilter]
        [FormatarViewXml("xml", ViewData = new string[] { "Mensagem", "Lista" })]
        [FormatarViewJson("json", ViewData = new string[] { "Mensagem", "Lista" })]
        [FormatarViewHtml("html", "_IndexTBody", "_LayoutEmpty")]
        public ActionResult Index(FiltroForm filtro = null, bool voltar = false)
        {
            string ids;
            IList<Artigo> lista = new List<Artigo>();
            CommonRouteData routeData = new CommonRouteData(ControllerContext);
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (filtro.Acao == "ativar")
                    {
                        ids = string.Join("&", filtro.Selecionados.Select(x => string.Format("id={0}", x)));
                        return Redirect(string.Format("Artigo/Ativa/?{0}", ids));
                    }
                    else if (filtro.Acao == "desativar")
                    {
                        ids = string.Join("&", filtro.Selecionados.Select(x => string.Format("id={0}", x)));
                        return Redirect(string.Format("Artigo/Desativa/?{0}", ids));
                    }
                    else if (filtro.Acao == "excluir")
                    {
                        ids = string.Join("&", filtro.Selecionados.Select(x => string.Format("id={0}", x)));
                        return Redirect(string.Format("Artigo/Exclue/?{0}", ids));
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
                lista = ArtigoService.Filtrar(filtro);
            }
            
            ViewBag.Lista = lista;            
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
        [ValidateInput(false)]
        [FormatarViewFilter]
        [FormatarViewXml("xml", ViewData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", ViewData = new string[] { "Mensagem" })]
        [FormatarViewHtml("html", "_InsereForm", "_LayoutEmpty")]
        public ActionResult Insere(InsereForm dados)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Artigo info = ArtigoService.Inserir(dados, MinhaConta.Instance.Info);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro inserido com sucesso");
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
        [FormatarViewHtml("html", "_InsereForm", "_LayoutEmpty")]
        public ActionResult Altera(long id = 0)
        {

            Artigo info = ArtigoService.Info(id);
            AlteraForm dados = info == null ? new AlteraForm() : Mapper.Map<Artigo, AlteraForm>(info);

            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            ViewBag.Info = info;
            return View(dados);
        }

        [HttpPost]
        [ValidateInput(false)]
        [FormatarViewFilter]
        [FormatarViewXml("xml", ViewData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", ViewData = new string[] { "Mensagem" })]
        [FormatarViewHtml("html", "_InsereForm", "_LayoutEmpty")]
        public ActionResult Altera(AlteraForm dados)
        {
            Artigo info = ArtigoService.Info(dados.Id);

            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            if (ModelState.IsValid)
            {
                try
                {
                    var Artigo = ArtigoService.Alterar(dados, MinhaConta.Instance.Info);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro alterado com sucesso");
                    return RedirectToAction("Altera", new { @Id = Artigo.Id });
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
        [FormatarViewXml("xml", TempData = new string[] { "Mensagem" })]
        [FormatarViewJson("json", TempData = new string[] { "Mensagem" })]
        public ActionResult Ativa(IList<long> id)
        {
            try
            {
                ArtigoService.Ativar(id, MinhaConta.Instance.Info);
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
                ArtigoService.Desativar(id, MinhaConta.Instance.Info);
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
                ArtigoService.Excluir(id, MinhaConta.Instance.Info);
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
    }
}