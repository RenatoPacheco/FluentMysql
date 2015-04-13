using AutoMapper;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Models.Services;
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

namespace FluentMysql.Site.Areas.Admin.Controllers
{
    [AuthorizeUser(Nivel = new Nivel[] { Nivel.Operador })]
    public class ArtigoController : Controller
    {
        public ActionResult Index(FiltroForm filtro = null, bool voltar = false, bool json = false, bool xml = false)
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
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
                catch (ArgumentException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }

                if (voltar && !object.Equals(Session[ViewBag.ActionRef], null))
                    filtro = (FiltroForm)Session[ViewBag.ActionRef];

                Session[ViewBag.ActionRef] = filtro;
                lista = ArtigoService.Filtrar(filtro);
            }

            if (xml)
                return ConverteResultadoService.ParaXml(lista, filtro, (string)ViewBag.Memsagem);
            else if (json)
                return ConverteResultadoService.ParaJson(lista, filtro, (string)ViewBag.Memsagem);
            
            ViewBag.Lista = lista;            
            return View(filtro);
        }

        [HttpGet]
        public ActionResult Insere()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insere(InsereForm dados)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Artigo info = ArtigoService.Inserir(dados, (Usuario)ViewBag.MinhaConta);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro inserido com sucesso");
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

            Artigo info = ArtigoService.Info(id);
            AlteraForm dados = info == null ? new AlteraForm() : Mapper.Map<Artigo, AlteraForm>(info);

            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            ViewBag.Info = info;
            return View(dados);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Altera(AlteraForm dados)
        {
            Artigo info = ArtigoService.Info(dados.Id);

            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            if (ModelState.IsValid)
            {
                try
                {
                    var Artigo = ArtigoService.Alterar(dados, (Usuario)ViewBag.MinhaConta);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro alterado com sucesso");
                    return RedirectToAction("Altera", new { @Id = Artigo.Id });
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

        public ActionResult Ativa(IList<long> id)
        {
            try
            {
                ArtigoService.Ativar(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) ativado(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        public ActionResult Desativa(IList<long> id)
        {
            try
            {
                ArtigoService.Desativar(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) desativado(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        public ActionResult Exclue(IList<long> id)
        {
            try
            {
                ArtigoService.Excluir(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) excluído(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }
    }
}